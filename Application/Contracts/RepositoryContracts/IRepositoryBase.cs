using System.Linq.Expressions;

namespace Application.Contracts.RepositoryContracts;

public interface IRepositoryBase<T>
{
    Task<IEnumerable<T>> FindAll(bool trackChanges);
    Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges);
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
    Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification, bool trackChanges);
}
