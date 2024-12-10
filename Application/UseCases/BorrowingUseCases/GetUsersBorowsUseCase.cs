using System.Security.Claims;
using Application.Contracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.BorrowingUseCases;

public class GetUsersBorowsUseCase : IGetUsersBorowsUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public GetUsersBorowsUseCase(IRepositoryManager repository,
        IHttpContextAccessor httpContextAccessor)
    {
        _repository = repository;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<PagedResult<UserBookBorrow>> ExecuteAsync(BorrowParameters borrowParameters)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var borrowsAsync = await _repository.Borrow.GetAllUserBookBorrowsAsync(borrowParameters, userId, trackChanges: false);
        if (borrowsAsync == null || !borrowsAsync.Any())
        {
            return new PagedResult<UserBookBorrow>
            {
                Items = Enumerable.Empty<UserBookBorrow>(),
                TotalCount = 0,
                TotalPages = 0,
                CurrentPage = borrowParameters.PageNumber
            };
        }
        var totalBooks = await _repository.Borrow.CountBorrowsAsync(borrowParameters);
        var totalPages = (int)Math.Ceiling((double)totalBooks / borrowParameters.PageSize);
        
        return new PagedResult<UserBookBorrow>
        {
            Items = borrowsAsync,
            TotalCount = totalBooks,
            TotalPages = totalPages,
            CurrentPage = borrowParameters.PageNumber
        };
    }
}
