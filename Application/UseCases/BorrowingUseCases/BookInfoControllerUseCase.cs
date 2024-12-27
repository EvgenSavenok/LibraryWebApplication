﻿using Application.Contracts.UseCasesContracts.AuthorUseCasesContracts;
using Application.Contracts.UseCasesContracts.BookUseCasesContracts;
using Application.Contracts.UseCasesContracts.BorrowUseCasesContracts;
using Application.DataTransferObjects;

namespace Application.UseCases.BorrowingUseCases;

public class BookInfoControllerUseCase : IBookInfoControllerUseCase
{
    private readonly IGetBookByIdUseCase _getBookByIdUseCase;
    private readonly IGetAuthorByIdUseCase _getAuthorByIdUseCase;
    public BookInfoControllerUseCase(IGetBookByIdUseCase getBookByIdUseCase,
        IGetAuthorByIdUseCase getAuthorByIdUseCase)
    {
        _getBookByIdUseCase = getBookByIdUseCase;
        _getAuthorByIdUseCase = getAuthorByIdUseCase;
    }

    public async Task<PageDataDto> GetBookInfo(int bookId, CancellationToken cancellationToken)
    {
        var bookInfo = await _getBookByIdUseCase.ExecuteAsync(bookId, cancellationToken);
        var authorInfo = await _getAuthorByIdUseCase.ExecuteAsync(bookInfo.AuthorId, cancellationToken);
        
        return new PageDataDto
        {
            Book = bookInfo, 
            Author = authorInfo 
        };
    }
}
