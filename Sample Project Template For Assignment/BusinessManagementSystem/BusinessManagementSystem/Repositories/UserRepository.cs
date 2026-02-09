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
            _responseDtoUser = new ResponseDto<UserDto>();
        }

        public List<User> GetAllActiveUsers()
        {
            return _dbContext.Users.Where(p => p.Status).ToList();
        }

        public List<User> GetAllInactiveUsers()
        {
            return _dbContext.Users.Where(p => !p.Status).ToList();
        }

        public ResponseDto<UserRoleDto> GetAllUser(string filter)
        {
            try
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

                if (_responseDtoUserRole.Datas.Any())
                {
                    _responseDtoUserRole.StatusCode = HttpStatusCode.OK;
                }
                else
                {
                    _responseDtoUserRole.StatusCode = HttpStatusCode.NotFound;
                    _responseDtoUserRole.Message = "No users found";
                }
            }
            catch (Exception ex)
            {
                _responseDtoUserRole.StatusCode = HttpStatusCode.InternalServerError;
                _responseDtoUserRole.Message = $"Error retrieving users: {ex.Message}";
            }

            return _responseDtoUserRole;
        }

        public ResponseDto<UserDto> GetUserByGuid(Guid guid)
        {
            try
            {
                var item = _dbContext.Users
                    .Include(m => m.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefault(p => p.Guid == guid);

                if (item == null)
                {
                    _responseDtoUser.StatusCode = HttpStatusCode.NotFound;
                    _responseDtoUser.Message = "User not found";
                    return _responseDtoUser;
                }

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
                _responseDtoUser.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _responseDtoUser.StatusCode = HttpStatusCode.InternalServerError;
                _responseDtoUser.Message = $"Error retrieving user: {ex.Message}";
            }

            return _responseDtoUser;
        }

        public ResponseDto<UserDto> GetUserById(int id)
        {
            try
            {
                var item = _dbContext.Users
                    .Include(m => m.UserRoles)
                    .ThenInclude(ur => ur.Role)
                    .FirstOrDefault(p => p.Id == id);

                if (item == null)
                {
                    _responseDtoUser.StatusCode = HttpStatusCode.NotFound;
                    _responseDtoUser.Message = "User not found";
                    return _responseDtoUser;
                }

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
                _responseDtoUser.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                _responseDtoUser.StatusCode = HttpStatusCode.InternalServerError;
                _responseDtoUser.Message = $"Error retrieving user: {ex.Message}";
            }

            return _responseDtoUser;
        }

        public ResponseDto<User> CreateUser(UserDto userDto)
        {
            try
            {
                // Validate input
                if (userDto == null)
                {
                    _responseDto.StatusCode = HttpStatusCode.BadRequest;
                    _responseDto.Message = "User data is required";
                    return _responseDto;
                }

                // Check if user already exists
                var existingUser = _dbContext.Users.FirstOrDefault(u => u.Email == userDto.Email);
                if (existingUser != null)
                {
                    _responseDto.StatusCode = HttpStatusCode.Conflict;
                    _responseDto.Message = "User with this email already exists";
                    return _responseDto;
                }

                // Generate salt and hash password
                var hashInfo = Helper.Helpers.GetHashPassword(userDto.Password);

                // Map UserDto to User entity
                var user = new User
                {
                    Guid = Helper.Helpers.GenerateGUID(),
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
                    FirstPasswordReset = false
                };

                // Add user to database
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();

                // Create user role relationship
                var userRole = new UserRole
                {
                    UserId = user.Id,
                    RoleId = userDto.RoleId
                };
                _dbContext.UserRoles.Add(userRole);
                _dbContext.SaveChanges();

                _responseDto.StatusCode = HttpStatusCode.OK;
                _responseDto.Message = "User created successfully";
                _responseDto.Data = user;

                return _responseDto;
            }
            catch (Exception ex)
            {
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
                _responseDto.Message = $"Error creating user: {ex.Message}";
                return _responseDto;
            }
        }

        public ResponseDto<User> UpdateUser(UserDto userDto)
        {
            try
            {
                var item = _dbContext.Users
                    .Include(u => u.UserRoles)
                    .AsTracking()
                    .FirstOrDefault(p => p.Id == userDto.UserId);

                if (item == null)
                {
                    _responseDto.StatusCode = HttpStatusCode.NotFound;
                    _responseDto.Message = "User not found";
                    return _responseDto;
                }

                // Update user properties
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

                // Update user role
                var userRole = item.UserRoles.FirstOrDefault(p => p.UserId == userDto.UserId);
                if (userRole != null)
                {
                    _dbContext.UserRoles.Remove(userRole);
                    _dbContext.SaveChanges();
                }

                var newUserRole = new UserRole 
                { 
                    UserId = item.Id,
                    RoleId = userDto.RoleId 
                };
                _dbContext.UserRoles.Add(newUserRole);

                _dbContext.Users.Update(item);
                _dbContext.SaveChanges();

                _responseDto.StatusCode = HttpStatusCode.OK;
                _responseDto.Message = "User updated successfully";
                _responseDto.Data = item;
            }
            catch (Exception ex)
            {
                _responseDto.StatusCode = HttpStatusCode.InternalServerError;
                _responseDto.Message = $"Error updating user: {ex.Message}";
            }

            return _responseDto;
        }

        public dynamic RoleList()
        {
            return _dbContext.Roles.Select(p => new { Id = p.Id, Name = p.Name }).ToList();
        }
    }
}
