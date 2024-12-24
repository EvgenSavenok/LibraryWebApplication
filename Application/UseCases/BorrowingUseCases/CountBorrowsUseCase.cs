using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.RequestFeatures;
using Application.Validation;
using Application.Validation.CustomExceptions;
using AutoMapper;
using Domain.Entities.RequestFeatures;

namespace Application.UseCases.BorrowingUseCases;

public class CountBorrowsUseCase : ICountBorrowsUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public CountBorrowsUseCase(IRepositoryManager repository, 
        IMapper mapper,
        ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<int> ExecuteAsync(BorrowParameters borrowParameters)
    {
        Task<int> countOfBorrowsAsync = _repository.Borrow.CountBorrowsAsync(borrowParameters);
        if (countOfBorrowsAsync == null)
        {
            throw new BadRequestException("Cannot count number of borrows.");
        }
        return await countOfBorrowsAsync;
    }
}
