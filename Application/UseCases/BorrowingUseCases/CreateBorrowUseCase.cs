using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation.CustomExceptions;
using AutoMapper;
using Domain.Models;
using FluentValidation;

namespace Application.UseCases.BorrowingUseCases;

public class CreateBorrowUseCase : ICreateBorrowUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<UserBookBorrow> _validator;
    private readonly IUpdateBookUseCase _updateBookUseCase;
    private readonly IGetUserBorrowUseCase _getUserBorrowUseCase;

    public CreateBorrowUseCase(IRepositoryManager repository,
        IMapper mapper,
        IValidator<UserBookBorrow> validator,
        IUpdateBookUseCase updateBookUseCase,
        IGetUserBorrowUseCase getUserBorrowUseCase)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
        _updateBookUseCase = updateBookUseCase;
        _getUserBorrowUseCase = getUserBorrowUseCase;   
    }

    public async Task ExecuteAsync(string userId, int bookId, BookDto bookDto)
    {
        var userBookBorrow = new UserBookBorrowDto
        {
            UserId = userId,                
            BookId = bookId,               
            BorrowDate = DateTime.UtcNow,      
            ReturnDate = DateTime.UtcNow.AddDays(30)
        };
        var bookForUpdateEntity = _mapper.Map<BookForUpdateDto>(bookDto);
        bookForUpdateEntity.Amount = --bookForUpdateEntity.Amount;
        await _updateBookUseCase.ExecuteAsync(bookId, bookForUpdateEntity);
        var borrowEntity = _mapper.Map<UserBookBorrow>(userBookBorrow);
        var validationResult = await _validator.ValidateAsync(borrowEntity);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        _repository.Borrow.Create(borrowEntity);
        await _repository.SaveAsync();
        if (_getUserBorrowUseCase.ExecuteAsync(bookId) == null)
        {
            throw new BadRequestException($"Cannot reserve book with id {bookId}");
        }
    }
}
