using Catalog.Domain.Entities;

namespace Catalog.Application.Interfaces;

public interface IBookRepository
{
    Task AddAsync(Book book, CancellationToken cancellationToken  = default);
    Task<Book?> GetByIdAsync(Guid id, CancellationToken cancellationToken  = default);
    Task<List<Book>> GetAllAsync(CancellationToken cancellationToken  = default);
    void Update(Book book);
    void Delete(Book book);
}