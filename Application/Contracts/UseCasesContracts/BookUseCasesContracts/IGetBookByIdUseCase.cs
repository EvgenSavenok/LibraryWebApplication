using Application.DataTransferObjects;

namespace Application.Contracts.UseCasesContracts.BookUseCasesContracts;

public interface IGetBookByIdUseCase
{
    public Task<BookDto> ExecuteAsync(int bookId);
}
