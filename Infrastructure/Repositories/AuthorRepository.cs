﻿using Application.Contracts.RepositoryContracts;
using Application.Specifications;
using Domain.Entities.RequestFeatures;
using Domain.Models;
using Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Repository;

namespace Infrastructure.Repositories;

public class AuthorRepository : RepositoryBase<Author>, IAuthorRepository
{
    public AuthorRepository(ApplicationContext repositoryContext)
        : base(repositoryContext)
    {
    }

    public async Task<int> CountAuthorsAsync(AuthorParameters authorParameters, CancellationToken cancellationToken)
    {
        var specification = new AuthorSpecification(authorParameters) { PageNumber = 1, PageSize = int.MaxValue };
        var authors = await GetBySpecificationAsync(specification, trackChanges: false, cancellationToken);
        return authors.Count();
    }

    public async Task<IEnumerable<Author>> GetAllAuthorsAsync(AuthorParameters authorParameters, bool trackChanges,
        CancellationToken cancellationToken)
    {
        var specification = new AuthorSpecification(authorParameters);
        return await GetBySpecificationAsync(specification, trackChanges, cancellationToken);
    }

    public async Task<Author> GetAuthorAsync(int authorId, bool trackChanges, CancellationToken cancellationToken)
    {
        var authors = await FindByCondition(c => c.Id.Equals(authorId), trackChanges, cancellationToken);
        return authors.SingleOrDefault();
    }
}
