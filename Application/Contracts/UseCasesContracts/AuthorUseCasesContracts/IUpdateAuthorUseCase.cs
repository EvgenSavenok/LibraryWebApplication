using Application.DataTransferObjects;

namespace Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;

public interface IUpdateAuthorUseCase
{
    public Task ExecuteAsync(int id, AuthorForUpdateDto author, CancellationToken token);
}
