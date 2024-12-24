using Application.Contracts.RepositoryContracts;
using Application.RequestFeatures;
using Application.Specifications;
using Domain.Entities.RequestFeatures;
using Domain.Models;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Infrastructure.Repositories;

public class BookRepository : RepositoryBase<Book>, IBookRepository
{
    public BookRepository(ApplicationContext repositoryContext) : base(repositoryContext)
    {
        
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
    {
        var specification = new BookSpecification(bookParameters);
        return await GetBySpecificationAsync(specification, trackChanges);
    }

    public async Task<Book> GetBookAsync(int bookId, bool trackChanges)
    {
        var book = await FindByCondition(c => c.Id.Equals(bookId), trackChanges);
        return book.SingleOrDefault();
    }

    public async Task<int> CountBooksAsync(BookParameters bookParameters)
    {
        var specification = new BookSpecification(bookParameters) { PageNumber = 1, PageSize = int.MaxValue };
        var books = await GetBySpecificationAsync(specification, trackChanges: false);
        return books.Count();
    }

    public async Task<Book> GetBookByIsbnAsync(string isbn)
    {
        var book = await FindByCondition(b => b.ISBN == isbn, trackChanges: false);
        return book.SingleOrDefault();
    }
}