using Application.Contracts.ServicesContracts;
using Application.Contracts.UseCasesContracts.AuthUseCasesContracts;
using Application.DataTransferObjects;
using Domain.Entities.AuthDto;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AuthService : IAuthService
{
    private readonly IRegisterUserUseCase _registerUserUseCase;
    private readonly IAuthenticateUserUseCase _authenticateUserUseCase;
    private readonly IRefreshTokenUseCase _refreshTokenUseCase;

    public AuthService(
        IRegisterUserUseCase registerUserUseCase,
        IAuthenticateUserUseCase authenticateUserUseCase,
        IRefreshTokenUseCase refreshTokenUseCase)
    {
        _registerUserUseCase = registerUserUseCase;
        _authenticateUserUseCase = authenticateUserUseCase;
        _refreshTokenUseCase = refreshTokenUseCase;
    }

    public async Task<IdentityResult> RegisterUserAsync(UserForRegistrationDto userForRegistration)
        => await _registerUserUseCase.ExecuteAsync(userForRegistration);

    public async Task<(string AccessToken, string RefreshToken)> AuthenticateUserAsync(UserForAuthenticationDto userForLogin)
        => await _authenticateUserUseCase.ExecuteAsync(userForLogin);

    public async Task<TokenDto> RefreshToken(TokenDto tokenDto)
        => await _refreshTokenUseCase.ExecuteAsync(tokenDto);
}
