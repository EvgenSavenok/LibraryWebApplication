using Application.Contracts;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using AutoMapper;

namespace Application.UseCases.AuthorUseCases;

public class DeleteAuthorUseCase : IDeleteAuthorUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    
    public DeleteAuthorUseCase(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task ExecuteAsync(int id)
    {
        var author = await _repository.Author.GetAuthorAsync(id, trackChanges: false);
        _repository.Author.Delete(author);
        await _repository.SaveAsync();
    }
}
