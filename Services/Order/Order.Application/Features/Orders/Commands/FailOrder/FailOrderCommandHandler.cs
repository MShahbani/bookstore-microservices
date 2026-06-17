using MediatR;
using Order.Application.Interfaces;

namespace Order.Application.Features.Orders.Commands.FailOrder;

public class FailOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<FailOrderCommand>
{
    public async Task Handle(FailOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        order.Fail();
        orderRepository.Update(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}