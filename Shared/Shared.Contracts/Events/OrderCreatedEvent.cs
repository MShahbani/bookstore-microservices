namespace Shared.Contracts.Events;

public sealed record OrderCreatedEvent
(
    Guid OrderId,
    Guid BookId,
    int Quantity
);