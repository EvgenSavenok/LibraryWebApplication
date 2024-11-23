using Application.DataTransferObjects;
using Microsoft.AspNetCore.Identity;

namespace Application.Contracts.UseCasesContracts.AuthUseCasesContracts;

public interface IRegisterUserUseCase
{
    public Task<IdentityResult> ExecuteAsync(UserForRegistrationDto userForRegistration);
}
