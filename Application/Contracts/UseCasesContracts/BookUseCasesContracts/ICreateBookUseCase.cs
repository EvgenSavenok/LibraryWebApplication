using Application.DataTransferObjects;

namespace Application.Contracts.UseCasesContracts.BookUseCasesContracts;

public interface ICreateBookUseCase
{
    public Task ExecuteAsync(BookForCreationDto bookDto, CancellationToken token);
}
