using Catalog.Application.DTOs;
using Catalog.Application.Interfaces;
using Catalog.Application.Mappers;
using MediatR;

namespace Catalog.Application.Features.Books.Queries.GetAllBooks;

public class GetAllBooksQueryHandler(IBookRepository bookRepository) : IRequestHandler<GetAllBooksQuery, List<BookDto>>
{
    public async Task<List<BookDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await bookRepository.GetAllAsync(cancellationToken);
        return books.Select(x => x.ToDto()).ToList();
    }
}