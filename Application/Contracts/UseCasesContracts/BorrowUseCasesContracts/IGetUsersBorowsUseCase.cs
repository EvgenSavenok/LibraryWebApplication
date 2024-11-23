using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;

public interface IGetUsersBorowsUseCase
{
    public Task<IEnumerable<UserBookBorrow>> ExecuteAsync(BorrowParameters requestParameters,
        string userId);
}
