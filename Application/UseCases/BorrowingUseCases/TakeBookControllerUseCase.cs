using System.Security.Claims;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation;
using Application.Validation.CustomExceptions;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.BorrowingUseCases;

public class TakeBookControllerUseCase : ITakeBookControllerUseCase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IRepositoryManager _repository;
    private readonly IValidator<Book> _bookValidator;
    private readonly IValidator<UserBookBorrow> _borrowValidator;
    private readonly IMapper _mapper;
    public TakeBookControllerUseCase(IHttpContextAccessor httpContextAccessor,
        IRepositoryManager repository,
        IMapper mapper,
        IValidator<UserBookBorrow> borrowValidator,
        IValidator<Book> bookValidator)
    {
        _httpContextAccessor = httpContextAccessor;
        _repository = repository;
        _mapper = mapper;
        _borrowValidator = borrowValidator;
        _bookValidator = bookValidator;
    }

    public async Task ExecuteAsync(int bookId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedException("Cannot get userId");

        var bookEntity = await _repository.Book.GetBookAsync(bookId, trackChanges: true, cancellationToken);
        if (bookEntity == null)
        {
            throw new NotFoundException($"Book with id {bookId} not found.");
        }
        
        if (bookEntity.Amount <= 0)
            throw new ValidationException("Book is out of correct amount.");
        bookEntity.Amount--;
        
        string oldIsbn = bookEntity.ISBN;
        _mapper.Map(bookEntity, bookEntity);
        bookEntity.Author = await _repository.Author.GetAuthorAsync(bookId, trackChanges: true, cancellationToken);
        var bookValidationResult = await _bookValidator.ValidateAsync(bookEntity, cancellationToken);
        if (!bookValidationResult.IsValid)
        {
            throw new ValidationException(bookValidationResult.Errors);
        }

        if (oldIsbn != bookEntity.ISBN)
        {
            var existingBook = await _repository.Book.GetBookByIsbnAsync(bookEntity.ISBN, cancellationToken);
            if (existingBook != null)
            {
                throw new AlreadyExistsException("A book with this ISBN already exists.");
            }
        }
        
        var userBookBorrow = new UserBookBorrowDto
        {
            UserId = userId,                
            BookId = bookId,               
            BorrowDate = DateTime.UtcNow,      
            ReturnDate = DateTime.UtcNow.AddDays(30)
        };
        var borrowEntity = _mapper.Map<UserBookBorrow>(userBookBorrow);
        var borrowValidationResult = await _borrowValidator.ValidateAsync(borrowEntity, cancellationToken);
        if (!borrowValidationResult.IsValid)
        {
            throw new ValidationException(borrowValidationResult.Errors);
        }
        _repository.Borrow.Create(borrowEntity);
        await _repository.SaveAsync();
    }
}
