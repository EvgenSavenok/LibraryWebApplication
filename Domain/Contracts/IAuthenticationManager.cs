using Domain.Entities.AuthDto;
using Domain.Entities.Models;

namespace Domain.Contracts;

public interface IAuthenticationManager
{
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
    Task<TokenDto> CreateTokens(User user, bool populateExp);
    public Task<string> CreateAccessToken(User user);
}
