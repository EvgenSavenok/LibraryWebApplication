using Application.DataTransferObjects;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Application.Contracts.ServicesContracts;

public interface IBookingService
{
    Task<PagedResult<UserBookBorrow>> GetAllUserBookBorrowsAsync(BorrowParameters requestParameters, string userId);
    Task<UserBookBorrowDto> GetUserBookBorrowAsync(int id);
    Task CreateUserBookBorrowAsync(UserBookBorrowDto borrowDto);
    Task<int> CountBorrowsAsync(BorrowParameters borrowParameters);
}
