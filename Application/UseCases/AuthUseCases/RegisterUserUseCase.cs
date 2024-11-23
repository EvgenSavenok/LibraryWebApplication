using Application.Contracts;
using Application.Contracts.UseCasesContracts.AuthUseCasesContracts;
using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.UseCases.AuthUseCases;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IMapper _mapper;
    private readonly UserManager<User> _userManager;
    private readonly ILoggerManager _logger;

    public RegisterUserUseCase(IMapper mapper, UserManager<User> userManager, ILoggerManager logger)
    {
        _mapper = mapper;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<IdentityResult> ExecuteAsync(UserForRegistrationDto userForRegistration)
    {
        var user = _mapper.Map<User>(userForRegistration);
        var result = await _userManager.CreateAsync(user, userForRegistration.Password);

        if (result.Succeeded)
        {
            var userRoleAsString = userForRegistration.Role.ToString();
            await _userManager.AddToRolesAsync(user, new List<string> { userRoleAsString });
        }
        else
        {
            _logger.LogError("Registration failed");
        }

        return result;
    }
}
