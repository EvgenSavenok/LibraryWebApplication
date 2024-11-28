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
        var books = await _bookService.GetBooksAsync(requestParameters);
        var totalBooks = await _bookService.CountBooksAsync(requestParameters);
        var totalPages = (int)Math.Ceiling((double)totalBooks / requestParameters.PageSize);
        
        var response = new
        {
            books,
            currentPage = requestParameters.PageNumber,
            totalPages
        };
        return Ok(response);
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
        
        var authors = await _authorService.GetAllAuthorsAsync(null); 
        var authorSelectList = authors.Select((a, index) => new SelectListItem
        {
            Text = $"{a.Name} {a.LastName}",
            Value = a.Id.ToString(),  
            Selected = index == 0  
        }).ToList();
        
        ViewBag.Genres = genres; 
        ViewBag.Authors = authorSelectList;
        return View("~/Views/Books/AddBookPage.cshtml");
    }

    [HttpPost("add"), Authorize]
    public async Task<IActionResult> CreateBook([FromBody] BookForCreationDto book)
    {
        if (!ModelState.IsValid)
            return UnprocessableEntity(ModelState);

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
    
        var authors = await _authorService.GetAllAuthorsAsync(null);
        var authorSelectList = authors.Select(a => new SelectListItem
        {
            Text = $"{a.Name} {a.LastName}",
            Value = a.Id.ToString(), 
            Selected = a.Id == bookDto.AuthorId
        }).ToList();

        ViewBag.Genres = genres;
        ViewBag.Authors = authorSelectList;
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