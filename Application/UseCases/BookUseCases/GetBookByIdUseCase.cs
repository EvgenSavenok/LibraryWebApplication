﻿using System.Collections;
using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation;
using AutoMapper;

namespace Application.UseCases.BookUseCases;

public class GetBookByIdUseCase : IGetBookByIdUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public GetBookByIdUseCase(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<BookDto> ExecuteAsync(int bookId, CancellationToken cancellationToken)
    {
        var book = await _repository.Book.GetBookAsync(bookId, trackChanges: false, cancellationToken);
        if (book == null)
        {
            throw new NotFoundException($"Book with id {bookId} not found.");
        }
        return _mapper.Map<BookDto>(book);
    }
}
