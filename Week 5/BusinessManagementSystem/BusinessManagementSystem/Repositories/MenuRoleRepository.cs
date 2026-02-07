using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;

namespace BusinessManagementSystem.Repositories
{
    public class MenuRoleRepository:GenericRepository<MenuRole>, IMenuRole
    {
        private readonly ApplicationDBContext _db;
        public ResponseDto<MenuRole> _responseDto;
        public MenuRoleRepository(ApplicationDBContext db) : base(db)
        {
            _responseDto = new ResponseDto<MenuRole>();
            _db = db;
        }

        public dynamic GetRolesAssignedToMenu(int id)
        {
            var roles = (from m in _db.Menus
                         join mr in _db.MenuRoles on m.Id equals mr.MenuId
                         join r in _db.Roles on mr.RoleId equals r.Id
                         where m.Id == id
                         select new { Id = r.Id, Name = r.Name }).ToList();
            return roles;
        }
    }
}
