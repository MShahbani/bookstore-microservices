using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using Catalog.Application.Mappers;
using MediatR;

namespace Catalog.Application.Features.Books.Queries.GetBookById;

public class GetBookByIdQueryHandler(IBookRepository bookRepository, ICacheService cacheService)
    : IRequestHandler<GetBookByIdQuery, BookDto?>
{
    public async Task<BookDto?> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
    {
        var cacheKey = $"book:{request.Id}";
        var cachedBook = await cacheService.GetAsync<BookDto?>(cacheKey, cancellationToken);

        if (cachedBook is not null)
            return cachedBook;

        var book = await bookRepository.GetByIdAsync(request.Id, cancellationToken);
        if (book is null)
            return null;

        var dto = book.ToDto();
        await cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5), cancellationToken);

        return dto;
    }
}