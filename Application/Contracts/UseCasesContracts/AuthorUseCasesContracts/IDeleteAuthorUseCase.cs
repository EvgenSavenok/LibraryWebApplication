namespace Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;

public interface IDeleteAuthorUseCase
{
    public Task ExecuteAsync(int id);
}
