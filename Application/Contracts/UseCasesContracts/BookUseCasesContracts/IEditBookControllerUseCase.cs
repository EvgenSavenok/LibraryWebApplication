using Application.DataTransferObjects;

namespace Application.Contracts.UseCasesContracts.BookUseCasesContracts;

public interface IEditBookControllerUseCase
{
    Task<PageDataDto> ExecuteAsync(int bookId, CancellationToken cancellationToken);
}
