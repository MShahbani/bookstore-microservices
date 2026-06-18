using MediatR;
using Order.Application.DTOs;
using Order.Application.Interfaces;
using Order.Application.Mappers;

namespace Order.Application.Features.Orders.Queries.GetAllOrders;

public class GetAllOrdersQueryHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetAllOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await orderRepository.GetAllAsync(cancellationToken);
        return orders.Select(x => x.ToDto()).ToList();
    }
}