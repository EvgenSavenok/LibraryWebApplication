using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.DataTransferObjects;
using Domain.Entities.Models;
using Domain.Entities.RequestFeatures;

namespace Application.UseCases.BookUseCases;

public class AddBookControllerUseCase : IAddBookControllerUseCase
{
    private readonly IGetAllAuthorsUseCase _getAllAuthorsUseCase;

    public AddBookControllerUseCase(IGetAllAuthorsUseCase getAllAuthorsUseCase)
    {
        _getAllAuthorsUseCase = getAllAuthorsUseCase;
    }
    
    public async Task<PageDataDto> ExecuteAsync(AuthorParameters authorParameters)
    {
        BookGenre defaultGenre = BookGenre.Adventures;

        var genres = Enum.GetValues(typeof(BookGenre))
            .Cast<BookGenre>()
            .Select(g => new GenreDto
            {
                Text = g.ToString(),
                Value = g.ToString(),
                Selected = g == defaultGenre 
            })
            .ToList();
        var authorsResult = await _getAllAuthorsUseCase.ExecuteAsync(authorParameters);

        return new PageDataDto
        {
            Genres = genres,
            Authors = authorsResult.Items
        };
    }
}
