using Catalog.Application.Interfaces;
using Catalog.Domain.Entities;
using MediatR;

namespace Catalog.Application.Features.Books.Commands.CreateBook;

public class CreateBookCommandHandler(
    IBookRepository bookRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<CreateBookCommand, Guid>
{
    public async Task<Guid> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var book = new Book(
            request.Title,
            request.Author,
            request.Price,
            request.Stock);

        await bookRepository.AddAsync(book, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return book.Id;
    }
}