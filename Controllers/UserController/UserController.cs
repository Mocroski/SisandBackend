using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SisandBackend.Controllers.UserController.Contracts;
using SisandBackend.EntityFrameworkCore.Repositories.UserRepository;
using SisandBackend.Shared.Consts;
using SisandBackend.Shared.Utils;

namespace SisandBackend.Controllers.UserController;

[ApiController]
[Authorize]
public class UserController : ControllerBase
{
    #region Consts
    private const string USERS_ROUTE = ApiRouteConstants.API_BASE_ROUTE + "users";
    #endregion

    #region Properties
    private readonly IMapper _mapper;
    private readonly IUserRepository _userRepository;
    #endregion

    #region Constructors
    public UserController(IMapper mapper, IUserRepository userRepository)
    {
        _mapper = mapper;
        _userRepository = userRepository;
    }

    #endregion

    #region Methods

    [HttpPost(USERS_ROUTE)]
    public async Task<ResponseBase<UserOutputDto>> InsertAsync([FromBody] UserInputDto input)
    {
        try
        {
            return new(ApiRouteConstants.USER_INSERT_OK, _mapper.Map<UserOutputDto>(await _userRepository.CreateAsync(input)));
        }
        catch (Exception ex)
        {
            throw new Exception(ApiRouteConstants.USER_INSERT_ERROR, ex);
        }
    }

    [HttpGet(USERS_ROUTE)]
    public async Task<ResponseBase<List<UserOutputDto>>> GetListAsync([FromQuery] UserFilterPaginatedDto userFilterPaginatedDto)
    {
        try
        {
            var (total, users) = await _userRepository.GetFilteredListAsync(userFilterPaginatedDto);
            return new(ApiRouteConstants.USER_GET_LIST_OK, _mapper.Map<List<UserOutputDto>>(users), total);
        }
        catch (Exception ex)
        {
            throw new Exception(ApiRouteConstants.USER_GET_LIST_ERROR, ex);
        }
    }

    [HttpGet(USERS_ROUTE + "/{userId}")]
    public async Task<ResponseBase<UserOutputDto>> GetByIdAsync(long userId)
    {
        try
        {
            return new(ApiRouteConstants.USER_GET_OK, _mapper.Map<UserOutputDto>(await _userRepository.GetAsync(userId)));
        }
        catch (Exception ex)
        {

            throw new Exception(ApiRouteConstants.USER_GET_ERROR, ex);
        }
    }

    [HttpPut(USERS_ROUTE + "/{userId}")]
    public async Task<ResponseBase<UserOutputDto>> UpdateAsync(long userId, UserInputDto userInputDto)
    {
        try
        {
            return new(ApiRouteConstants.USER_UPDATE_OK, _mapper.Map<UserOutputDto>(await _userRepository.UpdateAsync(userId, userInputDto)));
        }
        catch (Exception ex)
        {
            throw new Exception(ApiRouteConstants.USER_UPDATE_ERROR, ex);
        }
    }

    [HttpDelete((USERS_ROUTE + "/{userId}"))]
    public async Task<ResponseBase<bool>> DeleteAsync(long userId)
    {
        try
        {
            return new(ApiRouteConstants.USER_DELETE_OK, await _userRepository.SoftDeleteAsync(userId));
        }
        catch (Exception ex)
        {
            throw new Exception(ApiRouteConstants.USER_DELETE_ERROR, ex);
        }
    }
    #endregion
}
