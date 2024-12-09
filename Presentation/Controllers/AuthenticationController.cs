using Application.Contracts.UseCasesContracts.AuthUseCasesContracts;
using Application.DataTransferObjects;
using Domain.Entities.AuthDto;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : Controller
{
    private readonly IRegisterUserUseCase _registerUserUseCase;
    private readonly IAuthenticateUserUseCase _authenticateUserUseCase;
    private readonly IRefreshTokenUseCase _refreshTokenUseCase;

    public AuthenticationController(
        IRegisterUserUseCase registerUserUseCase,
        IAuthenticateUserUseCase authenticateUserUseCase,
        IRefreshTokenUseCase refreshTokenUseCase)
    {
        _registerUserUseCase = registerUserUseCase;
        _authenticateUserUseCase = authenticateUserUseCase;
        _refreshTokenUseCase = refreshTokenUseCase;
    }

    [HttpGet("registerPage")]
    public IActionResult RegisterPage()
    {
        return View("~/Views/Auth/RegisterPage.cshtml");
    }
    
    [HttpGet("loginPage")]
    public IActionResult LoginPage()
    {
        return View("~/Views/Auth/LoginPage.cshtml");
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto userForRegistration)
    {
        var result = await _registerUserUseCase.ExecuteAsync(userForRegistration);
        return result.Succeeded 
            ? StatusCode(201) 
            : BadRequest(result.Errors);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForLogin)
    {
        var (accessToken, refreshToken) = await _authenticateUserUseCase.ExecuteAsync(userForLogin);
        return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
    }
}
