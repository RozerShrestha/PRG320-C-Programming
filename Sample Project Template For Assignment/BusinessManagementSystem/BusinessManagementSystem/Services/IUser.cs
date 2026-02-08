using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.Services
{
    public interface IUser:IGeneric<User>
    {
        List<User> GetAllActiveUsers();
        List<User> GetAllInactiveUsers();
        ResponseDto<UserRoleDto> GetAllUser(string filter);
        //ResponseDto<UserDto> GetUser(Guid guid);
        dynamic RoleList();

    }
}
