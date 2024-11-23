using Domain.Entities.AuthDto;
using Domain.Entities.Models;

namespace Domain.Contracts;

public interface IAuthenticationManager
{
    Task<bool> ValidateUser(UserForAuthenticationDto userForAuth);
    Task<TokenDto> CreateToken(User user, bool populateExp);
}
