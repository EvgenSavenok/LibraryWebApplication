using Application.RequestFeatures;
using Domain.Entities.RequestFeatures;

namespace Application.Contracts.UseCasesContracts.BookUseCasesContracts;

public interface ICountBooksUseCase
{
    public Task<int> ExecuteAsync(BookParameters requestParameters, CancellationToken cancellationToken);
}
