using Microsoft.AspNetCore.Mvc;
using SisandBackend.Controllers.AuthController.Contracts;
using SisandBackend.Services.AuthService;
using SisandBackend.Shared.Consts;
using SisandBackend.Shared.Utils;

namespace SisandBackend.Controllers.AuthController;

[ApiController]
public class AuthController : ControllerBase
{
    #region Constants
    private const string LOGIN_ROUTE = ApiRouteConstants.API_BASE_ROUTE + "login";
    private const string REFRESH_ROUTE = ApiRouteConstants.API_BASE_ROUTE + "refresh";
    #endregion

    #region Properties
    private readonly IAuthService _authService;

    #endregion

    #region Constructors
    public AuthController(IAuthService authService) => _authService = authService;

    #endregion

    #region Methods

    [HttpPost(LOGIN_ROUTE)]
    public async Task<ResponseBase<AuthOutputDto>> LoginAsync([FromBody] AuthInputDto authInputDto)
    {
        try
        {
            return new(ApiRouteConstants.LOGIN_SUCCESS, await _authService.LoginAsync(authInputDto));
        }
        catch (Exception ex)
        {
            throw new(ApiRouteConstants.LOGIN_ERROR, ex);
        }
    }

    [HttpPost(REFRESH_ROUTE)]
    public ResponseBase<AuthOutputDto> RefreshAsync([FromBody] AuthRefreshTokenInputDto authRefreshTokenInputDto)
    {
        try
        {
            return new(ApiRouteConstants.LOGIN_SUCCESS, _authService.Refresh(authRefreshTokenInputDto.RefreshToken));
        }
        catch (Exception ex)
        {
            throw new(ApiRouteConstants.LOGIN_ERROR, ex);
        }
    }

    #endregion
}
