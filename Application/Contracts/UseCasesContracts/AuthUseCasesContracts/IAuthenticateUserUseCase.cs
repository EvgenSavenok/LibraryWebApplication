using Domain.Entities.AuthDto;

namespace Application.Contracts.UseCasesContracts.AuthUseCasesContracts;

public interface IAuthenticateUserUseCase
{
    public Task<(string AccessToken, string RefreshToken)> ExecuteAsync(UserForAuthenticationDto userForLogin);
}
