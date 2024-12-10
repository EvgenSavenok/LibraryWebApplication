using Application.Contracts;
using Application.Contracts.UseCasesContracts.AuthUseCasesContracts;
using Application.Validation.CustomExceptions;
using Domain.Contracts;
using Domain.Entities.AuthDto;
using Domain.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.AuthUseCases;

public class AuthenticateUserUseCase : IAuthenticateUserUseCase
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthenticationManager _authManager;
    private readonly ILoggerManager _logger;

    public AuthenticateUserUseCase(UserManager<User> userManager, IAuthenticationManager authManager, ILoggerManager logger)
    {
        _userManager = userManager;
        _authManager = authManager;
        _logger = logger;
    }

    public async Task<(string AccessToken, string RefreshToken)> ExecuteAsync(UserForAuthenticationDto userForLogin)
    {
        var user = await _userManager.FindByNameAsync(userForLogin.UserName);
        if (user == null || !await _userManager.CheckPasswordAsync(user, userForLogin.Password))
        {
            throw new UnauthorizedException("Cannot login");
        }
        await _authManager.ValidateUser(userForLogin);
        var tokenDto = await _authManager.CreateTokens(user, populateExp: true);
        if (tokenDto.AccessToken == null || tokenDto.RefreshToken == null)
        {
            throw new UnauthorizedException("Cannot create access or refresh token");
        }
        return (tokenDto.AccessToken, tokenDto.RefreshToken);
    }
}
