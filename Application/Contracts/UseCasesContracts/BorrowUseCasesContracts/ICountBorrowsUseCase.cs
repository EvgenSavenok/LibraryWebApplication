using Application.RequestFeatures;
using Domain.Entities.RequestFeatures;

namespace Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;

public interface ICountBorrowsUseCase
{
    public Task<int> ExecuteAsync(BorrowParameters borrowParameters);
}
