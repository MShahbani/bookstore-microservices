using MediatR;
using Order.Application.DTOs;
using Order.Application.Interfaces;
using Order.Application.Mappers;

namespace Order.Application.Features.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.Id, cancellationToken);

        return order?.ToDto();
    }
}
