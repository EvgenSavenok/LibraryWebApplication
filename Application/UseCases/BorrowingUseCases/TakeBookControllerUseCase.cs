using System.Security.Claims;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.Validation;
using Application.Validation.CustomExceptions;
using Microsoft.AspNetCore.Http;

namespace Application.UseCases.BorrowingUseCases;

public class TakeBookControllerUseCase : ITakeBookControllerUseCase
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IGetBookByIdUseCase _getBookByIdUseCase;
    private readonly ICreateBorrowUseCase _createBorrowUseCase;
    public TakeBookControllerUseCase(IHttpContextAccessor httpContextAccessor,
        IGetBookByIdUseCase getBookByIdUseCase,
        ICreateBorrowUseCase createBorrowUseCase)
    {
        _httpContextAccessor = httpContextAccessor;
        _getBookByIdUseCase = getBookByIdUseCase;
        _createBorrowUseCase = createBorrowUseCase;
    }

    public async Task ExecuteAsync(int bookId, CancellationToken cancellationToken)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (userId == null)
            throw new UnauthorizedException("Cannot get userId");
        var book = await _getBookByIdUseCase.ExecuteAsync(bookId, cancellationToken);
        if (book == null)
            throw new NotFoundException("Cannot find book by such id");
        await _createBorrowUseCase.ExecuteAsync(userId, bookId, book, cancellationToken);
    }
}
