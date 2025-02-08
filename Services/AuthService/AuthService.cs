using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SisandBackend.Controllers.AuthController.Contracts;
using SisandBackend.Entities.Users;
using SisandBackend.Shared.Consts;
using SisandBackend.Shared.Utils;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SisandBackend.Services.AuthService;

public class AuthService : IAuthService
{
    #region Properties
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    #endregion

    #region Constructors
    public AuthService(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }
    #endregion

    #region Methods
    public async Task<AuthOutputDto> LoginAsync(AuthInputDto authInputDto)
    {
        var user = await _userManager.FindByNameAsync(authInputDto.UserName) ?? throw new(ApiRouteConstants.USER_GET_ERROR);
        var result = await _signInManager.CheckPasswordSignInAsync(user, authInputDto.Password, false);

        if (result.Succeeded)
        {
            var accessToken = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            RefreshTokenManager.AddOrUpdateRefreshToken(refreshToken, user);

            return new AuthOutputDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                ExpiresIn = 3600
            };
        }
        throw new(ApiRouteConstants.REFRESH_TOKEN_ERROR);
    }

    public AuthOutputDto Refresh(string refreshToken)
    {
        if (RefreshTokenManager.TryGetRefreshToken(refreshToken, out var user))
        {
            var newAccessToken = GenerateJwtToken(user);
            var newRefreshToken = GenerateRefreshToken();

            RefreshTokenManager.RemoveRefreshToken(refreshToken);
            RefreshTokenManager.AddOrUpdateRefreshToken(newRefreshToken, user);

            return new AuthOutputDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken,
                ExpiresIn = 3600
            };
        }
        throw new(ApiRouteConstants.REFRESH_TOKEN_ERROR);
    }
    #endregion

    #region Private Methods
    private string GenerateJwtToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Iss, _configuration["Jwt:Issuer"]),
                new Claim(JwtRegisteredClaimNames.Aud, _configuration["Jwt:Audience"])
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    #endregion
}
