using Application.Contracts.UseCasesContracts.AuthUseCasesContracts;
using Application.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Route("api/token")]
[ApiController]
public class TokenController : Controller
{
    private readonly IRefreshTokenUseCase _refreshTokenUseCase;

    public TokenController(IRefreshTokenUseCase refreshTokenUseCase)
    {
        _refreshTokenUseCase = refreshTokenUseCase;
    }
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] TokenDto tokenDto)
    {
        string accessToken = await _refreshTokenUseCase.ExecuteAsync(tokenDto);
        return Ok(accessToken);
    }
}
