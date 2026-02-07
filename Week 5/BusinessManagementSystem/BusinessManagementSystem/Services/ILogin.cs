using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using System.Diagnostics.Metrics;

namespace BusinessManagementSystem.Services
{
    public interface ILogin<T>
    {
        ResponseDto<T> Login(LoginRequestDto l);
        User GetUser(LoginRequestDto l);
        ResponseDto<T> Register_User(UserDto userDto);
        bool IsEmailAvailable(string Email);
        bool IsPhoneNumberAvailable(string PhoneNumber);

        ResponseDto<T> ForgotPassword(LoginRequestDto l);
    }
}
