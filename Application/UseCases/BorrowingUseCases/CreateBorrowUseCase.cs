using Application.Contracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.Models;

namespace Application.UseCases.BorrowingUseCases;

public class CreateBorrowUseCase : ICreateBorrowUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public CreateBorrowUseCase(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task ExecuteAsync(UserBookBorrowDto borrowDto)
    {
        var borrowEntity = _mapper.Map<UserBookBorrow>(borrowDto);
        _repository.Borrow.Create(borrowEntity);
        await _repository.SaveAsync();
    }
}
