using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Specifications;

public class BorrowSpecification : BaseSpecification<UserBookBorrow>
{
    public BorrowSpecification(BorrowParameters borrowParameters, string? userId)
    {
        if (!string.IsNullOrEmpty(userId))
            ApplyCriteria(b => b.UserId == userId);

        ApplyOrderBy(b => b.OrderBy(borrow => borrow.Id));

        Includes = q => q.Include(b => b.Book)
            .ThenInclude(book => book.Author);;
    }
}
