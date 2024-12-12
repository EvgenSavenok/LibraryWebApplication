using Application.Contracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation;
using Application.Validation.CustomExceptions;
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
            throw new BadRequestException("Cannot get borrow.");
        }
        var bookDto = _mapper.Map<UserBookBorrowDto>(borrow);
        return bookDto;
    }
}
