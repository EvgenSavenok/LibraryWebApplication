using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Specifications;

public class BookSpecification : BaseSpecification<Book>
{
    public BookSpecification(BookParameters parameters)
    {
        ApplyCriteria(book =>
            (parameters.Genre == 0 || book.Genre == parameters.Genre) &&
            (parameters.AuthorId == 0 || book.Author.Id == parameters.AuthorId));

        ApplyOrderBy(books => books.OrderBy(book => book.BookTitle));
        Includes = q => q.Include(b => b.Author);

        PageSize = parameters.PageSize;
        PageNumber = parameters.PageNumber;
    }
}

