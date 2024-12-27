using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;
using Domain.Models;
using FluentValidation;

namespace Application.UseCases.AuthorUseCases;

public class CreateAuthorUseCase : ICreateAuthorUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<Author> _validator;

    public CreateAuthorUseCase(IRepositoryManager repository, 
        IMapper mapper,
        IValidator<Author> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }
    
    public async Task ExecuteAsync(AuthorForCreationDto author, CancellationToken cancellationToken)
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
        
        var authorEntity = _mapper.Map<Author>(author);
        var validationResult = await _validator.ValidateAsync(authorEntity);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        _repository.Author.Create(authorEntity);
        await _repository.SaveAsync();
    }
}
