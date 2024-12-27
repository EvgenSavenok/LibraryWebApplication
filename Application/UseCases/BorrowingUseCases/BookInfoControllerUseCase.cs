using Application.Contracts.RepositoryContracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;
using Application.Validation;
using AutoMapper;

namespace Application.UseCases.BorrowingUseCases;

public class BookInfoControllerUseCase : IBookInfoControllerUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    public BookInfoControllerUseCase(IRepositoryManager repository,
        IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<PageDataDto> GetBookInfo(int bookId, CancellationToken cancellationToken)
    {
        var book = await _repository.Book.GetBookAsync(bookId, trackChanges: false, cancellationToken);
        if (book == null)
        {
            throw new NotFoundException($"Book with id {bookId} not found.");
        }
        BookDto bookDto = _mapper.Map<BookDto>(book);
        
        var author = await _repository.Author.GetAuthorAsync(bookDto.AuthorId, 
            trackChanges: false, cancellationToken: cancellationToken);
        if (author == null)
        {
            throw new NotFoundException($"Author with id {bookDto.AuthorId} not found.");
        }
        AuthorDto authorDto = _mapper.Map<AuthorDto>(author);
        
        return new PageDataDto
        {
            Book = bookDto, 
            Author = authorDto 
        };
    }
}
