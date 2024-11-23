using Application.DataTransferObjects;

namespace Application.Contracts.UseCasesContracts.BookUseCasesContracts;

public interface IUpdateBookUseCase
{
    public Task ExecuteAsync(int bookId, BookForUpdateDto bookDto);
}
