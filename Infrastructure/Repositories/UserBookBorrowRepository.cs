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

    public async Task<UserBookBorrow> GetUserBookBorrowAsync(int id, bool trackChanges, CancellationToken cancellationToken)
    {
        var borrow = await FindByCondition(b => b.BookId == id, trackChanges, cancellationToken);
        return borrow.SingleOrDefault();
    }

    public async Task<IEnumerable<UserBookBorrow>> GetAllUserBookBorrowsAsync(BorrowParameters borrowParameters, string userId,
        bool trackChanges, CancellationToken cancellationToken)
    {
        var specification = new BorrowSpecification(borrowParameters, userId);
        return await GetBySpecificationAsync(specification, trackChanges, cancellationToken);
    }
    
    public async Task<int> CountBorrowsAsync(BorrowParameters borrowParameters, CancellationToken cancellationToken)
    {
        var specification = new BorrowSpecification(borrowParameters, null);
        var borrows = await GetBySpecificationAsync(specification, trackChanges: false, cancellationToken);
        return borrows.Count();
    }
}

