using Application.DataTransferObjects;

namespace Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;

public interface IGetUserBorrowUseCase
{
    public Task<UserBookBorrowDto> ExecuteAsync(int id);
}
