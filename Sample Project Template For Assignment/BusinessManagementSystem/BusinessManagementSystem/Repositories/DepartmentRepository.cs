using BusinessManagementSystem.Data;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;

namespace BusinessManagementSystem.Repositories
{
    public class DepartmentRepository : GenericRepository<Department>,IDepartment
    {
        public DepartmentRepository(ApplicationDBContext context) : base(context)
        {

        }
    }
}
