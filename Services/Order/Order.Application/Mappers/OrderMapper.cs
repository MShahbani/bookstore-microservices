using Order.Application.DTOs;
using Order.Domain.Entities;

namespace Order.Application.Mappers;

public static class OrderMapper
{
    public static OrderDto ToDto(this OrderEntity order)
    {
        ArgumentNullException.ThrowIfNull(order);

        return new OrderDto
        {
            Id = order.Id,
            BookId = order.BookId,
            Quantity = order.Quantity,
            Status = order.Status.ToString()
        };
    }
}

