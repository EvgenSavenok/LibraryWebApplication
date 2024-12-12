namespace Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;

public interface IReturnBookUseCase
{
    public Task ExecuteAsync(int bookId);
}
