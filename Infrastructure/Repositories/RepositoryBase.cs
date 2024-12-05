using System.Linq.Expressions;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Infrastructure.Repositories;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected ApplicationContext RepositoryContext;
    public RepositoryBase(ApplicationContext repositoryContext)
    {
        RepositoryContext = repositoryContext;
    }
    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges ?
            RepositoryContext.Set<T>()
                .AsNoTracking() :
            RepositoryContext.Set<T>();
    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges) =>
        !trackChanges ?
            RepositoryContext.Set<T>()
                .Where(expression)
                .AsNoTracking() :
            RepositoryContext.Set<T>()
                .Where(expression);
    public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
    public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
    public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
    public async Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification, bool trackChanges)
    {
        IQueryable<T> query = RepositoryContext.Set<T>();

        if (specification.Criteria != null)
            query = query.Where(specification.Criteria);

        if (specification.OrderBy != null)
            query = specification.OrderBy(query);

        if (specification.Includes != null)
            query = specification.Includes(query);

        query = query
            .Skip((specification.PageNumber - 1) * specification.PageSize)
            .Take(specification.PageSize);

        return await query.ToListAsync();
    }
}