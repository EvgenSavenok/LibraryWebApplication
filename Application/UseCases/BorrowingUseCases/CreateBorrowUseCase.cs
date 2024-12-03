using Application.Contracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.Models;
using FluentValidation;

namespace Application.UseCases.BorrowingUseCases;

public class CreateBorrowUseCase : ICreateBorrowUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly IValidator<UserBookBorrow> _validator;

    public CreateBorrowUseCase(IRepositoryManager repository,
        IMapper mapper,
        IValidator<UserBookBorrow> validator)
    {
        _repository = repository;
        _mapper = mapper;
        _validator = validator;
    }

    public async Task ExecuteAsync(UserBookBorrowDto borrowDto)
    {
        var borrowEntity = _mapper.Map<UserBookBorrow>(borrowDto);
        var validationResult = await _validator.ValidateAsync(borrowEntity);
        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }
        _repository.Borrow.Create(borrowEntity);
        await _repository.SaveAsync();
    }
}
