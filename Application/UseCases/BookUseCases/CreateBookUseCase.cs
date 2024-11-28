using Application.Contracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation;
using AutoMapper;
using Domain.Entities.Models;
using FluentValidation;

namespace Application.UseCases.BookUseCases;

public class CreateBookUseCase : ICreateBookUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<Book> _validator;
    private readonly ILoggerManager _logger;

    public CreateBookUseCase(IRepositoryManager repository, 
        IMapper mapper,
        IValidator<Book> validator,
        ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }

    public async Task ExecuteAsync(BookForCreationDto bookDto)
    {
        var bookEntity = _mapper.Map<Book>(bookDto);
        var validationResult = await _validator.ValidateAsync(bookEntity);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var existingBook = await _repository.Book.GetBookByIsbnAsync(bookEntity.ISBN);
        if (existingBook != null)
        {
            _logger.LogInfo($"Book with id: {bookEntity.Id} already exists in the database.");
            throw new ConflictException("A book with this ISBN already exists.");
        }
        _repository.Book.Create(bookEntity);
        await _repository.SaveAsync();
    }
}
