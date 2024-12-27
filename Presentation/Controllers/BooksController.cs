using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.DataTransferObjects;
using Application.RequestFeatures;
using Domain.Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    private readonly IEditBookControllerUseCase _iEditBookControllerUseCase;
    private readonly IAddBookControllerUseCase _addBookControllerUseCase;

    public BooksController(
        IGetBooksUseCase getBooksUseCase,
        IGetBookByIdUseCase getBookByIdUseCase,
        ICreateBookUseCase createBookUseCase,
        IUpdateBookUseCase updateBookUseCase,
        IDeleteBookUseCase deleteBookUseCase,
        IEditBookControllerUseCase iIEditBookControllerUseCase,
        IAddBookControllerUseCase addBookControllerUseCase)
    {
        _getBooksUseCase = getBooksUseCase;
        _getBookByIdUseCase = getBookByIdUseCase;
        _createBookUseCase = createBookUseCase;
        _updateBookUseCase = updateBookUseCase;
        _deleteBookUseCase = deleteBookUseCase;
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
    public async Task<IActionResult> GetBooks([FromQuery] BookParameters requestParameters, 
        CancellationToken cancellationToken)
    {
        var pagedResult = await _getBooksUseCase.ExecuteAsync(requestParameters, cancellationToken);    
        return Ok(pagedResult);
    }

    [HttpGet("{id}", Name = "BookById"), Authorize(Policy = "Admin")]
    public async Task<IActionResult> GetBook(int id, CancellationToken cancellationToken)
    {
        var book = await _getBookByIdUseCase.ExecuteAsync(id, cancellationToken);
        return Ok(book);
    }
    
    [HttpGet("AddBook")]
    public async Task<IActionResult> CreateBook(CancellationToken cancellationToken)
    {
        var authorsResult = await _addBookControllerUseCase.ExecuteAsync(new AuthorParameters() { PageSize = 10},
            cancellationToken);   
        ViewBag.Genres = authorsResult.Genres; 
        ViewBag.Authors = authorsResult.Authors; 
        return View("~/Views/Books/AddBookPage.cshtml");
    }

    [HttpPost("add"), Authorize(Policy = "Admin")]
    public async Task<IActionResult> CreateBook([FromBody] BookForCreationDto book,
        CancellationToken cancellationToken)
    {
        await _createBookUseCase.ExecuteAsync(book, cancellationToken);
        return Ok();
    }
    
    [HttpGet("edit/{id}", Name = "EditBook")]
    public async Task<IActionResult> EditBook(int id, CancellationToken cancellationToken)
    {
        var pageData = await _iEditBookControllerUseCase.ExecuteAsync(id, cancellationToken);
        ViewBag.Genres = pageData.Genres;
        ViewBag.Authors = pageData.Authors;
        return View("~/Views/Books/EditBookPage.cshtml", pageData.Book);
    }

    [HttpPut("{id}", Name = "UpdateBook"), Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] BookForUpdateDto bookDto,
        CancellationToken cancellationToken)
    {
        await _updateBookUseCase.ExecuteAsync(id, bookDto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("delete/{id}"), Authorize(Policy = "Admin")]
    public async Task<IActionResult> DeleteBook(int id, CancellationToken cancellationToken)
    {
        await _deleteBookUseCase.ExecuteAsync(id, cancellationToken);
        return NoContent();
    }
}