using System.Linq.Expressions;

namespace Application.Contracts.RepositoryContracts;

public interface IRepositoryBase<T>
{
    Task<IEnumerable<T>> FindAll(bool trackChanges, CancellationToken cancellationToken);
    Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges, CancellationToken cancellationToken);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification, bool trackChanges,
        CancellationToken cancellationToken);
}
