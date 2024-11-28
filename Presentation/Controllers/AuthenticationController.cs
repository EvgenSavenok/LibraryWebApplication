using System.Security.Claims;
using Application.Contracts;
using Application.Contracts.ServicesContracts;
using Application.DataTransferObjects;
using Domain.Entities.AuthDto;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/authentication")]
[ApiController]
public class AuthenticationController : Controller
{
    private readonly IAuthService _authService;

    public AuthenticationController(IAuthService authService)
    {
        _authService = authService;
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
        var result = await _authService.RegisterUserAsync(userForRegistration);
        return result.Succeeded 
            ? StatusCode(201) 
            : BadRequest(result.Errors);
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Authenticate([FromBody] UserForAuthenticationDto userForLogin)
    {
        if (userForLogin == null || !ModelState.IsValid)
        {
            return BadRequest("Invalid login request");
        }
        var (accessToken, refreshToken) = await _authService.AuthenticateUserAsync(userForLogin);
        if (accessToken == null || refreshToken == null)
        {
            return Unauthorized();
        }
        return Ok(new { AccessToken = accessToken, RefreshToken = refreshToken });
    }
}
