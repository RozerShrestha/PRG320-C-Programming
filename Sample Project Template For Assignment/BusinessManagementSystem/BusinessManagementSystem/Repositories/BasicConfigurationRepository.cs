using Azure;
using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BusinessManagementSystem.Repositories
{
    public class BasicConfigurationRepository : GenericRepository<BasicConfiguration>, IBasicConfiguration
    {
        private readonly ApplicationDBContext _db;
        private readonly ILogger<BasicConfigurationRepository> _logger;
        private ResponseDto<BasicConfiguration> _responseDto;

        public BasicConfigurationRepository(ApplicationDBContext db, ILogger<BasicConfigurationRepository> logger) : base(db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _responseDto = new ResponseDto<BasicConfiguration>();
        }

        public ResponseDto<BasicConfiguration> UpdateBasicConfigurationDetail(BasicConfiguration basicConfiguration)
        {
            try
            {
                if (basicConfiguration == null)
                {
                    _responseDto.StatusCode = HttpStatusCode.BadRequest;
                    _responseDto.Message = "Configuration data is required";
                    return _responseDto;
                }

                var item = _db.BasicConfigurations.FirstOrDefault(x => x.Id == basicConfiguration.Id);
                
                if (item == null)
                {
                    _responseDto.StatusCode = HttpStatusCode.NotFound;
                    _responseDto.Message = "Configuration not found";
                    _logger.LogWarning($"Configuration with ID {basicConfiguration.Id} not found");
                    return _responseDto;
                }

                _db.Entry(item).CurrentValues.SetValues(basicConfiguration);
                _db.Entry(item).State = EntityState.Modified;
                _db.SaveChanges();

                _responseDto.Data = basicConfiguration;
                _responseDto.StatusCode = HttpStatusCode.OK;
                _responseDto.Message = "Configuration updated successfully";
                _logger.LogInformation("Basic configuration updated successfully");
            }
            catch (Exception ex)
            {
                _responseDto.Message = $"Failed to update configuration: {ex.Message}";
                if (ex.InnerException != null)
                    _responseDto.Message += $" Inner: {ex.InnerException.Message}";
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
                _responseDto.Data = basicConfiguration;
                _logger.LogError($"Error updating configuration: {ex.Message}");
            }
            return _responseDto;
        }
    }
}
