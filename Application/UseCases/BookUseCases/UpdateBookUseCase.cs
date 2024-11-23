using Application.Contracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;

namespace Application.UseCases.BookUseCases;

public class UpdateBookUseCase : IUpdateBookUseCase
{
    private readonly IRepositoryManager _repository;
    private readonly IMapper _mapper;
    private readonly ILoggerManager _logger;

    public UpdateBookUseCase(IRepositoryManager repository, IMapper mapper, ILoggerManager logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task ExecuteAsync(int bookId, BookForUpdateDto bookDto)
    {
        var bookEntity = await _repository.Book.GetBookAsync(bookId, trackChanges: true);
        if (bookEntity == null)
        {
            _logger.LogInfo($"Book with id: {bookId} doesn't exist in the database.");
        }
        _mapper.Map(bookDto, bookEntity);
        await _repository.SaveAsync();
    }
}
