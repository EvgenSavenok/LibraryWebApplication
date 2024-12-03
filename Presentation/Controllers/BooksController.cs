using Application.Contracts.ServicesContracts;
using Application.DataTransferObjects;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Presentation.Controllers;

[Route("api/books")]
[ApiController]
public class BooksController : Controller
{
    private readonly IBookService _bookService;
    private readonly IAuthorService _authorService;

    public BooksController(IBookService bookService, IAuthorService authorService)
    {
        _bookService = bookService;
        _authorService = authorService;
    }
    
    [HttpGet("admin")]
    public IActionResult BooksPageAdmin()
    {
        return View("~/Views/Books/AllBooksPage.cshtml");
    }

    [HttpGet("GetBooks"), Authorize]
    public async Task<IActionResult> GetBooks([FromQuery] BookParameters requestParameters)
    {
        var pagedResult = await _bookService.GetBooksAsync(requestParameters);
        return Ok(new
        {
            books = pagedResult.Items,
            currentPage = pagedResult.CurrentPage,
            totalPages = pagedResult.TotalPages,
            totalBooks = pagedResult.TotalCount
        });
    }

    [HttpGet("{id}", Name = "BookById"), Authorize]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        return Ok(book);
    }
    
    [HttpGet("AddBook")]
    public async Task<IActionResult> CreateBook()
    {
        var genres = Enum.GetValues(typeof(BookGenre)).Cast<BookGenre>()
            .Select(g => new SelectListItem
            {
                Text = g.ToString(),
                Value = g.ToString(),
                Selected = g == BookGenre.Adventures 
            }).ToList();
        
        var authorsResult = await _authorService.GetAllAuthorsAsync(new AuthorParameters() { PageSize = 10}); 
        ViewBag.Genres = genres; 
        ViewBag.Authors = authorsResult.Items; 
        return View("~/Views/Books/AddBookPage.cshtml");
    }

    [HttpPost("add"), Authorize]
    public async Task<IActionResult> CreateBook([FromBody] BookForCreationDto book)
    {
        await _bookService.CreateBookAsync(book);
        return Ok();
    }
    
    [HttpGet("edit/{id}", Name = "EditBook")]
    public async Task<IActionResult> EditBook(int id)
    {
        var bookDto = await _bookService.GetBookByIdAsync(id);
        var genres = Enum.GetValues(typeof(BookGenre)).Cast<BookGenre>()
            .Select(g => new SelectListItem
            {
                Text = g.ToString(),
                Value = g.ToString(),
                Selected = g == bookDto.Genre
            }).ToList();
    
        var authorsResult = await _authorService.GetAllAuthorsAsync(new AuthorParameters() { PageSize = 10}); 
        ViewBag.Genres = genres; 
        ViewBag.Authors = authorsResult.Items; 
        return View("~/Views/Books/EditBookPage.cshtml", bookDto);
    }


    [HttpPut("{id}", Name = "UpdateBook"), Authorize]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookForUpdateDto bookDto)
    {
        await _bookService.UpdateBookAsync(id, bookDto);
        return NoContent();
    }

    [HttpDelete("delete/{id}"), Authorize]
    public async Task<IActionResult> DeleteBook(int id)
    {
        await _bookService.DeleteBookAsync(id);
        return NoContent();
    }
}