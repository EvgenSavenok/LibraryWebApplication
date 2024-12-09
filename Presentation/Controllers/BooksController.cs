using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
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
    private readonly IGetBooksUseCase _getBooksUseCase;
    private readonly IGetBookByIdUseCase _getBookByIdUseCase;
    private readonly ICreateBookUseCase _createBookUseCase;
    private readonly IUpdateBookUseCase _updateBookUseCase;
    private readonly IDeleteBookUseCase _deleteBookUseCase;
    private readonly IGetAllAuthorsUseCase _getAllAuthorsUseCase;

    public BooksController(
        IGetBooksUseCase getBooksUseCase,
        IGetBookByIdUseCase getBookByIdUseCase,
        ICreateBookUseCase createBookUseCase,
        IUpdateBookUseCase updateBookUseCase,
        IDeleteBookUseCase deleteBookUseCase,
        ICountBooksUseCase countBooksUseCase,
        IGetAllAuthorsUseCase getAllAuthorsUseCase)
    {
        _getBooksUseCase = getBooksUseCase;
        _getBookByIdUseCase = getBookByIdUseCase;
        _createBookUseCase = createBookUseCase;
        _updateBookUseCase = updateBookUseCase;
        _deleteBookUseCase = deleteBookUseCase;
        _getAllAuthorsUseCase = getAllAuthorsUseCase;
    }
    
    [HttpGet("admin")]
    public IActionResult BooksPageAdmin()
    {
        return View("~/Views/Books/AllBooksPage.cshtml");
    }

    [HttpGet("GetBooks")]
    [Authorize(Policy = "AdminOrUser")]
    public async Task<IActionResult> GetBooks([FromQuery] BookParameters requestParameters)
    {
        var pagedResult = await _getBooksUseCase.ExecuteAsync(requestParameters);    
        return Ok(new
        {
            books = pagedResult.Items,
            currentPage = pagedResult.CurrentPage,
            totalPages = pagedResult.TotalPages,
            totalBooks = pagedResult.TotalCount
        });
    }

    [HttpGet("{id}", Name = "BookById"), Authorize(Policy = "Admin")]
    public async Task<IActionResult> GetBook(int id)
    {
        var book = await _getBookByIdUseCase.ExecuteAsync(id);
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

        var authorsResult = await _getAllAuthorsUseCase.ExecuteAsync(new AuthorParameters() { PageSize = 10});   
        ViewBag.Genres = genres; 
        ViewBag.Authors = authorsResult.Items; 
        return View("~/Views/Books/AddBookPage.cshtml");
    }

    [HttpPost("add"), Authorize(Policy = "Admin")]
    public async Task<IActionResult> CreateBook([FromBody] BookForCreationDto book)
    {
        await _createBookUseCase.ExecuteAsync(book);
        return Ok();
    }
    
    [HttpGet("edit/{id}", Name = "EditBook")]
    public async Task<IActionResult> EditBook(int id)
    {
        var bookDto = await _getBookByIdUseCase.ExecuteAsync(id);
        var genres = Enum.GetValues(typeof(BookGenre)).Cast<BookGenre>()
            .Select(g => new SelectListItem
            {
                Text = g.ToString(),
                Value = g.ToString(),
                Selected = g == bookDto.Genre
            }).ToList();
    
        var authorsResult = await _getAllAuthorsUseCase.ExecuteAsync(new AuthorParameters() { PageSize = 10 });
        ViewBag.Genres = genres; 
        ViewBag.Authors = authorsResult.Items; 
        return View("~/Views/Books/EditBookPage.cshtml", bookDto);
    }

    [HttpPut("{id}", Name = "UpdateBook"), Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookForUpdateDto bookDto)
    {
        await _updateBookUseCase.ExecuteAsync(id, bookDto);
        return NoContent();
    }

    [HttpDelete("delete/{id}"), Authorize(Policy = "Admin")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        await _deleteBookUseCase.ExecuteAsync(id);
        return NoContent();
    }
}