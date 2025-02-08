using SisandBackend.Controllers.UserController.Contracts;
using SisandBackend.Entities.Users;

namespace SisandBackend.EntityFrameworkCore.Repositories.UserRepository;

public interface IUserRepository
{
    Task<User> CreateAsync(UserInputDto user);
    Task<User> UpdateAsync(long userId, UserInputDto user);
    Task<(int, List<User>)> GetFilteredListAsync(UserFilterPaginatedDto userFilterPaginatedDto);
    Task<User> GetAsync(long userId);
    Task<bool> SoftDeleteAsync(long userId);
}
