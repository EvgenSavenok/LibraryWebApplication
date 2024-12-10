using Application.DataTransferObjects;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Application.Contracts.UseCasesContracts.BookUseCasesContracts;

public interface IAddBookControllerUseCase
{
    Task<AddBookPageDataDto> ExecuteAsync(AuthorParameters authorParameters);
}
