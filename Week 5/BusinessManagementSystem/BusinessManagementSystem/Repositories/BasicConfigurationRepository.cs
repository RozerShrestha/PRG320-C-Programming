using Azure;
using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BusinessManagementSystem.Repositories
{
    public class BasicConfigurationRepository: GenericRepository<BasicConfiguration>, IBasicConfiguration
    {
        private readonly ApplicationDBContext _db;
        public ResponseDto<BasicConfiguration> _responseDto;
        public BasicConfigurationRepository(ApplicationDBContext db) : base(db) 
        {
            _responseDto = new ResponseDto<BasicConfiguration>();
            _db = db; 
        }
        public ResponseDto<BasicConfiguration> UpdateBasicConfigurationDetail(BasicConfiguration basicConfiguration)
        {
            try
            {
                var item = _db.BasicConfigurations.FirstOrDefault(x => x.Id == basicConfiguration.Id);
                _db.Entry(item).CurrentValues.SetValues(basicConfiguration);
                _db.Entry(item).State = EntityState.Modified;
                _db.SaveChanges();
                _responseDto.Data = basicConfiguration;
            }
            catch (Exception ex)
            {
                _responseDto.Message = "Failed due to: " + ex.Message;
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
                _responseDto.Data = basicConfiguration;
            }
            return _responseDto;
        }
    }
}
