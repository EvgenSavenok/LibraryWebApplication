using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation;
using AutoMapper;
using Domain.Models;
using FluentValidation;

namespace Application.UseCases.AuthorUseCases;

public class UpdateAuthorUseCase : IUpdateAuthorUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<Author> _validator;
    private readonly ILoggerManager _logger;

    public UpdateAuthorUseCase(IRepositoryManager repository, 
        IMapper mapper,
        IValidator<Author> validator,
        ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
        _logger = logger;
    }
    public async Task ExecuteAsync(int id, AuthorForUpdateDto author, CancellationToken cancellationToken)
    {
        if (!DateTime.TryParse(author.BirthDate, out var birthDate))
        {
            throw new ValidationException("Invalid date format.");
        }
    
        if (birthDate > DateTime.Now)
        {
            throw new ValidationException("Birth date cannot be in the future.");
        }

        if (birthDate < DateTime.Now.AddYears(-120))
        {
            throw new ValidationException("Birth date is too far in the past.");
        }
        
        var authorEntity = await _repository.Author.GetAuthorAsync(id, trackChanges: true, cancellationToken);
        if (authorEntity == null)
        {
            throw new NotFoundException($"Author with id {id} not found.");
        }
        _mapper.Map(author, authorEntity);
        var validationResult = await _validator.ValidateAsync(authorEntity);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        await _repository.SaveAsync();
    }
}
