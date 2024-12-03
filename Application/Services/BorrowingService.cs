using Application.Contracts;
using Application.Contracts.ServicesContracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Application.Services;

public class BorrowingService : IBookingService
{
    private readonly ICountBorrowsUseCase _countBorrowsUseCase;
    private readonly ICreateBorrowUseCase _createBorrowUseCase;
    private readonly IGetUserBorrowUseCase _getUserBorrowUseCase;
    private readonly IGetUsersBorowsUseCase _getUsersBorowsUseCase;
    
    public BorrowingService(
        ICountBorrowsUseCase countBorrowsUseCase,
        ICreateBorrowUseCase createBorrowUseCase,
        IGetUserBorrowUseCase getUserBorrowUseCase,
        IGetUsersBorowsUseCase getUsersBorowsUseCase)
    {
        _countBorrowsUseCase = countBorrowsUseCase;
        _createBorrowUseCase = createBorrowUseCase;
        _getUsersBorowsUseCase = getUsersBorowsUseCase;
        _getUserBorrowUseCase = getUserBorrowUseCase;
    }

    public async Task<PagedResult<UserBookBorrow>> GetAllUserBookBorrowsAsync(BorrowParameters requestParameters, string userId)
        => await _getUsersBorowsUseCase.ExecuteAsync(requestParameters, userId);

    public async Task<UserBookBorrowDto> GetUserBookBorrowAsync(int id)
        => await _getUserBorrowUseCase.ExecuteAsync(id);

    public async Task CreateUserBookBorrowAsync(UserBookBorrowDto borrowDto)
        => await _createBorrowUseCase.ExecuteAsync(borrowDto);

    public async Task<int> CountBorrowsAsync(BorrowParameters borrowParameters)
        => await _countBorrowsUseCase.ExecuteAsync(borrowParameters);
}