using Application.DataTransferObjects;
using Domain.Entities.RequestFeatures;

namespace Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;

public interface IGetAllAuthorsUseCase
{
    public Task<IEnumerable<AuthorDto>> ExecuteAsync(AuthorParameters requestParameters);
}
