using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation;
using AutoMapper;

namespace Application.UseCases.AuthorUseCases;

public class GetAuthorByIdUseCase : IGetAuthorByIdUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public GetAuthorByIdUseCase(IRepositoryManager repository, 
        IMapper mapper,
        ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<AuthorDto> ExecuteAsync(int id)
    {
        
        var author = await _repository.Author.GetAuthorAsync(id, trackChanges: false);
        if (author == null)
        {
            throw new NotFoundException($"Author with id {id} not found.");
        }
        return _mapper.Map<AuthorDto>(author);
    }
}
