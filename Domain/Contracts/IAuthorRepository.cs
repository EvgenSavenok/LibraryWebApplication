using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Domain.Contracts;

public interface IAuthorRepository : IRepositoryBase<Author>
{
    Task<IEnumerable<Author>> GetAllAuthorsAsync(AuthorParameters authorParameters, bool trackChanges);
    Task<Author> GetAuthorAsync(int bookId, bool trackChanges);
    Task<int> CountAuthorsAsync(AuthorParameters authorParameters);
}
