﻿using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.Validation;

namespace Application.UseCases.BookUseCases;

public class DeleteBookUseCase : IDeleteBookUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public DeleteBookUseCase(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task ExecuteAsync(int bookId, CancellationToken cancellationToken)
    {
        var book = await _repository.Book.GetBookAsync(bookId, trackChanges: false, cancellationToken);
        if (book == null)
        {
            throw new NotFoundException($"Book with id {bookId} not found.");
        }
        _repository.Book.Delete(book);
        await _repository.SaveAsync();
    }
}
