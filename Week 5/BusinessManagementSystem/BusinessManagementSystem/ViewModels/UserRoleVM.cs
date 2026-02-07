using BusinessManagementSystem.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BusinessManagementSystem.ViewModels
{
    public class UserRoleVM
    {
        public IEnumerable<SelectListItem> Users { get; set; }
        public int UserId { get; set; }
        public string CurrentRole { get; set; }
        public IEnumerable<SelectListItem> Roles { get; set; }
        public int RoleId { get; set; }
    }
}
