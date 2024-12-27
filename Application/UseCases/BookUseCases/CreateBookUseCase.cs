using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation;
using Application.Validation.CustomExceptions;
using AutoMapper;
using Domain.Models;
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

    public async Task ExecuteAsync(BookForCreationDto bookDto, CancellationToken cancellationToken)
    {
        var bookEntity = _mapper.Map<Book>(bookDto);
        var validationResult = await _validator.ValidateAsync(bookEntity);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        var existingBook = await _repository.Book.GetBookByIsbnAsync(bookEntity.ISBN, cancellationToken);
        if (existingBook != null)
        {
            throw new AlreadyExistsException("A book with this ISBN already exists.");
        }
        _repository.Book.Create(bookEntity);
        await _repository.SaveAsync();
    }
}
