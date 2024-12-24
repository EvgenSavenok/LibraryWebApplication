using Domain.Entities.RequestFeatures;
using Domain.Models;

namespace Application.Contracts.RepositoryContracts;

public interface IAuthorRepository : IRepositoryBase<Author>
{
    Task<IEnumerable<Author>> GetAllAuthorsAsync(AuthorParameters authorParameters, bool trackChanges);
    Task<Author> GetAuthorAsync(int bookId, bool trackChanges);
    Task<int> CountAuthorsAsync(AuthorParameters authorParameters);
}
