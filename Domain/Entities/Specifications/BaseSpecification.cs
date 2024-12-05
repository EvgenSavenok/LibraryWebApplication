using System.Linq.Expressions;
using Domain.Contracts;

namespace Domain.Entities.Specifications;

public abstract class BaseSpecification<T> : ISpecification<T>
{
    private Expression<Func<T, bool>> _criteria = _ => true;
    private Func<IQueryable<T>, IOrderedQueryable<T>>? _orderBy;
    private Func<IQueryable<T>, IQueryable<T>>? _includes;

    public virtual Expression<Func<T, bool>> Criteria
    {
        get => _criteria;
        set => _criteria = value;
    }
    public virtual Func<IQueryable<T>, IOrderedQueryable<T>>? OrderBy
    {
        get => _orderBy;
        set => _orderBy = value;
    }

    public virtual Func<IQueryable<T>, IQueryable<T>>? Includes
    {
        get => _includes;
        set => _includes = value;
    }
    public int PageSize { get; set; } = 10;
    public int PageNumber { get; set; } = 1;
    
    public void ApplyOrderBy(Func<IQueryable<T>, IOrderedQueryable<T>> orderByExpression)
    {
        OrderBy = orderByExpression;
    }

    public void ApplyCriteria(Expression<Func<T, bool>> criteriaExpression)
    {
        Criteria = criteriaExpression;
    }
}

