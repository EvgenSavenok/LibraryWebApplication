using System.Security.Claims;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
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

    public async Task ExecuteAsync(int bookId)
    {
        var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var book = await _getBookByIdUseCase.ExecuteAsync(bookId);
        await _createBorrowUseCase.ExecuteAsync(userId, bookId, book);
    }
}
