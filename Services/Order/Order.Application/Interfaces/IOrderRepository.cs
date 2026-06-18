using Order.Domain.Entities;

namespace Order.Application.Interfaces;

public interface IOrderRepository
{
    Task AddAsync(OrderEntity orderEntity, CancellationToken cancellationToken = default);
    Task<OrderEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<OrderEntity>> GetAllAsync(CancellationToken cancellationToken = default);
    void Update(OrderEntity orderEntity);
}

