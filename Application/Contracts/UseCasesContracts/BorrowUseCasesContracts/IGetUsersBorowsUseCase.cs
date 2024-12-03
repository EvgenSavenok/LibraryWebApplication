using Application.DataTransferObjects;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;

public interface IGetUsersBorowsUseCase
{
    public Task<PagedResult<UserBookBorrow>> ExecuteAsync(BorrowParameters borrowParameters,
        string userId);
}
