using Application.Contracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;

namespace Application.UseCases.BookUseCases;

public class DeleteBookUseCase : IDeleteBookUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public DeleteBookUseCase(IRepositoryManager repository, ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task ExecuteAsync(int bookId)
    {
        var book = await _repository.Book.GetBookAsync(bookId, trackChanges: false);
        if (book == null)
        {
            _logger.LogInfo($"Book with id: {bookId} doesn't exist in the database.");
        }
        _repository.Book.Delete(book);
        await _repository.SaveAsync();
    }
}
