using Application.Contracts;
using Application.Contracts.ServicesContracts;
using Domain.Entities.AuthDto;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/token")]
[ApiController]
public class TokenController : Controller
{
    private readonly IAuthService _authService;

    public TokenController(IAuthService authService) => _authService = authService;

    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        string accessToken = await _authService.RefreshToken(tokenDto);
        return Ok(accessToken);
    }
}
