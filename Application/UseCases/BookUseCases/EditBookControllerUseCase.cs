using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.DataTransferObjects;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Application.UseCases.BookUseCases;

public class EditBookControllerUseCase : IEditBookControllerUseCase
{
    private readonly IGetAllAuthorsUseCase _getAllAuthorsUseCase;
    private readonly IGetBookByIdUseCase _getBookByIdUseCase;

    public EditBookControllerUseCase(IGetAllAuthorsUseCase getAllAuthorsUseCase,
        IGetBookByIdUseCase getBookByIdUseCase)
    {
        _getAllAuthorsUseCase = getAllAuthorsUseCase;
        _getBookByIdUseCase = getBookByIdUseCase;
    }
    public async Task<PageDataDto> ExecuteAsync(int bookId)
    {
        var bookDto = await _getBookByIdUseCase.ExecuteAsync(bookId);
        BookGenre defaultGenre = BookGenre.Adventures;
        var genres = Enum.GetValues(typeof(BookGenre))
            .Cast<BookGenre>()
            .Select(g => new GenreDto
            {
                Text = g.ToString(),
                Value = g.ToString(),
                Selected = g == defaultGenre 
            }).ToList();

        var authorsResult = await _getAllAuthorsUseCase.ExecuteAsync(new AuthorParameters { PageSize = 10 });

        return new PageDataDto
        {
            Genres = genres,
            Authors = authorsResult.Items,
            Book = bookDto
        };
    }
}
