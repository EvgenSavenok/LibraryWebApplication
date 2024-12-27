using Domain.Entities.RequestFeatures;
using Domain.Models;

namespace Application.Contracts.RepositoryContracts;

public interface IAuthorRepository : IRepositoryBase<Author>
{
    Task<IEnumerable<Author>> GetAllAuthorsAsync(AuthorParameters authorParameters, bool trackChanges,
        CancellationToken cancellationToken);
    Task<Author> GetAuthorAsync(int bookId, bool trackChanges, CancellationToken cancellationToken);
    Task<int> CountAuthorsAsync(AuthorParameters authorParameters, CancellationToken cancellationToken);
}
