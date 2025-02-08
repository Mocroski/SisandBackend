using AutoMapper;
using SisandBackend.Controllers.UserController.Contracts;
using SisandBackend.Entities.Users;

namespace SisandBackend.Controllers;

public class ApiMapperProfile : Profile
{
    public ApiMapperProfile()
    {
        CreateMap<User, UserOutputDto>();
        CreateMap<UserInputDto, User>();
    }
}
