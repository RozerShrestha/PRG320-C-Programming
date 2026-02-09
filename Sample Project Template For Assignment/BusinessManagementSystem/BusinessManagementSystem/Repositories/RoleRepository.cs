using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using System.Net;

namespace BusinessManagementSystem.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRole
    {
        private readonly ApplicationDBContext _db;
        private readonly ILogger<RoleRepository> _logger;

        public RoleRepository(ApplicationDBContext db, ILogger<RoleRepository> logger) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public dynamic GetRoles()
        {
            try
            {
                var roles = _db.Roles.Select(p => new { p.Id, p.Name }).ToList();
                
                if (roles == null || roles.Count == 0)
                {
                    _logger.LogWarning("No roles found in database");
                    return new List<dynamic>();
                }

                return roles;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving roles: {ex.Message}");
                return new List<dynamic>();
            }
        }
    }
}
