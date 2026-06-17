using Catalog.Application.Interfaces;
using MediatR;

namespace Catalog.Application.Features.Books.Commands.UpdateBook;

public class UpdateBookCommandHandler(
    IBookRepository bookRepository,
    ICacheService cacheService,
    IUnitOfWork unitOfWork) : IRequestHandler<UpdateBookCommand, bool>
{
    public async Task<bool> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetByIdAsync(request.Id, cancellationToken);

        if (book is null)
            return false;

        book.Update(request.Title, request.Author, request.Price, request.Stock);

        bookRepository.Update(book);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        await cacheService.RemoveAsync($"book:{request.Id}", cancellationToken);

        return true;
    }
}