using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.Services
{
    public interface IMenuRole : IGeneric<MenuRole>
    {
        dynamic GetRolesAssignedToMenu(int id);
    }
}
