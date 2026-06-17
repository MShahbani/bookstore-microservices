using MediatR;

namespace Order.Application.Features.Orders.Commands.CreateOrder;

public record CreateOrderCommand(
    Guid BookId,
    int Quantity): IRequest<Guid>;