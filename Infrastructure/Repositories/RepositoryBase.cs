using System.Linq.Expressions;
using Application.Contracts;
using Application.Contracts.RepositoryContracts;
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
    public async Task<IEnumerable<T>> FindAll(bool trackChanges, CancellationToken cancellationToken) =>
        await (!trackChanges ?
            RepositoryContext.Set<T>()
                .AsNoTracking() :
            RepositoryContext.Set<T>()).ToListAsync(cancellationToken);
    
    public async Task<IEnumerable<T>> FindByCondition(Expression<Func<T, bool>> expression,
        bool trackChanges, CancellationToken cancellationToken) =>
        await (!trackChanges ?
            RepositoryContext.Set<T>()
                .Where(expression)
                .AsNoTracking() :
            RepositoryContext.Set<T>()
                .Where(expression)).ToListAsync(cancellationToken);
    
    public void Create(T entity) => RepositoryContext.Set<T>().Add(entity);
    public void Update(T entity) => RepositoryContext.Set<T>().Update(entity);
    public void Delete(T entity) => RepositoryContext.Set<T>().Remove(entity);
    public async Task<IEnumerable<T>> GetBySpecificationAsync(ISpecification<T> specification, bool trackChanges,
        CancellationToken cancellationToken)
    {
        IQueryable<T> query = RepositoryContext.Set<T>();

        if (specification.Criteria != null)
            query = query.Where(specification.Criteria);
        
        if (!trackChanges)
            query = query.AsNoTracking();

        if (specification.OrderBy != null)
            query = specification.OrderBy(query);

        if (specification.Includes != null)
            query = specification.Includes(query);

        query = query
            .Skip((specification.PageNumber - 1) * specification.PageSize)
            .Take(specification.PageSize);

        return await query.ToListAsync(cancellationToken);
    }
}