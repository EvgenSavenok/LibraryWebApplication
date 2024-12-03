using Application.DataTransferObjects;
using Domain.Entities.AuthDto;
using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.ServicesContracts;

public interface IAuthService
{
    Task<IdentityResult> RegisterUserAsync(UserForRegistrationDto userForRegistration);
    Task<(string AccessToken, string RefreshToken)> AuthenticateUserAsync(UserForAuthenticationDto user);
    Task<string> RefreshToken(TokenDto tokenDto);
}