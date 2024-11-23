using Domain.Entities.Models;

namespace Repository.Extensions;

public static class RepositoryAuthorExtensions
{
    public static IQueryable<Author> Search(this IQueryable<Author> authors, string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm) || searchTerm.Equals(null))
            return authors;
        var lowerCaseTerm = searchTerm.Trim().ToLower();
        return authors.Where(b => b.LastName.ToLower().Contains(lowerCaseTerm));
    }
}
