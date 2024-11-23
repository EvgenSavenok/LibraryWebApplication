using Application.Contracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using AutoMapper;
using Domain.Entities.RequestFeatures;

namespace Application.UseCases.BorrowingUseCases;

public class CountBorrowsUseCase : ICountBorrowsUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public CountBorrowsUseCase(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<int> ExecuteAsync(BorrowParameters borrowParameters)
    {
        return await _repository.Borrow.CountBorrowsAsync(borrowParameters);
    }
}
