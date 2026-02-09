using BusinessManagementSystem.Data;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;

namespace BusinessManagementSystem.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>, IDepartment
    {
        private readonly ILogger<DepartmentRepository> _logger;

        public DepartmentRepository(ApplicationDBContext context, ILogger<DepartmentRepository> logger) : base(context)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // Additional department-specific methods can be added here
        // Currently inherits all CRUD operations from GenericRepository
    }
}
