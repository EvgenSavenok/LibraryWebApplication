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
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.BorrowingUseCases;

public class ReturnBookUseCase : IReturnBookUseCase
{
    private readonly IGetBookByIdUseCase _getBookByIdUseCase;
    private readonly IGetUserBorrowUseCase _getUserBorrowUseCase;
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IUpdateBookUseCase _updateBookUseCase;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ReturnBookUseCase(IGetBookByIdUseCase getBookByIdUseCase,
        IGetUserBorrowUseCase getUserBorrowUseCase,
        IRepositoryManager repository,
        IMapper mapper,
        IUpdateBookUseCase updateBookUseCase,
        IHttpContextAccessor httpContextAccessor)
    {
        _getBookByIdUseCase = getBookByIdUseCase;
        _getUserBorrowUseCase = getUserBorrowUseCase;
        _repository = repository;
        _mapper = mapper;
        _updateBookUseCase = updateBookUseCase;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task ExecuteAsync(int bookId)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedException("Cannot get userId");
        var bookDto = await _getBookByIdUseCase.ExecuteAsync(bookId);
        if (bookDto == null)
        {
            throw new NotFoundException($"Cannot find book with id {bookId}");
        }
        
        var borrowDto = await _getUserBorrowUseCase.ExecuteAsync(bookId);
        if (borrowDto == null)
        {
            throw new NotFoundException($"Cannot find user borrow with id {bookId}");
        }
        
        var bookForUpdateEntity = _mapper.Map<BookForUpdateDto>(bookDto);
        bookForUpdateEntity.Amount = ++bookForUpdateEntity.Amount;
        await _updateBookUseCase.ExecuteAsync(bookId, bookForUpdateEntity);
        
        var userBookBorrow = _mapper.Map<UserBookBorrow>(borrowDto);
        _repository.Borrow.Delete(userBookBorrow);

        await _repository.SaveAsync();
    }
}
