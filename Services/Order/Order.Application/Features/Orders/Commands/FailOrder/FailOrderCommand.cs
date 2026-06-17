using MediatR;

namespace Order.Application.Features.Orders.Commands.FailOrder;

public record FailOrderCommand(Guid OrderId) : IRequest;