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
        var cacheBook = await cacheService.GetAsync<BookDto?>(cacheKey, cancellationToken);
        if (cacheBook is null)
            return cacheBook;
        
        
        var book = await bookRepository.GetByIdAsync(request.Id, cancellationToken);
        if (book is null)
            return null;

        var dto = book.ToDto();
        await cacheService.SetAsync(cacheKey, dto, TimeSpan.FromMinutes(5), cancellationToken);
        
        return dto;
    }
}