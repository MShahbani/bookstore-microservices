using System.Text;
using System.Text.Json;
using Catalog.Application.Interfaces;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Contracts.Events;

namespace Catalog.Infrastructure.Messaging.Consumers;

public class OrderCreatedEventConsumer(
    IServiceScopeFactory scopeFactory,
    IConfiguration configuration)
    : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;

    private const string ExchangeName = "bookstore.events";
    private const string QueueName = "catalog.order-created";
    private const string RoutingKey = "order.created";

    public override async Task StartAsync(CancellationToken cancellationToken)
    {
        var host = configuration["RabbitMQ:Host"] ?? "localhost";

        var factory = new ConnectionFactory
        {
            HostName = host
        };

        _connection = await factory.CreateConnectionAsync(cancellationToken);
        _channel = await _connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await _channel.ExchangeDeclareAsync(
            exchange: ExchangeName,
            type: ExchangeType.Topic,
            durable: true,
            cancellationToken: cancellationToken);

        await _channel.QueueDeclareAsync(
            queue: QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null,
            cancellationToken: cancellationToken);

        await _channel.QueueBindAsync(
            queue: QueueName,
            exchange: ExchangeName,
            routingKey: RoutingKey,
            cancellationToken: cancellationToken);

        await base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (_channel is null)
            return;

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (_, ea) =>
        {
            try
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);

                var message = JsonSerializer.Deserialize<OrderCreatedEvent>(json);

                if (message is null)
                {
                    await _channel.BasicNackAsync(
                        ea.DeliveryTag,
                        multiple: false,
                        requeue: false,
                        cancellationToken: stoppingToken);
                    return;
                }

                using var scope = scopeFactory.CreateScope();

                var dbContext = scope.ServiceProvider.GetRequiredService<CatalogDbContext>();
                var eventPublisher = scope.ServiceProvider.GetRequiredService<IEventPublisher>();
                var cacheService = scope.ServiceProvider.GetRequiredService<ICacheService>();

                var book = await dbContext.Books
                    .FirstOrDefaultAsync(x => x.Id == message.BookId, stoppingToken);

                if (book is null)
                {
                    await eventPublisher.PublishAsync(
                        new StockFailedEvent(
                            message.OrderId,
                            message.BookId,
                            message.Quantity,
                            "Book not found"),
                        stoppingToken);

                    await _channel.BasicAckAsync(
                        ea.DeliveryTag,
                        multiple: false,
                        cancellationToken: stoppingToken);

                    return;
                }

                if (!book.HasEnoughStock(message.Quantity))
                {
                    await eventPublisher.PublishAsync(
                        new StockFailedEvent(
                            message.OrderId,
                            message.BookId,
                            message.Quantity,
                            "Insufficient stock"),
                        stoppingToken);

                    await _channel.BasicAckAsync(
                        ea.DeliveryTag,
                        multiple: false,
                        cancellationToken: stoppingToken);

                    return;
                }

                book.ReserveStock(message.Quantity);
                await dbContext.SaveChangesAsync(stoppingToken);

                await cacheService.RemoveAsync($"book:{book.Id}", stoppingToken);

                await eventPublisher.PublishAsync(
                    new StockReservedEvent(
                        message.OrderId,
                        message.BookId,
                        message.Quantity),
                    stoppingToken);

                await _channel.BasicAckAsync(
                    ea.DeliveryTag,
                    multiple: false,
                    cancellationToken: stoppingToken);
            }
            catch
            {
                if (_channel is not null)
                {
                    await _channel.BasicNackAsync(
                        ea.DeliveryTag,
                        multiple: false,
                        requeue: true,
                        cancellationToken: stoppingToken);
                }
            }
        };

        await _channel.BasicConsumeAsync(
            queue: QueueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken);
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_channel is not null && _channel.IsOpen)
            await _channel.CloseAsync(cancellationToken);

        if (_connection is not null && _connection.IsOpen)
            await _connection.CloseAsync(cancellationToken);

        await base.StopAsync(cancellationToken);
    }
}
