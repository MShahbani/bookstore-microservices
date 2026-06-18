using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Infrastructure.Repositories;

public class BookRepository(CatalogDbContext dbContext) : IBookRepository
{
    public async Task AddAsync(Book book, CancellationToken cancellationToken = default)
    {
        await dbContext.Books.AddAsync(book, cancellationToken);
    }

    public async Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Books.FirstOrDefaultAsync(book => book.Id == id, cancellationToken);
    }

    public async Task<List<Book>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await dbContext.Books.ToListAsync(cancellationToken);
    }

    public void Update(Book book)
    {
        dbContext.Books.Update(book);
    }

    public void Delete(Book book)
    {
        dbContext.Books.Remove(book);
    }
}