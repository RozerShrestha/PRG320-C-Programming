using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using System.Net;

namespace BusinessManagementSystem.Repositories
{
    public class MenuRoleRepository : GenericRepository<MenuRole>, IMenuRole
    {
        private readonly ApplicationDBContext _db;
        private readonly ILogger<MenuRoleRepository> _logger;
        private ResponseDto<MenuRole> _responseDto;

        public MenuRoleRepository(ApplicationDBContext db, ILogger<MenuRoleRepository> logger) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _responseDto = new ResponseDto<MenuRole>();
        }

        public dynamic GetRolesAssignedToMenu(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning($"Invalid menu ID: {id}");
                    return new List<dynamic>();
                }

                var roles = (from m in _db.Menus
                             join mr in _db.MenuRoles on m.Id equals mr.MenuId
                             join r in _db.Roles on mr.RoleId equals r.Id
                             where m.Id == id
                             select new { Id = r.Id, Name = r.Name }).ToList();

                if (roles == null || roles.Count == 0)
                {
                    _logger.LogWarning($"No roles found for menu ID: {id}");
                    return new List<dynamic>();
                }

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving roles for menu {id}: {ex.Message}");
                return new List<dynamic>();
            }
        }
    }
}
