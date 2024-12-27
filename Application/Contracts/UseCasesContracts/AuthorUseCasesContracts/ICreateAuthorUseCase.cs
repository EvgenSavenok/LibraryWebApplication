using Application.DataTransferObjects;

namespace Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;

public interface ICreateAuthorUseCase
{
    public Task ExecuteAsync(AuthorForCreationDto author, CancellationToken token);
}
