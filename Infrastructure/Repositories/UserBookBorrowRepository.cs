using Domain.Contracts;
using Domain.Entities;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Repository.Repositories;

public class UserBookBorrowRepository : RepositoryBase<UserBookBorrow>, IUserBookBorrowRepository
{
    public UserBookBorrowRepository(ApplicationContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public async Task<IEnumerable<UserBookBorrow>> GetAllUserBookBorrowsAsync(bool trackChanges) =>
        await FindAll(trackChanges).ToListAsync();

    public async Task<UserBookBorrow> GetUserBookBorrowAsync(int id, bool trackChanges) =>
        await FindByCondition(b => b.Id == id, trackChanges).SingleOrDefaultAsync();

    public async Task<IEnumerable<UserBookBorrow>> GetAllUserBookBorrowsAsync
        (BorrowParameters borrowParameters, string userId, bool trackChanges)
    {
        return await FindByCondition(borrow => borrow.UserId == userId, trackChanges)
            .Include(borrow => borrow.Book)
            .ThenInclude(book => book.Author)
            .Skip((borrowParameters.PageNumber - 1) * borrowParameters.PageSize)
            .Take(borrowParameters.PageSize)
            .ToListAsync();
    }
    
    public async Task<int> CountBorrowsAsync(BorrowParameters borrowParameters)
    {
        var query = FindByCondition(b => true, trackChanges: false);
        return await query.CountAsync();
    }
}

