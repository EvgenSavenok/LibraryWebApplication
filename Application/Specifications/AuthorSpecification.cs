using Domain.Entities.RequestFeatures;
using Domain.Models;

namespace Application.Specifications
{
    public class AuthorSpecification : BaseSpecification<Author>
    {
        public AuthorSpecification(AuthorParameters authorParameters)
        {
            if (!string.IsNullOrEmpty(authorParameters.SearchTerm))
            {
                ApplyCriteria(a => a.LastName.Contains(authorParameters.SearchTerm) || a.Name.Contains(authorParameters.SearchTerm));
            }
            
            ApplyOrderBy(a => a.OrderBy(author => author.LastName));
            
            PageSize = authorParameters.PageSize;
            PageNumber = authorParameters.PageNumber;
        }
    }
}
