using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.Services
{
    public interface IRole : IGeneric<Role>
    {
        dynamic GetRoles();
    }
}
