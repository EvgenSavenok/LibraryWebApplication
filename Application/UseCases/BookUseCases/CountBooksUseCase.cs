using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.RequestFeatures;
using Application.Validation;
using Application.Validation.CustomExceptions;
using Domain.Entities.RequestFeatures;

namespace Application.UseCases.BookUseCases;

public class CountBooksUseCase : ICountBooksUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerManager _logger;

    public CountBooksUseCase(IRepositoryManager repository,
        ILoggerManager logger)
    {
        _repository = repository;
        _logger = logger;
    }
    public async Task<int> ExecuteAsync(BookParameters requestParameters)
    {
        Task<int> numOfBooks = _repository.Book.CountBooksAsync(requestParameters);
        if (numOfBooks == null)
        {
            throw new BadRequestException("Cannot count number of books.");
        }
        return await numOfBooks;
    }
}
