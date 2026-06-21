using Catalog.Application.Interfaces;
using Catalog.Infrastructure.Caching;
using Catalog.Infrastructure.Messaging;
using Catalog.Infrastructure.Messaging.Consumers;
using Catalog.Infrastructure.Persistence;
using Catalog.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using IUnitOfWork = Catalog.Application.Interfaces.IUnitOfWork;

namespace Catalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<CatalogDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("CatalogDb")));

        services.AddScoped<IBookRepository, BookRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddSingleton<IEventPublisher, RabbitMqEventPublisher>();
        services.AddSingleton<ICacheService, RedisCacheService>();

        services.AddHostedService<OrderCreatedEventConsumer>();

        return services;
    }
}