using Application.RequestFeatures;
using Domain.Models;

namespace Application.Contracts.RepositoryContracts;

public interface IUserBookBorrowRepository : IRepositoryBase<UserBookBorrow>
{
    public Task<IEnumerable<UserBookBorrow>> GetAllUserBookBorrowsAsync(BorrowParameters requestParameters, string userId, 
        bool trackChanges, CancellationToken cancellation);
    public Task<UserBookBorrow> GetUserBookBorrowAsync(int id, bool trackChanges, CancellationToken cancellation);
    public Task<int> CountBorrowsAsync(BorrowParameters borrowParameters, CancellationToken cancellationToken);
}
