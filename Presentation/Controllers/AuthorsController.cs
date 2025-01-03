﻿using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.DataTransferObjects;
using Domain.Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/authors")]
[ApiController]
public class AuthorsController : Controller
{
    private readonly ICreateAuthorUseCase _createAuthorUseCase;
    private readonly IDeleteAuthorUseCase _deleteAuthorUseCase;
    private readonly IGetAllAuthorsUseCase _getAllAuthorsUseCase;
    private readonly IGetAuthorByIdUseCase _getAuthorByIdUseCase;
    private readonly IUpdateAuthorUseCase _updateAuthorUseCase;

    public AuthorsController(ICountAuthorsUseCase countAuthorsUseCase,
        ICreateAuthorUseCase createAuthorUseCase,
        IDeleteAuthorUseCase deleteAuthorUseCase,
        IGetAllAuthorsUseCase getAllAuthorsUseCase,
        IGetAuthorByIdUseCase getAuthorByIdUseCase,
        IUpdateAuthorUseCase updateAuthorUseCase)
    {
        _createAuthorUseCase = createAuthorUseCase;
        _deleteAuthorUseCase = deleteAuthorUseCase;
        _getAllAuthorsUseCase = getAllAuthorsUseCase;
        _getAuthorByIdUseCase = getAuthorByIdUseCase;
        _updateAuthorUseCase = updateAuthorUseCase;
    }

    [HttpGet("authorsPage")]
    public IActionResult GetAuthorsPage()
    {
        return View("~/Views/Authors/AllAuthorsPage.cshtml");
    }

    [HttpGet("GetAuthors")]
    public async Task<IActionResult> GetAuthors([FromQuery] AuthorParameters requestParameters, CancellationToken cancellationToken)
    {
        var pagedResult = await _getAllAuthorsUseCase.ExecuteAsync(requestParameters, cancellationToken);
        return Ok(pagedResult);
    }

    [HttpGet("AddAuthor")]
    public IActionResult AddAuthor()
    {
        return View("~/Views/Authors/AddAuthorPage.cshtml");
    }

    [HttpPost("add"), Authorize(Policy = "Admin")]
    public async Task<IActionResult> CreateAuthor([FromBody] AuthorForCreationDto author, CancellationToken cancellationToken)
    {
        await _createAuthorUseCase.ExecuteAsync(author, cancellationToken);
        return Ok();
    }

    [HttpDelete("delete/{id}"), Authorize(Policy = "Admin")]
    public async Task<IActionResult> DeleteAuthor(int id, CancellationToken cancellationToken)
    {
        await _deleteAuthorUseCase.ExecuteAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpGet("edit/{id}", Name = "EditAuthor")]
    public async Task<IActionResult> EditAuthor(int id, CancellationToken cancellationToken)
    {
        var authorDto = await _getAuthorByIdUseCase.ExecuteAsync(id, cancellationToken);
        return View("~/Views/Authors/EditAuthorPage.cshtml", authorDto);
    }

    [HttpPut("{id}", Name = "UpdateAuthor"), Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateAuthor(int id, [FromBody] AuthorForUpdateDto authorDto, CancellationToken cancellationToken)
    {
        await _updateAuthorUseCase.ExecuteAsync(id, authorDto, cancellationToken);
        return NoContent();
    }
}
