using System;
using System.Linq.Expressions;
using Domain.Entities;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Domain.Entities.Specifications
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
