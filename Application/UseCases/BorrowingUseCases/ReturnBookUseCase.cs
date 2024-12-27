using System.Security.Claims;
using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation;
using Application.Validation.CustomExceptions;
using AutoMapper;
using Domain.Models;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.BorrowingUseCases;

public class ReturnBookUseCase : IReturnBookUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IValidator<Book> _bookValidator;
    public ReturnBookUseCase(IValidator<Book> bookValidator,
        IRepositoryManager repository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _bookValidator = bookValidator;
    }
    public async Task ExecuteAsync(int bookId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedException("Cannot get userId");

        var bookEntity = await _repository.Book.GetBookAsync(bookId, trackChanges: true, cancellationToken);
        if (bookEntity == null)
            throw new NotFoundException($"Book with id {bookId} not found.");
        bookEntity.Author = await _repository.Author.GetAuthorAsync(bookId, trackChanges: true, cancellationToken);
        
        var borrow = await _repository.Borrow.GetUserBookBorrowAsync(bookId, trackChanges: false, cancellationToken);
        if (borrow == null)
            throw new BadRequestException("Cannot get borrow.");
        
        bookEntity.Amount++;

        string oldIsbn = bookEntity.ISBN;
        var validationResult = await _bookValidator.ValidateAsync(bookEntity, cancellationToken);
        if (!validationResult.IsValid)
            throw new ValidationException(validationResult.Errors);

        if (oldIsbn != bookEntity.ISBN)
        {
            var existingBook = await _repository.Book.GetBookByIsbnAsync(bookEntity.ISBN, cancellationToken);
            if (existingBook != null)
                throw new AlreadyExistsException("A book with this ISBN already exists.");
        }

        _repository.Borrow.Delete(borrow);
        await _repository.SaveAsync();
    }
}
