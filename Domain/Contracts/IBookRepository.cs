using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Domain.Contracts;

public interface IBookRepository : IRepositoryBase<Book>
{
    Task<IEnumerable<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges);
    Task<Book> GetBookAsync(int bookId, bool trackChanges);
    Task<int> CountBooksAsync(BookParameters requestParameters);
    public Task<Book> GetBookByIsbnAsync(string isbn);
}
