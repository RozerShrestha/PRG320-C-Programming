using StudentPortal.Web.Models;

namespace StudentPortal.Web.Services.Dto
{
    public class UserRoleDto
    {
        public User User { get; set; }
        public string RoleName { get; set; }
    }
}
