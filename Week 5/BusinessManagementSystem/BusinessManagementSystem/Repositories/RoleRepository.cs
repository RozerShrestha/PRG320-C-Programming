using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace BusinessManagementSystem.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRole
    {
        private readonly ApplicationDBContext _db;
        public RoleRepository(ApplicationDBContext db) : base(db)
        {
            _db = db;
        }

        public dynamic GetRoles()
        {
            var roles = _db.Roles.Select(p => new { p.Id, p.Name }).ToList();
            return roles;
        }
        
    }

}
