﻿using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.RequestFeatures;
using Domain.Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/booking")]
[ApiController]
public class BookingController : Controller
{
    private readonly IGetUsersBorowsUseCase _getUsersBorowsUseCase;
    private readonly IBookInfoControllerUseCase _bookInfoControllerUseCase;
    private readonly ITakeBookControllerUseCase _takeBookControllerUseCase;
    private readonly IReturnBookUseCase _returnBookUseCase;
    
    public BookingController(IGetUsersBorowsUseCase getUsersBorowsUseCase,
        IBookInfoControllerUseCase bookInfoControllerUseCase,
        ITakeBookControllerUseCase takeBookControllerUseCase,
        IReturnBookUseCase returnBookUseCase)
    {
        _getUsersBorowsUseCase = getUsersBorowsUseCase;
        _bookInfoControllerUseCase = bookInfoControllerUseCase;
        _takeBookControllerUseCase = takeBookControllerUseCase;
        _returnBookUseCase = returnBookUseCase;
    }

    [HttpGet("user")]
    public IActionResult BooksPageUser()
    {
        return View("~/Views/Booking/AllBooksPage.cshtml");
    }

    [HttpGet("bookInfo/{bookId}")]
    public async Task<IActionResult> BookInfo(int bookId, CancellationToken cancellationToken)
    {
        var pageDataDto = await _bookInfoControllerUseCase.GetBookInfo(bookId, cancellationToken);
        return View("~/Views/Booking/InfoAboutBook.cshtml", pageDataDto);
    }

    [HttpGet("user/reservedBooksPage")]
    public IActionResult GetUserReservedBooks()
    {
        return View("~/Views/Booking/ReservedBooksPage.cshtml");
    }

    [HttpGet("user/reservedBooks")]
    public async Task<IActionResult> DisplayUserReservedBooks([FromQuery] BorrowParameters requestParameters, 
        CancellationToken cancellationToken)
    {
        var result = await _getUsersBorowsUseCase.ExecuteAsync(requestParameters, cancellationToken);
        return result; 
    }

    [HttpPost("take/{bookId}"), Authorize(Policy = "User")]
    public async Task<IActionResult> TakeBook(int bookId, CancellationToken cancellationToken)
    {
        await _takeBookControllerUseCase.ExecuteAsync(bookId, cancellationToken);
        return Ok();
    }
    
    [HttpDelete("delete/{bookId}"), Authorize(Policy = "User")]
    public async Task<IActionResult> ReturnBook(int bookId, CancellationToken cancellationToken)
    {
        await _returnBookUseCase.ExecuteAsync(bookId, cancellationToken);
        return Ok();
    }
}
