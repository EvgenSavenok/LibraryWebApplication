using Application.DataTransferObjects;
using Domain.Entities.RequestFeatures;

namespace Application.Contracts.UseCasesContracts.BookUseCasesContracts;

public interface IAddBookControllerUseCase
{
    Task<PageDataDto> ExecuteAsync(AuthorParameters authorParameters);
}
