using Application.Contracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Domain.Entities.RequestFeatures;

namespace Application.UseCases.BookUseCases;

public class CountBooksUseCase : ICountBooksUseCase
{
    private readonly IRepositoryManager _repository;

    public CountBooksUseCase(IRepositoryManager repository)
    {
        _repository = repository;
    }
    public async Task<int> ExecuteAsync(BookParameters requestParameters)
    {
        return await _repository.Book.CountBooksAsync(requestParameters);
    }
}
