using MediatR;

namespace Catalog.Application.Features.Books.Commands.CreateBook;

public record CreateBookCommand(
    string Title,
    string Author,
    decimal Price,
    int Stock
) : IRequest<Guid>;