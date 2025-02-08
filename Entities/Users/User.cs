using Microsoft.AspNetCore.Identity;
using SisandBackend.Controllers.UserController.Contracts;

namespace SisandBackend.Entities.Users;

public class User : IdentityUser<long>
{
    #region Properties
    public bool IsDeleted { get; set; } = false;
    #endregion

    #region Methods

    public void UpdateUser(UserInputDto userInputDto)
    {
        UserName = userInputDto.UserName;
        Email = userInputDto.Email;
    }

    public void SoftDelete() => IsDeleted = true;
    #endregion
}
