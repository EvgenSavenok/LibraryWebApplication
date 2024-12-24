using Application.Contracts;
using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.DataTransferObjects;
using Application.RequestFeatures;
using Application.Validation;
using AutoMapper;
using Domain.Entities.RequestFeatures;

namespace Application.UseCases.BookUseCases;

public class GetBooksUseCase : IGetBooksUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public GetBooksUseCase(IRepositoryManager repository, 
        IMapper mapper,
        ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedResult<BookDto>> ExecuteAsync(BookParameters bookParameters)
    {
        var books = await _repository.Book.GetAllBooksAsync(bookParameters, trackChanges: false);

        if (books == null || !books.Any())
        {
            _logger.LogInfo($"No books found for the given parameters: {bookParameters}.");
            return new PagedResult<BookDto>
            {
                Items = Enumerable.Empty<BookDto>(),
                TotalCount = 0,
                TotalPages = 0,
                CurrentPage = bookParameters.PageNumber
            };
        }

        var totalBooks = await _repository.Book.CountBooksAsync(bookParameters);
        var totalPages = (int)Math.Ceiling((double)totalBooks / bookParameters.PageSize);

        return new PagedResult<BookDto>
        {
            Items = _mapper.Map<IEnumerable<BookDto>>(books),
            TotalCount = totalBooks,
            TotalPages = totalPages,
            CurrentPage = bookParameters.PageNumber
        };
    }
}
