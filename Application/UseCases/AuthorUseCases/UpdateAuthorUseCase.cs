using Application.Contracts;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;

namespace Application.UseCases.AuthorUseCases;

public class UpdateAuthorUseCase : IUpdateAuthorUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public UpdateAuthorUseCase(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task ExecuteAsync(int id, AuthorForUpdateDto author)
    {
        var authorEntity = await _repository.Author.GetAuthorAsync(id, trackChanges: true);
        _mapper.Map(author, authorEntity);
        await _repository.SaveAsync();
    }
}
