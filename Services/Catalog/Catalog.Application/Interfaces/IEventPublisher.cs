namespace Catalog.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync<T>(T @event, CancellationToken cancellationToken = default);
}