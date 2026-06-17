using Catalog.Application.DTOs;
using MediatR;

namespace Catalog.Application.Features.Books.Queries.GetAllBooks;

public record GetAllBooksQuery(): IRequest<List<BookDto>>;