using MediatR;
using Order.Application.DTOs;

namespace Order.Application.Features.Orders.Queries.GetOrderById;

public record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto?>;