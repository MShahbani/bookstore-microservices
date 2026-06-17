using Catalog.Application.DTOs;
using Catalog.Domain.Entities;

namespace Catalog.Application.Mappers;

public static class BookMapper
{
    public static BookDto ToDto(this Book book)
    {
        ArgumentNullException.ThrowIfNull(book);

        return new BookDto
        {
            Id = book.Id,
            Title = book.Title,
            Author = book.Author,
            Price = book.Price,
            Stock = book.Stock
        };
    }
}