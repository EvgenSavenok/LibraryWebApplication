using Application.DataTransferObjects;

namespace Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;

public interface IBookInfoControllerUseCase
{
    public Task<PageDataDto> GetBookInfo(int bookId);
}
