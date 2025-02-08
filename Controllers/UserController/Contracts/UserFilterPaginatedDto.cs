using SisandBackend.Shared.Utils;

namespace SisandBackend.Controllers.UserController.Contracts;

public class UserFilterPaginatedDto : PagedInput
{
    public string Filter {  get; set; } = string.Empty;
}
