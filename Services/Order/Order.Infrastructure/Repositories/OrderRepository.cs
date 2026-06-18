using Microsoft.EntityFrameworkCore;
using Order.Application.Interfaces;
using Order.Domain.Entities;
using Order.Infrastructure.Persistence;

namespace Order.Infrastructure.Repositories;

public class OrderRepository(OrderDbContext dbContext) : IOrderRepository
{
    public async Task AddAsync(OrderEntity orderEntity, CancellationToken cancellationToken = default)
    {
        await dbContext.Orders.AddAsync(orderEntity, cancellationToken);
    }

    public async Task<OrderEntity?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders.FirstOrDefaultAsync(order => order.Id == id, cancellationToken);
    }

    public async Task<List<OrderEntity>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Orders.ToListAsync(cancellationToken);
    }

    public void Update(OrderEntity orderEntity)
    {
        dbContext.Orders.Update(orderEntity);
    }
}