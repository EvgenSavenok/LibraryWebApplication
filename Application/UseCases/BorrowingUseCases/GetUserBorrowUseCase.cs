using Application.Contracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation;
using AutoMapper;

namespace Application.UseCases.BorrowingUseCases;

public class GetUserBorrowUseCase : IGetUserBorrowUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public GetUserBorrowUseCase(IRepositoryManager repository, 
        IMapper mapper,
        ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<UserBookBorrowDto> ExecuteAsync(int id)
    {
        var borrow = await _repository.Borrow.GetUserBookBorrowAsync(id, trackChanges: false);
        if (borrow == null)
        {
            _logger.LogInfo("Cannot count number of borrows.");
            throw new ConflictException("Cannot count number of borrows.");
        }
        return _mapper.Map<UserBookBorrowDto>(borrow);
    }
}
