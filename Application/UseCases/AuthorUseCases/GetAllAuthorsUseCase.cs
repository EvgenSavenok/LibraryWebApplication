using Application.Contracts;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.RequestFeatures;

namespace Application.UseCases.AuthorUseCases;

public class GetAllAuthorsUseCase : IGetAllAuthorsUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetAllAuthorsUseCase(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<IEnumerable<AuthorDto>> ExecuteAsync(AuthorParameters requestParameters)
    {
        var authors = await _repository.Author.GetAllAuthorsAsync(requestParameters, trackChanges: false);
        return _mapper.Map<IEnumerable<AuthorDto>>(authors);
    }
}
