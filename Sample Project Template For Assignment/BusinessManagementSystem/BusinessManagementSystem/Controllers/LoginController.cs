using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Services;
using BusinessManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BusinessManagementSystem.Controllers
{
    [AllowAnonymous]
    public class LoginController : Controller
    {
        ILogin<LoginResponseDto> _iLogin;
        ResponseDto<LoginResponseDto> _responseDto;
        protected readonly INotyfService _notyf;
        public LoginController(ILogin<LoginResponseDto> iLogin, INotyfService notyf) 
        { 
            _iLogin = iLogin;
            _responseDto= new ResponseDto<LoginResponseDto>();
            _notyf = notyf;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginUser(LoginRequestDto loginRequest)
        {
            ModelState.Remove(nameof(loginRequest.ConfirmPassword)); //just to ignore ConfirmPassword to validate
            if (ModelState.IsValid)
            {
                _responseDto = _iLogin.Login(loginRequest);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    HttpContext.Session.SetString("Token", _responseDto.Data.Token);
                    ViewBag.Message = _responseDto.Message;
                    _notyf.Success(_responseDto.Message);
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    ModelState.AddModelError("", _responseDto.Message);
                }
                ViewBag.LoginResponse = _responseDto;
            }
            
            return View("Index",loginRequest); ;
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterUser(UserDto userDto)
        {
            if (ModelState.IsValid)
            {
                bool passwordMatch = userDto.Password == userDto.ConfirmPassword ? true : false;
               _responseDto = _iLogin.Register_User(userDto);
                if(_responseDto.StatusCode!= HttpStatusCode.OK) 
                {
                    _notyf.Error(_responseDto.Message);
                    ViewBag.RegisterResponse = _responseDto;
                }
                else
                {
                    _notyf.Success(_responseDto.Message);
                    ViewBag.LoginResponse = _responseDto;
                    return View("Index");
                }
                
            }
            return View("Register");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(LoginRequestDto loginRequestDto)
        {
            if (ModelState.IsValid)
            {
                _responseDto = _iLogin.ForgotPassword(loginRequestDto);
                if (_responseDto.StatusCode != HttpStatusCode.OK)
                {
                    _notyf.Error(_responseDto.Message);
                    ViewBag.RegisterResponse = _responseDto;
                }
                else
                {
                    _notyf.Success(_responseDto.Message);
                    ViewBag.LoginResponse = _responseDto;
                }
            }
                return View("Index");
        }
        public IActionResult Logout([FromQuery] string returnUrl)
        {
            HttpContext.Session.Remove("Token");
            return RedirectToAction("Index");
        }


        #region API
        [HttpPost]
        public IActionResult LoginUserAPI(LoginRequestDto loginRequest)
        {
            ModelState.Remove(nameof(loginRequest.ConfirmPassword)); //just to ignore ConfirmPassword to validate
            if (ModelState.IsValid)
            {
                _responseDto = _iLogin.Login(loginRequest);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    HttpContext.Session.SetString("Token", _responseDto.Data.Token);
                    _notyf.Success(_responseDto.Message);
                    return Ok(_responseDto);
                }
                else
                {
                    ModelState.AddModelError("", _responseDto.Message);
                    return BadRequest(_responseDto);
                }
            }
            else
            {
                return BadRequest(_responseDto);
            }

        }

        #endregion
    }
}
