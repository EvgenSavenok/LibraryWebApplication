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
    private readonly IEditBookControllerUseCase _iEditBookControllerUseCase;
    private readonly IAddBookControllerUseCase _addBookControllerUseCase;

    public BooksController(
        IGetBooksUseCase getBooksUseCase,
        IGetBookByIdUseCase getBookByIdUseCase,
        ICreateBookUseCase createBookUseCase,
        IUpdateBookUseCase updateBookUseCase,
        IDeleteBookUseCase deleteBookUseCase,
        IGetAllAuthorsUseCase getAllAuthorsUseCase,
        IEditBookControllerUseCase iIEditBookControllerUseCase,
        IAddBookControllerUseCase addBookControllerUseCase)
    {
        _getBooksUseCase = getBooksUseCase;
        _getBookByIdUseCase = getBookByIdUseCase;
        _createBookUseCase = createBookUseCase;
        _updateBookUseCase = updateBookUseCase;
        _deleteBookUseCase = deleteBookUseCase;
        _getAllAuthorsUseCase = getAllAuthorsUseCase;
        _iEditBookControllerUseCase = iIEditBookControllerUseCase;
        _addBookControllerUseCase = addBookControllerUseCase;
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
        return Ok(pagedResult);
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
        var authorsResult = await _addBookControllerUseCase.ExecuteAsync(new AuthorParameters() { PageSize = 10});   
        ViewBag.Genres = authorsResult.Genres; 
        ViewBag.Authors = authorsResult.Authors; 
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
        var pageData = await _iEditBookControllerUseCase.ExecuteAsync(id);
        ViewBag.Genres = pageData.Genres;
        ViewBag.Authors = pageData.Authors;
        return View("~/Views/Books/EditBookPage.cshtml", pageData.Book);
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