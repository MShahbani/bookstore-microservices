using MediatR;

namespace Catalog.Application.Features.Books.Commands.UpdateBook;

public record UpdateBookCommand(
    Guid Id,
    string Title,
    string Author,
    decimal Price,
    int Stock
) : IRequest<bool>;