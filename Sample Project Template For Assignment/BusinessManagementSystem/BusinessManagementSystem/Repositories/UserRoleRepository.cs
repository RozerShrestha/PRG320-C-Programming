using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;

namespace BusinessManagementSystem.Repositories
{
    public class UserRoleRepository : GenericRepository<UserRole>, IUserRole
    {
        private readonly ApplicationDBContext _db;
        private readonly ILogger<UserRoleRepository> _logger;
        private ResponseDto<UserRole> _responseDto;

        public UserRoleRepository(ApplicationDBContext db, ILogger<UserRoleRepository> logger) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _responseDto = new ResponseDto<UserRole>();
        }

        // Additional user role-specific methods can be added here
        // Currently inherits all CRUD operations from GenericRepository
    }
}
