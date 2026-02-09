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
        private readonly ILogin<LoginResponseDto> _iLogin;
        private readonly INotyfService _notyf;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogin<LoginResponseDto> iLogin, INotyfService notyf, ILogger<LoginController> logger)
        {
            _iLogin = iLogin ?? throw new ArgumentNullException(nameof(iLogin));
            _notyf = notyf ?? throw new ArgumentNullException(nameof(notyf));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LoginUser(LoginRequestDto loginRequest)
        {
            try
            {
                ModelState.Remove(nameof(loginRequest.ConfirmPassword));
                
                if (ModelState.IsValid)
                {
                    var response = _iLogin.Login(loginRequest);
                    if (response.StatusCode == HttpStatusCode.OK && response.Data != null)
                    {
                        HttpContext.Session.SetString("Token", response.Data.Token);
                        _notyf.Success(response.Message ?? "Login successful");
                        _logger.LogInformation($"User {loginRequest.Username} logged in successfully");
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Message ?? "Login failed");
                        _logger.LogWarning($"Login failed for user {loginRequest.Username}");
                        ViewBag.LoginResponse = response;
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        _notyf.Error(error.ErrorMessage);
                    }
                }

                return View("Index", loginRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during login: {ex.Message}");
                _notyf.Error("An error occurred during login. Please try again.");
                return View("Index", loginRequest);
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterUser(UserDto userDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (userDto.Password != userDto.ConfirmPassword)
                    {
                        _notyf.Error("Passwords do not match");
                        return View("Register", userDto);
                    }

                    var response = _iLogin.Register_User(userDto);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        _notyf.Success(response.Message ?? "Registration successful");
                        _logger.LogInformation($"User {userDto.Email} registered successfully");
                        return View("Index");
                    }
                    else
                    {
                        _notyf.Error(response.Message ?? "Registration failed");
                        _logger.LogWarning($"Registration failed for {userDto.Email}: {response.Message}");
                        ViewBag.RegisterResponse = response;
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        _notyf.Error(error.ErrorMessage);
                    }
                }

                return View("Register", userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during registration: {ex.Message}");
                _notyf.Error("An error occurred during registration. Please try again.");
                return View("Register", userDto);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(LoginRequestDto loginRequestDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var response = _iLogin.ForgotPassword(loginRequestDto);
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        _notyf.Success(response.Message ?? "Password reset successful");
                        _logger.LogInformation($"Password reset requested for {loginRequestDto.Username}");
                        ViewBag.LoginResponse = response;
                    }
                    else
                    {
                        _notyf.Error(response.Message ?? "Password reset failed");
                        _logger.LogWarning($"Password reset failed for {loginRequestDto.Username}: {response.Message}");
                        ViewBag.RegisterResponse = response;
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        _notyf.Error(error.ErrorMessage);
                    }
                }

                return View("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during password reset: {ex.Message}");
                _notyf.Error("An error occurred during password reset. Please try again.");
                return View("Index");
            }
        }

        public IActionResult Logout([FromQuery] string returnUrl)
        {
            try
            {
                HttpContext.Session.Remove("Token");
                _logger.LogInformation("User logged out successfully");
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during logout: {ex.Message}");
                return RedirectToAction("Index");
            }
        }

        #region API

        [HttpPost]
        public IActionResult LoginUserAPI(LoginRequestDto loginRequest)
        {
            try
            {
                ModelState.Remove(nameof(loginRequest.ConfirmPassword));
                
                if (ModelState.IsValid)
                {
                    var response = _iLogin.Login(loginRequest);
                    if (response.StatusCode == HttpStatusCode.OK && response.Data != null)
                    {
                        HttpContext.Session.SetString("Token", response.Data.Token);
                        _notyf.Success(response.Message ?? "Login successful");
                        _logger.LogInformation($"API login successful for {loginRequest.Username}");
                        return Ok(response);
                    }
                    else
                    {
                        ModelState.AddModelError("", response.Message ?? "Login failed");
                        _logger.LogWarning($"API login failed for {loginRequest.Username}");
                        return BadRequest(response);
                    }
                }
                else
                {
                    _logger.LogWarning($"API login validation failed");
                    return BadRequest(new ResponseDto<LoginResponseDto> 
                    { 
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Validation failed",
                        Data = null
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error during API login: {ex.Message}");
                return StatusCode(500, new ResponseDto<LoginResponseDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        #endregion
    }
}
