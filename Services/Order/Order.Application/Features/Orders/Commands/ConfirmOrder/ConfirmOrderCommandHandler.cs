using MediatR;
using Order.Application.Interfaces;

namespace Order.Application.Features.Orders.Commands.ConfirmOrder;

public class ConfirmOrderCommandHandler(
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<ConfirmOrderCommand>
{
    public async Task Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);

        if (order is null)
        {
            throw new KeyNotFoundException("Order not found.");
        }

        order.Confirm();
        orderRepository.Update(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);
    }
}