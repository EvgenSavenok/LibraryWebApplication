using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Specifications;

public class BookSpecification : BaseSpecification<Book>
{
    public BookSpecification(BookParameters bookParameters)
    {
        ApplyCriteria(book =>
            (bookParameters.Genre == 0 || book.Genre == bookParameters.Genre) &&
            (bookParameters.AuthorId == 0 || book.Author.Id == bookParameters.AuthorId));

        ApplyOrderBy(books => books.OrderBy(book => book.BookTitle));
        Includes = q => q.Include(b => b.Author);

        PageSize = bookParameters.PageSize;
        PageNumber = bookParameters.PageNumber;
    }
}

