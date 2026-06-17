using MediatR;
using Order.Application.Interfaces;
using Order.Domain.Entities;

namespace Order.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork): IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new OrderEntity(request.BookId, request.Quantity);

        await orderRepository.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return order.Id;
    }
}