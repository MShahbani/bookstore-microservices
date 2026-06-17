using Catalog.Application.Interfaces;
using MediatR;

namespace Catalog.Application.Features.Books.Commands.DeleteBook;

public class DeleteBookCommandHandler(IBookRepository bookRepository, ICacheService cacheService, IUnitOfWork unitOfWork)
    : IRequestHandler<DeleteBookCommand, bool>
{
    public async Task<bool> Handle(DeleteBookCommand request, CancellationToken cancellationToken)
    {
        var book = await bookRepository.GetByIdAsync(request.Id, cancellationToken);

        if (book is null)
        {
            return false;
        }
        
        bookRepository.Delete(book);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        await cacheService.RemoveAsync($"book:{request.Id}", cancellationToken);
        
        return true;
    }
}