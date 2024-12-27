namespace Application.Contracts.UseCasesContracts.BookUseCasesContracts;

public interface IDeleteBookUseCase
{
    public Task ExecuteAsync(int bookId, CancellationToken cancellationToken);
}
