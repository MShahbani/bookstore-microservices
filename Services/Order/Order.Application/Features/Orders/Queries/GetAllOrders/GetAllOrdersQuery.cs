using MediatR;
using Order.Application.DTOs;

namespace Order.Application.Features.Orders.Queries.GetAllOrders;

public record GetAllOrdersQuery : IRequest<List<OrderDto>>;