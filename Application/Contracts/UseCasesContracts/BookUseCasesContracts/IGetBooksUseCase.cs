using Application.DataTransferObjects;
using Domain.Entities.RequestFeatures;

namespace Application.Contracts.UseCasesContracts.BookUseCasesContracts;

public interface IGetBooksUseCase
{
    public Task<PagedResult<BookDto>> ExecuteAsync(BookParameters bookParameters);
}
