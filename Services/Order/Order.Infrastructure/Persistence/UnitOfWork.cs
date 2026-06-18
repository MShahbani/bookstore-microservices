using Order.Application.Interfaces;

namespace Order.Infrastructure.Persistence;

public class UnitOfWork(OrderDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}