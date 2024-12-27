using Application.RequestFeatures;
using Domain.Models;

namespace Application.Contracts.RepositoryContracts;

public interface IBookRepository : IRepositoryBase<Book>
{
    Task<IEnumerable<Book>> GetAllBooksAsync(BookParameters bookParameters, CancellationToken cancellationToken, 
        bool trackChanges);
    Task<Book> GetBookAsync(int bookId, bool trackChanges, CancellationToken cancellationToken);
    Task<int> CountBooksAsync(BookParameters requestParameters, CancellationToken cancellationToken);
    public Task<Book> GetBookByIsbnAsync(string isbn, CancellationToken cancellationToken);
}
