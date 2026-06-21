using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Order.Application.Interfaces;
using RabbitMQ.Client;
using Shared.Contracts.Events;

namespace Order.Infrastructure.Messaging;

public class RabbitMqEventPublisher : IEventPublisher, IAsyncDisposable
{
    private readonly IConnection _connection;
    private readonly IChannel _channel;

    private const string ExchangeName = "bookstore.events";

    public RabbitMqEventPublisher(IConfiguration configuration)
    {
        var host = configuration["RabbitMQ:Host"] ?? "localhost";

        var factory = new ConnectionFactory
        {
            HostName = host
        };

        _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
        _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

        _channel.ExchangeDeclareAsync(
            exchange: ExchangeName,
            type: ExchangeType.Topic,
            durable: true
        ).GetAwaiter().GetResult();
    }

    public async Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default)
    {
        var routingKey = GetRoutingKey<T>();
        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

        var properties = new BasicProperties
        {
            Persistent = true,
            ContentType = "application/json",
            Type = typeof(T).Name
        };

        await _channel.BasicPublishAsync(
            exchange: ExchangeName,
            routingKey: routingKey,
            mandatory: true,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }

    private static string GetRoutingKey<T>()
    {
        return typeof(T).Name switch
        {
            nameof(OrderCreatedEvent) => "order.created",
            nameof(StockReservedEvent) => "stock.reserved",
            nameof(StockFailedEvent) => "stock.failed",
            _ => typeof(T).Name.ToLower()
        };
    }

    public async ValueTask DisposeAsync()
    {
        if (_channel.IsOpen)
            await _channel.CloseAsync();

        if (_connection.IsOpen)
            await _connection.CloseAsync();
    }
}
