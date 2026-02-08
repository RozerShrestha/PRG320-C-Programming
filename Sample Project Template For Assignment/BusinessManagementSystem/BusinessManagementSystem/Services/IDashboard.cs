using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.ViewModels;

namespace BusinessManagementSystem.Services
{
    public interface IDashboard
    {
        ResponseDto<DashboardVM> GetDashboardInfo(int roleId, string username);
    }
}