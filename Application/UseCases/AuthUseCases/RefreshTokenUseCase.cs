using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.Contracts.UseCasesContracts.AuthUseCasesContracts;
using Application.Validation.CustomExceptions;
using Domain.Contracts;
using Domain.Entities.AuthDto;
using Domain.Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Application.UseCases.AuthUseCases;

public class RefreshTokenUseCase : IRefreshTokenUseCase
{
    private readonly UserManager<User> _userManager;
    private readonly IAuthenticationManager _authManager;
    private readonly IConfiguration _configuration;

    public RefreshTokenUseCase(
        UserManager<User> userManager,
        IAuthenticationManager authManager, 
        IConfiguration configuration)
    {
        _userManager = userManager;
        _authManager = authManager;
        _configuration = configuration;
    }

    public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings.GetSection("validIssuer").Value;
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new
                SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!)),
            ValidateLifetime = false,
            ValidIssuer = jwtSettings.GetSection("validIssuer").Value,
            ValidAudience = jwtSettings.GetSection("validAudience").Value,
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        var jwtSecurityToken = securityToken as JwtSecurityToken;

        if (jwtSecurityToken is null || !jwtSecurityToken.Header.Alg
                .Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            throw new SecurityTokenException("Invalid Token");
        }
        return principal;
    }
    
    public async Task<string> ExecuteAsync(TokenDto tokenDto)
    {
        var principal = GetPrincipalFromExpiredToken(tokenDto.AccessToken);
        var user = await _userManager.FindByNameAsync(principal.Identity.Name);
        if (user is null || user.RefreshToken != tokenDto.RefreshToken ||
            user.RefreshTokenExpireTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("Invalid refresh token or token expired.");
        }
        return await _authManager.CreateAccessToken(user);
    }
}
