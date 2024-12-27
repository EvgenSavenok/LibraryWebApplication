namespace Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;

public interface ITakeBookControllerUseCase
{
    public Task ExecuteAsync(int bookId, CancellationToken cancellationToken);
}
