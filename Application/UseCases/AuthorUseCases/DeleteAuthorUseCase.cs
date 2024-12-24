using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.Validation;

namespace Application.UseCases.AuthorUseCases;

public class DeleteAuthorUseCase : IDeleteAuthorUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;
    public DeleteAuthorUseCase(IRepositoryManager repository, 
        ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task ExecuteAsync(int id)
    {
        var author = await _repository.Author.GetAuthorAsync(id, trackChanges: false);
        if (author == null)
        {
            throw new NotFoundException($"Author with id {id} not found.");
        }
        _repository.Author.Delete(author);
        await _repository.SaveAsync();
    }
}
