using Application.Contracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Application.UseCases.BorrowingUseCases;

public class GetUsersBorowsUseCase : IGetUsersBorowsUseCase
{
    private readonly IRepositoryManager _repository;

    public GetUsersBorowsUseCase(IRepositoryManager repository)
    {
        _repository = repository;
    }
    public async Task<IEnumerable<UserBookBorrow>> ExecuteAsync(BorrowParameters requestParameters, string userId)
    {
        return await _repository.Borrow.GetAllUserBookBorrowsAsync(requestParameters, userId, trackChanges: false);
    }
}
