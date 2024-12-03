using Application.Contracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation;
using AutoMapper;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Application.UseCases.BorrowingUseCases;

public class GetUsersBorowsUseCase : IGetUsersBorowsUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    private readonly IMapper _mapper;

    public GetUsersBorowsUseCase(IRepositoryManager repository,
        ILoggerManager logger,
        IMapper mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }
    public async Task<PagedResult<UserBookBorrow>> ExecuteAsync(BorrowParameters borrowParameters, string userId)
    {
        var borrowsAsync = await _repository.Borrow.GetAllUserBookBorrowsAsync(borrowParameters, userId, trackChanges: false);
        if (borrowsAsync == null || !borrowsAsync.Any())
        {
            return new PagedResult<UserBookBorrow>
            {
                Items = Enumerable.Empty<UserBookBorrow>(),
                TotalCount = 0,
                TotalPages = 0,
                CurrentPage = borrowParameters.PageNumber
            };
        }
        var totalBooks = await _repository.Borrow.CountBorrowsAsync(borrowParameters);
        var totalPages = (int)Math.Ceiling((double)totalBooks / borrowParameters.PageSize);
        
        return new PagedResult<UserBookBorrow>
        {
            Items = borrowsAsync,
            TotalCount = totalBooks,
            TotalPages = totalPages,
            CurrentPage = borrowParameters.PageNumber
        };
    }
}
