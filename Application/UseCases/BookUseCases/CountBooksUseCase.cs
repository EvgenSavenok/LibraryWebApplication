using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.RequestFeatures;
using Application.Validation.CustomExceptions;

namespace Application.UseCases.BookUseCases;

public class CountBooksUseCase : ICountBooksUseCase
{
    private readonly IRepositoryManager _repository;

    public CountBooksUseCase(IRepositoryManager repository)
    {
        _repository = repository;
    }
    public async Task<int> ExecuteAsync(BookParameters requestParameters, CancellationToken cancellationToken)
    {
        Task<int> numOfBooks = _repository.Book.CountBooksAsync(requestParameters, cancellationToken);
        if (numOfBooks == null)
        {
            throw new BadRequestException("Cannot count number of books.");
        }
        return await numOfBooks;
    }
}
