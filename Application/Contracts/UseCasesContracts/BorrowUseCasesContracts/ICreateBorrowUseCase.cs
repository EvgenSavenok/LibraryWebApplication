using Application.DataTransferObjects;

namespace Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;

public interface ICreateBorrowUseCase
{
    public Task ExecuteAsync(string userId, int bookId, BookDto bookDto);
}
