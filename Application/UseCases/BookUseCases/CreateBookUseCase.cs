using Application.Contracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.Models;

namespace Application.UseCases.BookUseCases;

public class CreateBookUseCase : ICreateBookUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public CreateBookUseCase(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task ExecuteAsync(BookForCreationDto bookDto)
    {
        var bookEntity = _mapper.Map<Book>(bookDto);
        _repository.Book.Create(bookEntity);
        await _repository.SaveAsync();
    }
}
