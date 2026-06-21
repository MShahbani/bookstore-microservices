namespace Shared.Contracts.Events;

public sealed record StockFailedEvent
(
    Guid OrderId,
    Guid BookId,
    int Quantity,
    string Reason
);