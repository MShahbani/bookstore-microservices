using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Books.Queries.GetBookById;

public record GetBookByIdQuery(Guid Id) : IRequest<BookDto?>;