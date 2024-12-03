using Application.Contracts.ServicesContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.DataTransferObjects;
using Application.UseCases.BookUseCases;
using Domain.Entities.RequestFeatures;

namespace Application.Services;

public class BookService : IBookService
{
    private readonly IGetBooksUseCase _getBooksUseCase;
    private readonly IGetBookByIdUseCase _getBookByIdUseCase;
    private readonly ICreateBookUseCase _createBookUseCase;
    private readonly IUpdateBookUseCase _updateBookUseCase;
    private readonly IDeleteBookUseCase _deleteBookUseCase;
    private readonly ICountBooksUseCase _countBooksUseCase;

    public BookService(
        IGetBooksUseCase getBooksUseCase,
        IGetBookByIdUseCase getBookByIdUseCase,
        ICreateBookUseCase createBookUseCase,
        IUpdateBookUseCase updateBookUseCase,
        IDeleteBookUseCase deleteBookUseCase,
        ICountBooksUseCase countBooksUseCase)
    {
        _getBooksUseCase = getBooksUseCase;
        _getBookByIdUseCase = getBookByIdUseCase;
        _createBookUseCase = createBookUseCase;
        _updateBookUseCase = updateBookUseCase;
        _deleteBookUseCase = deleteBookUseCase;
        _countBooksUseCase = countBooksUseCase;
    }

    public async Task<PagedResult<BookDto>> GetBooksAsync(BookParameters bookParameters)
        => await _getBooksUseCase.ExecuteAsync(bookParameters);

    public async Task<BookDto> GetBookByIdAsync(int bookId)
        => await _getBookByIdUseCase.ExecuteAsync(bookId);

    public async Task CreateBookAsync(BookForCreationDto bookDto)
        => await _createBookUseCase.ExecuteAsync(bookDto);

    public async Task UpdateBookAsync(int bookId, BookForUpdateDto bookDto)
        => await _updateBookUseCase.ExecuteAsync(bookId, bookDto);

    public async Task DeleteBookAsync(int bookId)
        => await _deleteBookUseCase.ExecuteAsync(bookId);

    public async Task<int> CountBooksAsync(BookParameters requestParameters)
        => await _countBooksUseCase.ExecuteAsync(requestParameters);
}