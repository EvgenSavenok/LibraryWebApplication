using System.Security.Claims;
using Application.Contracts;
using Application.Contracts.ServicesContracts;
using Application.DataTransferObjects;
using Domain.Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/booking")]
[ApiController]
public class BookingController : Controller
{
    private readonly IBookingService _borrowService;
    private readonly IBookService _bookService;
    private readonly IAuthorService _authorService;

    public BookingController(IBookingService borrowService, IBookService bookService,
        IAuthorService authorService)
    {
        _borrowService = borrowService;
        _bookService = bookService;
        _authorService = authorService;
    }

    [HttpGet("user")]
    public IActionResult BooksPageUser()
    {
        return View("~/Views/Booking/AllBooksPage.cshtml");
    }

    [HttpGet("bookInfo/{id}")]
    public async Task<IActionResult> BookInfo(int id)
    {
        var bookInfo = await _bookService.GetBookByIdAsync(id);
        var authorInfo = await _authorService.GetAuthorByIdAsync(bookInfo.AuthorId);
        
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

        var pagedResult = await _borrowService.GetAllUserBookBorrowsAsync(requestParameters, userId);

        if (pagedResult == null)
        {
            return View("~/Views/Booking/NoReservedBooksPage.cshtml"); 
        }
        
        return Ok(new
        {
            reservedBooks = pagedResult.Items,
            currentPage = pagedResult.CurrentPage,
            totalPages = pagedResult.TotalPages,
            totalBooks = pagedResult.TotalCount
        });
    }

    [HttpPost("take/{id}"), Authorize(Policy = "User")]
    public async Task<IActionResult> TakeBook(int id)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var book = await _bookService.GetBookByIdAsync(id);
        var bookDto = new BookForUpdateDto
        {
            Amount = --book.Amount,
            ISBN = book.ISBN,
            BookTitle = book.BookTitle,
            Genre = book.Genre,
            Description = book.Description,
            AuthorId = book.AuthorId,
        };
        var userBookBorrow = new UserBookBorrowDto
        {
            UserId = userId,                
            BookId = book.Id,               
            BorrowDate = DateTime.UtcNow,      
            ReturnDate = DateTime.UtcNow.AddDays(30)
        };
        await _borrowService.CreateUserBookBorrowAsync(userBookBorrow);
        await _bookService.UpdateBookAsync(id, bookDto);
        return Ok();
    }
}
