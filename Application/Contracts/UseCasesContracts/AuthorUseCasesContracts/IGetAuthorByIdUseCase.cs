using Application.DataTransferObjects;

namespace Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;

public interface IGetAuthorByIdUseCase
{
    public Task<AuthorDto> ExecuteAsync(int id);
}
