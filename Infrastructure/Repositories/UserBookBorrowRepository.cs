using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.RequestFeatures;
using Application.Specifications;
using Domain.Entities.RequestFeatures;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Infrastructure.Repositories;

public class UserBookBorrowRepository : RepositoryBase<UserBookBorrow>, IUserBookBorrowRepository
{
    public UserBookBorrowRepository(ApplicationContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public async Task<UserBookBorrow> GetUserBookBorrowAsync(int id, bool trackChanges)
    {
        var borrow = await FindByCondition(b => b.BookId == id, trackChanges);
        return borrow.SingleOrDefault();
    }

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

