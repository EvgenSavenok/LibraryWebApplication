using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation;
using AutoMapper;
using Domain.Entities.RequestFeatures;
using Domain.Models;

namespace Application.UseCases.BookUseCases;

public class EditBookControllerUseCase : IEditBookControllerUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;

    public EditBookControllerUseCase(IRepositoryManager repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<PageDataDto> ExecuteAsync(int bookId, CancellationToken cancellationToken)
    {
        var book = await _repository.Book.GetBookAsync(bookId, trackChanges: false, cancellationToken);
        if (book == null)
        {
            throw new NotFoundException($"Book with id {bookId} not found.");
        }
        
        BookDto bookDto = _mapper.Map<BookDto>(book);
        
        BookGenre defaultGenre = BookGenre.Adventures;
        var genres = Enum.GetValues(typeof(BookGenre))
            .Cast<BookGenre>()
            .Select(g => new GenreDto
            {
                Text = g.ToString(),
                Value = g.ToString(),
                Selected = g == defaultGenre 
            }).ToList();
        
        var authors = await _repository.Author.GetAllAuthorsAsync(new AuthorParameters { PageSize = 10 },
            trackChanges: false, cancellationToken);
        
        if (authors == null || !authors.Any())
        {
            return new PageDataDto
            {
                Authors = Enumerable.Empty<AuthorDto>(),
                Genres = genres
            };
        }
        
        return new PageDataDto
        {
            Genres = genres,
            Authors = _mapper.Map<IEnumerable<AuthorDto>>(authors),
            Book = bookDto
        };
    }
}
