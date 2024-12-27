using Domain.Entities.RequestFeatures;

namespace Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;

public interface ICountAuthorsUseCase
{
    public Task<int> ExecuteAsync(AuthorParameters requestParameters, CancellationToken cancellationToken);
}
