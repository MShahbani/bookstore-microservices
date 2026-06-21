using MediatR;
using Order.Application.Interfaces;
using Order.Domain.Entities;
using Shared.Contracts.Events;

namespace Order.Application.Features.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IEventPublisher eventPublisher): IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = new OrderEntity(request.BookId, request.Quantity);

        await orderRepository.AddAsync(order, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        var orderCreatedEvent = new OrderCreatedEvent(
            order.Id,
            order.BookId,
            order.Quantity
        );

        await eventPublisher.PublishAsync(orderCreatedEvent, cancellationToken);

        return order.Id;
    }
}