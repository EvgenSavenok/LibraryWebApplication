using Application.Contracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.RequestFeatures;

namespace Application.UseCases.BookUseCases;

public class GetBooksUseCase : IGetBooksUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public GetBooksUseCase(IRepositoryManager repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<IEnumerable<BookDto>> ExecuteAsync(BookParameters bookParameters)
    {
        var books = await _repository.Book.GetAllBooksAsync(bookParameters, trackChanges: false);
        return _mapper.Map<IEnumerable<BookDto>>(books);
    }
}
