using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using BusinessManagementSystem.Utility;
using Humanizer;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BusinessManagementSystem.Repositories
{
    public class UserRepository : GenericRepository<User>, IUser
    {
        public ResponseDto<User> _responseDto;
        public ResponseDto<UserRoleDto> _responseDtoUserRole;
        public ResponseDto<UserDto> _responseDtoUser;
        public UserRepository(ApplicationDBContext dbContext) : base(dbContext) 
        {
            _responseDto = new ResponseDto<User>();
            _responseDtoUserRole = new ResponseDto<UserRoleDto>();
            _responseDtoUser=new ResponseDto<UserDto>();
        }

        public List<User> GetAllActiveUsers()
        {
            List<User> activeUsers = _dbContext.Users.Where(p => p.Status == true).ToList();
            return activeUsers;
        }

        public List<User> GetAllInactiveUsers()
        {
            List<User> activeUsers = _dbContext.Users.Where(p => p.Status == false).ToList();
            return activeUsers;
        }

        public ResponseDto<UserRoleDto> GetAllUser(string filter)
        {
            if (filter == SD.Role_Superadmin)
            {
                _responseDtoUserRole.Datas = (from u in _dbContext.Users
                                              join ur in _dbContext.UserRoles on u.Id equals ur.UserId
                                              join r in _dbContext.Roles on ur.RoleId equals r.Id
                                              select new UserRoleDto
                                              {
                                                  User = u,
                                                  RoleName = r.Name
                                              }).ToList();
            }
            else
            {
                _responseDtoUserRole.Datas = (from u in _dbContext.Users
                                              join ur in _dbContext.UserRoles on u.Id equals ur.UserId
                                              join r in _dbContext.Roles on ur.RoleId equals r.Id
                                              where r.Name == filter
                                              select new UserRoleDto
                                              {
                                                  User = u,
                                                  RoleName = r.Name
                                              }).ToList();
            }
           
            
            return _responseDtoUserRole;
        }

        public ResponseDto<UserDto> GetUserByGuid(Guid guid)
        {
            var item = _dbContext.Users.Include(m => m.UserRoles).Where(p => p.Guid == guid).SingleOrDefault();
            UserDto userDto = new UserDto
            {
                UserId = item.Id,
                UserName = item.UserName,
                Email = item.Email,
                FullName = item.FullName,
                DateOfBirth = item.DateOfBirth,
                Gender = item.Gender,
                Address = item.Address,
                PhoneNumber = item.PhoneNumber,
                Occupation = item.Occupation,
                Status = item.Status,
                FacebookLink = item.FacebookLink,
                InstagramLink = item.InstagramLink,
                TiktokLink = item.TiktokLink,
                ProfilePictureLink = item.ProfilePictureLink,
                Skills = item.Skills,
                Notes = item.Notes,
                RoleName = item.UserRoles?.FirstOrDefault()?.Role?.Name ?? string.Empty
            };
            _responseDtoUser.Data = userDto;
            return _responseDtoUser;


        }
        public ResponseDto<UserDto> GetUserById(int id)
        {
            var item = _dbContext.Users.Include(m => m.UserRoles).Where(p =>p.Id==id ).SingleOrDefault();
            UserDto userDto = new UserDto
            {
                UserId = item.Id,
                UserName = item.UserName,
                Email = item.Email,
                FullName = item.FullName,
                DateOfBirth = item.DateOfBirth,
                Gender = item.Gender,
                Address = item.Address,
                PhoneNumber = item.PhoneNumber,
                Occupation = item.Occupation,
                Status = item.Status,
                FacebookLink = item.FacebookLink,
                InstagramLink = item.InstagramLink,
                TiktokLink = item.TiktokLink,
                ProfilePictureLink = item.ProfilePictureLink,
                Skills = item.Skills,
                Notes = item.Notes,
                RoleName = item.UserRoles?.FirstOrDefault()?.Role?.Name ?? string.Empty
            };
            _responseDtoUser.Data = userDto;
            return _responseDtoUser;


        }

        public ResponseDto<User> CreateUser(UserDto userDto)
        {
            try
            {
                // Validate input
                if (userDto == null)
                {
                    _responseDto.StatusCode = System.Net.HttpStatusCode.BadRequest;
                    _responseDto.Message = "User data is required";
                    return _responseDto;
                }

                // Check if user already exists
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == userDto.Email);
                if (existingUser != null)
                {
                    _responseDto.StatusCode = System.Net.HttpStatusCode.Conflict;
                    _responseDto.Message = "User with this email already exists";
                    return _responseDto;
                }
                List<UserRole> urList = [new UserRole { RoleId = userDto.RoleId }];
                // Generate salt and hash password
                var hashInfo = Helper.Helpers.GetHashPassword(userDto.Password);
                // Map UserDto to User entity
                var user = new User
                {
                    Guid=Helper.Helpers.GenerateGUID(),
                    UserName = userDto.UserName,
                    Email = userDto.Email,
                    FullName = userDto.FullName,
                    DateOfBirth = userDto.DateOfBirth,
                    Gender = userDto.Gender,
                    Address = userDto.Address,
                    PhoneNumber = userDto.PhoneNumber,
                    Occupation = userDto.Occupation,
                    Status = userDto.Status,
                    FacebookLink = userDto.FacebookLink,
                    InstagramLink = userDto.InstagramLink,
                    TiktokLink = userDto.TiktokLink,
                    ProfilePictureLink = userDto.ProfilePictureLink,
                    Skills = userDto.Skills,
                    Notes = userDto.Notes,
                    HashPassword = hashInfo.Hash,
                    Salt = hashInfo.Salt,
                    FirstPasswordReset = false,
                    UserRoles= urList,
                };

                // Add user to database
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                _responseDto.StatusCode = System.Net.HttpStatusCode.OK;
                _responseDto.Message = "User created successfully";
                _responseDto.Data = user;

                return _responseDto;
            }
            catch (Exception ex)
            {
                _responseDto.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                _responseDto.Message = $"Error creating user: {ex.Message}";
                return _responseDto;
            }
        }

        public ResponseDto<User> UpdateUser(UserDto userDto)
        {
            var item = _dbContext.Users
                        .Include(u => u.UserRoles)
                        .Where(p => p.Id == userDto.UserId)
                        .AsTracking()
                        .FirstOrDefault();

            if (!string.IsNullOrEmpty(userDto.ProfilePictureLink))
            {
                item.ProfilePictureLink = userDto.ProfilePictureLink;
            }
            item.UserName = userDto.UserName;
            item.Email = userDto.Email;
            item.FullName = userDto.FullName;
            item.DateOfBirth = userDto.DateOfBirth;
            item.Address = userDto.Address;
            item.PhoneNumber = userDto.PhoneNumber;
            item.Gender = userDto.Gender;
            item.Occupation = userDto.Occupation;
            item.Status = userDto.Status;
            item.RoleId = userDto.RoleId;
            item.FacebookLink = userDto.FacebookLink;
            item.InstagramLink = userDto.InstagramLink;
            item.TiktokLink = userDto.TiktokLink;
            item.Skills = userDto.Skills;
            item.Notes = userDto.Notes;
            var userRole = item.UserRoles.SingleOrDefault(p => p.UserId == userDto.UserId);
            if (userRole != null)
            {
                var response = _dbContext.UserRoles.Remove(userRole);
                _dbContext.SaveChanges();
                List<UserRole> urList = [new UserRole { RoleId = userDto.RoleId }];
                item.UserRoles = urList;
            }
            _dbContext.Users.Update(item);
            _responseDto.StatusCode = System.Net.HttpStatusCode.OK;
            _responseDto.Message = "User updated successfully";
            return _responseDto;
        }

        public dynamic RoleList()
        {
            var roleLIst = _dbContext.Roles.Select(p => new { Id = p.Id, Name = p.Name }).ToList();
            return roleLIst;
        }
    }
}
