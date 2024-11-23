using Application.Contracts;
using Application.Contracts.ServicesContracts;
using Application.DataTransferObjects;
using Domain.Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/authors")]
[ApiController]
public class AuthorsController : Controller
{
    private readonly IAuthorService _authorService;

    public AuthorsController(IAuthorService authorService)
    {
        _authorService = authorService;
    }

    [HttpGet("authorsPage")]
    public IActionResult GetAuthorsPage()
    {
        return View("~/Views/Authors/AllAuthorsPage.cshtml");
    }

    [HttpGet("GetAuthors")]
    public async Task<IActionResult> GetAuthors([FromQuery] AuthorParameters requestParameters)
    {
        var authors = await _authorService.GetAllAuthorsAsync(requestParameters);
        var totalAuthors = await _authorService.CountAuthorsAsync(requestParameters);
        var totalPages = (int)Math.Ceiling((double)totalAuthors / requestParameters.PageSize);
        
        var response = new
        {
            authors,
            currentPage = requestParameters.PageNumber,
            totalPages
        };

        return Ok(response);
    }

    [HttpGet("AddAuthor")]
    public IActionResult AddAuthor()
    {
        return View("~/Views/Authors/AddAuthorPage.cshtml");
    }

    [HttpPost("add")]
    public async Task<IActionResult> CreateAuthor([FromBody] AuthorForCreationDto author)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        await _authorService.CreateAuthorAsync(author);
        return Ok();
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        await _authorService.DeleteAuthorAsync(id);
        return NoContent();
    }

    [HttpGet("edit/{id}", Name = "EditAuthor")]
    public async Task<IActionResult> EditAuthor(int id)
    {
        var authorDto = await _authorService.GetAuthorByIdAsync(id);
        return View("~/Views/Authors/EditAuthorPage.cshtml", authorDto);
    }

    [HttpPut("{id}", Name = "UpdateAuthor")]
    public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorForUpdateDto authorDto)
    {
        if (!ModelState.IsValid)
        {
            return UnprocessableEntity(ModelState);
        }
        await _authorService.UpdateAuthorAsync(id, authorDto);
        return NoContent();
    }
}
