using Application.DataTransferObjects;

namespace Application.Contracts.UseCasesContracts.BookUseCasesContracts;

public interface IEditBookWithAuthorsUseCase
{
    Task<AddBookPageDataDto> ExecuteAsync(int bookId);
}
