using Azure;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.Services
{
    public interface IBasicConfiguration : IGeneric<BasicConfiguration>
    {
        ResponseDto<BasicConfiguration> UpdateBasicConfigurationDetail(BasicConfiguration basicConfiguration);
    }
}
