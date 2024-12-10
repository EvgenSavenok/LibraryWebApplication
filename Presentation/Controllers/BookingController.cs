using System.Security.Claims;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using Domain.Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/booking")]
[ApiController]
public class BookingController : Controller
{
    private readonly IGetBookByIdUseCase _getBookByIdUseCase;
    private readonly IUpdateBookUseCase _updateBookUseCase;
    private readonly ICreateBorrowUseCase _createBorrowUseCase;
    private readonly IGetUsersBorowsUseCase _getUsersBorowsUseCase;
    private readonly IGetAuthorByIdUseCase _getAuthorByIdUseCase;
    
    public BookingController(IGetBookByIdUseCase getBookByIdUseCase,
        IUpdateBookUseCase updateBookUseCase,
        ICreateBorrowUseCase createBorrowUseCase,
        IGetUsersBorowsUseCase getUsersBorowsUseCase,
        IGetAuthorByIdUseCase getAuthorByIdUseCase)
    {
        _getBookByIdUseCase = getBookByIdUseCase;
        _updateBookUseCase = updateBookUseCase;
        _createBorrowUseCase = createBorrowUseCase;
        _getUsersBorowsUseCase = getUsersBorowsUseCase;
        _getAuthorByIdUseCase = getAuthorByIdUseCase;
    }

    [HttpGet("user")]
    public IActionResult BooksPageUser()
    {
        return View("~/Views/Booking/AllBooksPage.cshtml");
    }

    [HttpGet("bookInfo/{id}")]
    public async Task<IActionResult> BookInfo(int id)
    {
        var bookInfo = await _getBookByIdUseCase.ExecuteAsync(id);
        var authorInfo = await _getAuthorByIdUseCase.ExecuteAsync(id);
        
        var bookInfoViewModel = new
        {
             Book = bookInfo, 
             Author = authorInfo 
        };
        
        return View("~/Views/Booking/InfoAboutBook.cshtml", bookInfoViewModel);
    }

    [HttpGet("user/reservedBooksPage")]
    public IActionResult GetUserReservedBooks()
    {
        return View("~/Views/Booking/ReservedBooksPage.cshtml");
    }

    [HttpGet("user/reservedBooks")]
    public async Task<IActionResult> DisplayUserReservedBooks([FromQuery] BorrowParameters requestParameters)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var pagedResult = await _getUsersBorowsUseCase.ExecuteAsync(requestParameters, userId);

        if (pagedResult == null)
        {
            return View("~/Views/Booking/NoReservedBooksPage.cshtml"); 
        }
        
        return Ok(pagedResult);
    }

    [HttpPost("take/{bookId}"), Authorize(Policy = "User")]
    public async Task<IActionResult> TakeBook(int bookId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var book = await _getBookByIdUseCase.ExecuteAsync(bookId);
        await _createBorrowUseCase.ExecuteAsync(userId, bookId, book);
        return Ok();
    }
}
