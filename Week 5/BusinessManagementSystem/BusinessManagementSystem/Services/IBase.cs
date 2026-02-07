using BusinessManagementSystem.Dto;

namespace BusinessManagementSystem.Services
{
    public interface IBase
    {
        UserDto UserDetail(string userName);
        List<MenuDto> MenuList(string roleName);

        dynamic RoleList();
        
    }
}
