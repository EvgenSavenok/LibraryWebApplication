using System.Security.Claims;
using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using Application.RequestFeatures;
using AutoMapper;
using Domain.Entities.RequestFeatures;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> ExecuteAsync(BorrowParameters borrowParameters, CancellationToken cancellationToken)
    {
        string pathToController = "~/Views/Booking/NoReservedBooksPage.cshtml";
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId))
        {
            return new NotFoundResult(); 
        }

        var borrowsAsync = await _repository.Borrow.GetAllUserBookBorrowsAsync(borrowParameters, userId,
            trackChanges: false, cancellationToken);
        if (borrowsAsync == null || !borrowsAsync.Any())
        {
            return new ViewResult
            {
                ViewName = pathToController
            };
        }

        var totalBooks = await _repository.Borrow.CountBorrowsAsync(borrowParameters, cancellationToken);
        var totalPages = (int)Math.Ceiling((double)totalBooks / borrowParameters.PageSize);

        var pagedResult = new PagedResult<UserBookBorrow>
        {
            Items = borrowsAsync,
            TotalCount = totalBooks,
            TotalPages = totalPages,
            CurrentPage = borrowParameters.PageNumber
        };

        return new OkObjectResult(pagedResult);
    }

}
