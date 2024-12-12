using Domain.Contracts;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;
using Domain.Entities.Specifications;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Infrastructure.Repositories;

public class UserBookBorrowRepository : RepositoryBase<UserBookBorrow>, IUserBookBorrowRepository
{
    public UserBookBorrowRepository(ApplicationContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public async Task<UserBookBorrow> GetUserBookBorrowAsync(int id, bool trackChanges) =>
        await FindByCondition(b => b.BookId == id, trackChanges).SingleOrDefaultAsync();

    public async Task<IEnumerable<UserBookBorrow>> GetAllUserBookBorrowsAsync(BorrowParameters borrowParameters, string userId, bool trackChanges)
    {
        var specification = new BorrowSpecification(borrowParameters, userId);
        return await GetBySpecificationAsync(specification, trackChanges);
    }
    
    public async Task<int> CountBorrowsAsync(BorrowParameters borrowParameters)
    {
        var specification = new BorrowSpecification(borrowParameters, null);
        var borrows = await GetBySpecificationAsync(specification, trackChanges: false);
        return borrows.Count();
    }
}

