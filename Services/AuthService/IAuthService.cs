using SisandBackend.Controllers.AuthController.Contracts;

namespace SisandBackend.Services.AuthService;

public interface IAuthService
{
    Task<AuthOutputDto> LoginAsync(AuthInputDto authInputDto);
    AuthOutputDto Refresh(string refreshToken);
}
