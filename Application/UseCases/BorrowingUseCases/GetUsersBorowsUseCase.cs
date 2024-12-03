using Application.Contracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.Validation;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Application.UseCases.BorrowingUseCases;

public class GetUsersBorowsUseCase : IGetUsersBorowsUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public GetUsersBorowsUseCase(IRepositoryManager repository,
        ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<IEnumerable<UserBookBorrow>> ExecuteAsync(BorrowParameters requestParameters, string userId)
    {
        Task<IEnumerable<UserBookBorrow>> borrowsAsync = _repository.Borrow.GetAllUserBookBorrowsAsync(requestParameters, userId, trackChanges: false);
        if (borrowsAsync == null)
        {
            _logger.LogInfo("Cannot count number of borrows.");
            throw new ConflictException("Cannot count number of borrows.");
        }
        return await borrowsAsync;
    }
}
