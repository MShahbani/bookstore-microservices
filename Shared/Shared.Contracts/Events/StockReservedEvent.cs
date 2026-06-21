namespace Shared.Contracts.Events;

public sealed record StockReservedEvent
(
    Guid OrderId,
    Guid BookId,
    int Quantity
);