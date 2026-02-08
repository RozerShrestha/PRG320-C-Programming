using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.Services
{
    public interface IMenu : IGeneric<Menu>
    {
        dynamic ParentList();
        Multiselect RoleList();
        ResponseDto<Menu> GetMenuById(int id);
        ResponseDto<Menu> CreateMenu(Menu menu);
        ResponseDto<Menu> UpdateMenu(Menu menu);
    }
}
