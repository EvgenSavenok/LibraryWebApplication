using Application.Contracts;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;

namespace Application.UseCases.AuthorUseCases;

public class GetAuthorByIdUseCase : IGetAuthorByIdUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetAuthorByIdUseCase(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<AuthorDto> ExecuteAsync(int id)
    {
        var author = await _repository.Author.GetAuthorAsync(id, trackChanges: false);
        return _mapper.Map<AuthorDto>(author);
    }
}
