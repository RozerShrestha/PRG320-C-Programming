using AutoMapper;
using BusinessManagementSystem.Data;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Pages;
using BusinessManagementSystem.Services;
using BusinessManagementSystem.ViewModels;
using Microsoft.Identity.Client;
using System.IdentityModel.Tokens.Jwt;
using System.Net;

namespace BusinessManagementSystem.Repositories
{
    public class LoginRepository : ILogin<LoginResponseDto>
    {
        ApplicationDBContext _db;
        ResponseDto<LoginResponseDto> _responseDto;
        private readonly IMapper _mapper;
        LoginResponseDto _loginResponse;
        readonly IConfiguration _config;
        readonly TokenRepository _tokenRepository;
        JwtSecurityToken generatedToken = null;
        public LoginRepository(ApplicationDBContext db, IConfiguration config, IMapper mapper)
        {
            _db = db;
            _config = config;
            mapper = mapper;
            _tokenRepository = new TokenRepository();
            _responseDto = new ResponseDto<LoginResponseDto>();
            _loginResponse = new LoginResponseDto();

        }
        public ResponseDto<LoginResponseDto> Login(LoginRequestDto l)
        {
            string roleId = "";
            string roleName = "";
            try
            {
                var userData = GetUser(l);
                if (userData==null)
                {
                    _responseDto.StatusCode =HttpStatusCode.NotFound ;
                    _responseDto.Message = "Username Invalid";
                }
                else
                {
                    var hashPassword = Helper.Helpers.VerifyHashPassword(l.Password, userData.Salt);
                    var validUserInfo = (from m in _db.Users
                                         join mr in _db.UserRoles on m.Id equals mr.UserId
                                         join r in _db.Roles on mr.RoleId equals r.Id
                                         where (m.UserName == l.Username || m.Email==l.Username || m.PhoneNumber==l.Username) && m.HashPassword == hashPassword
                                         select new
                                         {
                                             m.UserName,
                                             m.Email,
                                             m.PhoneNumber,
                                             r.Id,
                                             RoleName = r.Name,
                                         }).ToList();
                    if (validUserInfo.Count == 0)
                    {
                        _responseDto.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                        _responseDto.Message = "Username or Password Invalid";
                        //_logger.LogInformation("Username or Password Invalid " + l.Username + ";" + l.Password);
                        return _responseDto;

                    }
                    foreach (var userInfo in validUserInfo)
                    {
                        roleId += userInfo.Id + "#";
                        roleName += userInfo.RoleName + "#";
                        
                    }
                    roleId = roleId.Remove(roleId.Length - 1, 1);
                    roleName = roleName.Remove(roleName.Length - 1, 1);
                    

                    _loginResponse.UserName = validUserInfo[0].UserName;
                    _loginResponse.Role = roleId;
                    _loginResponse.RoleDescription = roleName;
                    _loginResponse.Email= validUserInfo[0].Email;

                    generatedToken = _tokenRepository.BuildToken(_config["Jwt:Key"].ToString(), _config["Jwt:Issuer"].ToString(), _loginResponse);
                    if (generatedToken != null)
                    {
                        _loginResponse.Token = new JwtSecurityTokenHandler().WriteToken(generatedToken);
                        _loginResponse.TokenExpiry = generatedToken.ValidTo;
                    }
                    _responseDto.Message = "Login Successful";
                }
                _responseDto.Data = _loginResponse;
                return _responseDto;
            }
            catch (Exception ex)
            {
                _responseDto.StatusCode=HttpStatusCode.InternalServerError;
                _responseDto.Message = ex.Message;
                return _responseDto;
            }
        }
        public ResponseDto<LoginResponseDto> Register_User(UserDto userDto)
        {
            if(!IsEmailAvailable(userDto.Email) && !IsPhoneNumberAvailable(userDto.PhoneNumber))
            {
                try
                {
                    var hashInfo = Helper.Helpers.GetHashPassword(userDto.Password);
                    UserRole ur = new UserRole();
                    User u = new User()
                    {
                        Guid = Helper.Helpers.GenerateGUID(),
                        UserName = userDto.Email.Split("@")[0],
                        Email = userDto.Email,
                        HashPassword = hashInfo.Hash,
                        Salt = hashInfo.Salt,
                        FullName = userDto.FullName,
                        Address = userDto.Address,
                        DateOfBirth =DateOnly.Parse(userDto.DateOfBirth.ToString()),
                        PhoneNumber = userDto.PhoneNumber,
                        Gender=userDto.Gender,
                        CreatedBy = userDto.FullName,
                        UpdatedBy = userDto.FullName,
                        Status=true,
                        Occupation=userDto.Occupation
                    };
                    ur.User = u;
                    ur.RoleId = 2; //3 is role id for employee normal user
                    _db.Add(ur);
                    _db.SaveChanges();

                    _responseDto.Message = "Successfully Registered";
                    _responseDto.StatusCode = HttpStatusCode.OK;
                    _responseDto.Dynamic_Datas = userDto;
                }
                catch (Exception ex)
                {
                    _responseDto.Message = "Error due to:" + ex.InnerException;
                    _responseDto.StatusCode = HttpStatusCode.InternalServerError;
                }  
            }
            else
            {
                _responseDto.Message = "Email or Mobile Number already Exist";
                _responseDto.StatusCode = HttpStatusCode.Ambiguous;
            }
            return _responseDto;
        }
        public User GetUser(LoginRequestDto l)
        {
            var item = _db.Users.Where(p => p.UserName == l.Username || p.Email==l.Username || p.PhoneNumber==l.Username).FirstOrDefault();
            return item;
        }
        public  bool IsEmailAvailable(string Email)
        {
           return _db.Users.Where(p=>p.Email== Email).Any();  
        }
        public bool IsPhoneNumberAvailable(string PhoneNumber)
        {
            return _db.Users.Where(p=>p.PhoneNumber== PhoneNumber).Any();
        }
        public ResponseDto<LoginResponseDto> ForgotPassword(LoginRequestDto l)
        {
            bool passwordMatch = string.Equals(l.Password, l.ConfirmPassword);
            if(passwordMatch)
            {
                if (IsEmailAvailable(l.Username) || IsPhoneNumberAvailable(l.Username))
                {
                    var hashInfo = Helper.Helpers.GetHashPassword(l.Password);
                    var user = _db.Users.Where(p => p.Email == l.Username || p.PhoneNumber == l.Username).FirstOrDefault();
                    user.HashPassword = hashInfo.Hash;
                    user.Salt = hashInfo.Salt;
                    var result = _db.Users.Update(user);
                    _db.SaveChanges();
                    _responseDto.Message = "Password Update Successfully";
                }
                else
                {
                    _responseDto.StatusCode = HttpStatusCode.NotFound;
                    _responseDto.Message = "Email or Mobile Number did not match, Please try again";
                }
            }
            else
            {
                _responseDto.StatusCode = HttpStatusCode.BadRequest;
                _responseDto.Message = "Password and Confirm Password didn't match, Please try again";
            }
            
            return _responseDto;
        }
    }
}
