using System.Security.Claims;
using Application.Contracts;
using Application.Contracts.ServicesContracts;
using Application.DataTransferObjects;
using Domain.Entities.RequestFeatures;
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

        var reservedBooks = await _borrowService.GetAllUserBookBorrowsAsync(requestParameters, userId);
        var totalBooks = await _borrowService.CountBorrowsAsync(requestParameters); 
        var totalPages = (int)Math.Ceiling((double)totalBooks / requestParameters.PageSize);

        if (reservedBooks == null || !reservedBooks.Any())
        {
            return View("~/Views/Booking/NoReservedBooksPage.cshtml"); 
        }
        
        var response = new
        {
            reservedBooks,
            currentPage = requestParameters.PageNumber,
            totalPages
        };
        return Ok(response);
    }

    [HttpPost("take/{id}")]
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
