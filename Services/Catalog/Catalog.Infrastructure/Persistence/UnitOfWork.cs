using Catalog.Application.Interfaces;

namespace Catalog.Infrastructure.Persistence;

public class UnitOfWork(CatalogDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await dbContext.SaveChangesAsync(cancellationToken);
    }
}