using Application.DataTransferObjects;
using Domain.Entities.RequestFeatures;

namespace Application.Contracts.UseCasesContracts.BookUseCasesContracts;

public interface IGetBooksUseCase
{
    public Task<IEnumerable<BookDto>> ExecuteAsync(BookParameters bookParameters);
}
