﻿using System.ComponentModel.DataAnnotations;
using Application.Contracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation;
using AutoMapper;
using Domain.Entities.Models;
using FluentValidation;
using ValidationException = FluentValidation.ValidationException;

namespace Application.UseCases.BookUseCases;

public class UpdateBookUseCase : IUpdateBookUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;
    private readonly IValidator<Book> _validator;

    public UpdateBookUseCase(IRepositoryManager repository, 
        IMapper mapper, 
        ILoggerManager logger,
        IValidator<Book> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
        _validator = validator;
    }

    public async Task ExecuteAsync(int bookId, BookForUpdateDto bookDto)
    {
        var bookEntity = await _repository.Book.GetBookAsync(bookId, trackChanges: true);
        if (bookEntity == null)
        {
            _logger.LogInfo($"Book with id: {bookId} doesn't exist in the database.");
            throw new NotFoundException($"Book with id {bookId} not found.");
        }
        string oldIsbn = bookEntity.ISBN;
        _mapper.Map(bookDto, bookEntity);
        var validationResult = await _validator.ValidateAsync(bookEntity);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        if (oldIsbn != bookDto.ISBN)
        {
            var existingBook = await _repository.Book.GetBookByIsbnAsync(bookEntity.ISBN);
            if (existingBook != null)
            {
                _logger.LogInfo($"A book with ISBN: {bookEntity.ISBN} already exists in the database.");
                throw new ConflictException("A book with this ISBN already exists.");
            }
        }
        await _repository.SaveAsync();
    }

}
