using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.RequestFeatures;

namespace Application.UseCases.AuthorUseCases;

public class GetAllAuthorsUseCase : IGetAllAuthorsUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public GetAllAuthorsUseCase(IRepositoryManager repository, 
        IMapper mapper,
        ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }
    public async Task<PagedResult<AuthorDto>> ExecuteAsync(AuthorParameters authorParameters)
    {
        var authors = await _repository.Author.GetAllAuthorsAsync(authorParameters, trackChanges: false);
        if (authors == null || !authors.Any())
        {
            return new PagedResult<AuthorDto>
            {
                Items = Enumerable.Empty<AuthorDto>(),
                TotalCount = 0,
                TotalPages = 0,
                CurrentPage = authorParameters.PageNumber
            };
        }
        
        var totalAuthors = await _repository.Author.CountAuthorsAsync(authorParameters);
        var totalPages = (int)Math.Ceiling((double)totalAuthors / authorParameters.PageSize);
        
        return new PagedResult<AuthorDto>
        {
            Items = _mapper.Map<IEnumerable<AuthorDto>>(authors),
            TotalCount = totalAuthors,
            TotalPages = totalPages,
            CurrentPage = authorParameters.PageNumber
        };
    }
}
