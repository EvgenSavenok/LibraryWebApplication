﻿using Application.DataTransferObjects;

namespace Application.Contracts.UseCasesContracts.AuthUseCasesContracts;

public interface IRefreshTokenUseCase
{
    public Task<string> ExecuteAsync(TokenDto tokenDto);
}
