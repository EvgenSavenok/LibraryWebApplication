using Application.Contracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;

namespace Application.UseCases.BorrowingUseCases;

public class GetUserBorrowUseCase : IGetUserBorrowUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetUserBorrowUseCase(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    
    public async Task<UserBookBorrowDto> ExecuteAsync(int id)
    {
        var borrow = await _repository.Borrow.GetUserBookBorrowAsync(id, trackChanges: false);
        return borrow == null ? null : _mapper.Map<UserBookBorrowDto>(borrow);
    }
}
