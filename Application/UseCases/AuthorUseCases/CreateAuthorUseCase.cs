using Application.Contracts;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.Models;

namespace Application.UseCases.AuthorUseCases;

public class CreateAuthorUseCase : ICreateAuthorUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public CreateAuthorUseCase(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task ExecuteAsync(AuthorForCreationDto author)
    {
        var authorEntity = _mapper.Map<Author>(author);
        _repository.Author.Create(authorEntity);
        await _repository.SaveAsync();
    }
}
