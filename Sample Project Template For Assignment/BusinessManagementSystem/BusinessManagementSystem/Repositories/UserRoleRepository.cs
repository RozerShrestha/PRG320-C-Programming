using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;

namespace BusinessManagementSystem.Repositories
{
    public class UserRoleRepository:GenericRepository<UserRole>, IUserRole
    {
        private readonly ApplicationDBContext _db;
        public ResponseDto<UserRole> _responseDto;
        public UserRoleRepository(ApplicationDBContext db) : base(db) 
        {
            _responseDto = new ResponseDto<UserRole>();
            _db = db;
        }


    }
}
