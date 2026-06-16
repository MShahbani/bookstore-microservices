using Order.Domain.Enums;

namespace Order.Domain.Entities;

public class Order
{
    public Guid Id { get; private set; }
    public Guid BookId { get; private set; }
    public int Quantity { get; private set; }
    public OrderStatus Status { get; private set; }

    private Order()
    {
    }

    public Order(Guid bookId, int quantity)
    {
        if (bookId == Guid.Empty)
        {
            throw new ArgumentException("BookId cannot be empty.", nameof(bookId));
        }

        if (quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(quantity), "Quantity must be greater than zero.");
        }

        Id = Guid.NewGuid();
        BookId = bookId;
        Quantity = quantity;
        Status = OrderStatus.Pending;
    }

    public void Confirm()
    {
        if (Status != OrderStatus.Pending)
        {
            return;
        }

        Status = OrderStatus.Confirmed;
    }

    public void Fail()
    {
        if (Status != OrderStatus.Pending)
        {
            return;
        }

        Status = OrderStatus.Failed;
    }
}