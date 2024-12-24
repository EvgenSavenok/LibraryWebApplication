using Application.RequestFeatures;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Application.Specifications;

public class BookSpecification : BaseSpecification<Book>
{
    public BookSpecification(BookParameters bookParameters)
    {
        ApplyCriteria(book =>
            (bookParameters.Genre == 0 || book.Genre == bookParameters.Genre) &&
            (bookParameters.AuthorId == 0 || book.Author.Id == bookParameters.AuthorId)
            && (string.IsNullOrWhiteSpace(bookParameters.SearchTerm) ||
                book.BookTitle.ToLower().Contains(bookParameters.SearchTerm.ToLower())));

        ApplyOrderBy(books => books.OrderBy(book => book.BookTitle));
        Includes = q => q.Include(b => b.Author);

        PageSize = bookParameters.PageSize;
        PageNumber = bookParameters.PageNumber;
    }
}

