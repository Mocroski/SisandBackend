using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SisandBackend.Controllers.UserController.Contracts;
using SisandBackend.Entities.Users;
using SisandBackend.EntityFrameworkCore.Contexts;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using SisandBackend.Shared.Consts;

namespace SisandBackend.EntityFrameworkCore.Repositories.UserRepository;

public class UserRepository : IUserRepository
{
    #region Properties

    private readonly SisandDbContext _context;
    private readonly UserManager<User> _userManager;

    #endregion

    #region Constructors
    public UserRepository(SisandDbContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }
    #endregion

    #region Methods

    public async Task<User> CreateAsync(UserInputDto input)
    {
        var user = new User
        {
            UserName = input.UserName,
            Email = input.Email,
        };

        var result = await _userManager.CreateAsync(user, input.Password);

        if (!result.Succeeded)
            throw new Exception($"{ApiRouteConstants.USER_INSERT_ERROR}; Erro: {result.Errors.Select(e => e.Description)}");

        return user;
    }

    public async Task<bool> SoftDeleteAsync(long userId)
    {
        var user = await _context.Users.FirstAsync(ent => ent.Id == userId) ?? throw new Exception($"{ApiRouteConstants.USER_GET_ERROR}, Usuário com Id: {userId} não encontrado.");
        user.SoftDelete();
        _context.Users.Update(user);
        return await _context.SaveChangesAsync() > 0;
    }

    public async Task<User> GetAsync(long userId) 
        => await _context.Users.FirstAsync(ent => ent.Id == userId) ?? throw new Exception($"{ApiRouteConstants.USER_GET_ERROR}, Usuário com Id: {userId} não encontrado.");

    public async Task<(int, List<User>)> GetFilteredListAsync(UserFilterPaginatedDto userPagedInput)
    {
        var query = _userManager.Users.AsQueryable();

        if (!string.IsNullOrWhiteSpace(userPagedInput.Filter))
            query = query.Where(UserFilterByAnything(userPagedInput.Filter));

        if (!string.IsNullOrWhiteSpace(userPagedInput.Sort))
            query = query.OrderBy(userPagedInput.Sort);

        var users = await query.Skip(userPagedInput.Offset ?? 0).Take(userPagedInput.Limit ?? 10).AsNoTracking().ToListAsync();

        return (users.Count, users);
    }

    public async Task<User> UpdateAsync(long userId, UserInputDto userInputDto)
    {
        var user = await _context.Users.FirstAsync(ent => ent.Id == userId) ?? throw new Exception($"{ApiRouteConstants.USER_GET_ERROR}, Usuário com Id: {userId} não encontrado.");
        user.UpdateUser(userInputDto);
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return user;
    }
    #endregion

    #region Private methods

    private static Expression<Func<User, bool>> UserFilterByAnything(string filter)
        => (user) =>
                    EF.Functions.Like(user.UserName, $"%{filter}%") ||
                    EF.Functions.Like(user.Email, $"%{filter}%");
    #endregion
}