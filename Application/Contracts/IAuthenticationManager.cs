using Application.DataTransferObjects;
using Domain.Models;

namespace Application.Contracts;

public interface IAuthenticationManager
{
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
    Task<TokenDto> CreateTokens(User user, bool populateExp);
    public Task<string> CreateAccessToken(User user);
}
