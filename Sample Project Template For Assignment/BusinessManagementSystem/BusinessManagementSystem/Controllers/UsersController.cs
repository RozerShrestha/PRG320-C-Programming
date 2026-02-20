using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Helper;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Repositories;
using BusinessManagementSystem.Services;
using BusinessManagementSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;

namespace BusinessManagementSystem.Controllers
{
    [Authorize]
    public class UsersController : BaseController
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger<UsersController> _logger;
        private ResponseDto<User> _responseDto;
        private ResponseDto<UserDto> _responseUserDto;
        private ResponseDto<UserRoleDto> _responseUserRoleDto;
        private readonly ModalView _modalView;

        public UsersController(UserRepository userRepository, ILogger<UsersController> logger)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _responseDto = new ResponseDto<User>();
            _responseUserDto = new ResponseDto<UserDto>();
            _responseUserRoleDto = new ResponseDto<UserRoleDto>();
            _modalView = new ModalView("Delete Confirmation!", "Delete", "Are you sure to delete the selected User?", "");
        }

        private void SetupUserViewData()
        {
            try
            {
                var roleList = _userRepository.RoleList();
                ViewData["RoleList"] = new SelectList(roleList, "Id", "Name");
                ViewBag.OccupationList = new SelectList(SD.Occupations, "Value", "Value");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error setting up user view data: {ex.Message}");
                Notyf?.Error("Error loading user data");
            }
        }

        [HttpGet]
        [Authorize(Roles = "superadmin,admin_tattoo")]
        public IActionResult Index()
        {
            try
            {
                ViewBag.ModalInformation = _modalView;
                return View(_responseDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Index: {ex.Message}");
                Notyf?.Error("Error loading users page");
                return View(new ResponseDto<User>());
            }
        }

        [HttpGet]
        public IActionResult Detail(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                {
                    _responseDto = _userRepository.GetById(userId);
                    if (_responseDto?.StatusCode == HttpStatusCode.OK && _responseDto.Data != null)
                    {
                        guid = _responseDto.Data.Guid;
                        _responseUserDto = _userRepository.GetUserById(userId);
                        if (_responseUserDto?.StatusCode == HttpStatusCode.OK && _responseUserDto.Data != null)
                        {
                            return View(_responseUserDto.Data);
                        }
                    }
                    return NotFound();
                }
                else
                {
                    _responseUserDto = _userRepository.GetUserByGuid(guid);
                    if (_responseUserDto?.StatusCode == HttpStatusCode.OK && _responseUserDto.Data != null)
                    {
                        return View(_responseUserDto.Data);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Detail: {ex.Message}");
                Notyf?.Error("Error loading user details");
                return NotFound();
            }
        }

        [HttpGet]
        [Authorize(Roles = "superadmin")]
        public IActionResult Create()
        {
            try
            {
                SetupUserViewData();
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Create: {ex.Message}");
                Notyf?.Error("Error loading create user page");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [Authorize(Roles = "superadmin")]
        public IActionResult Create(UserDto userDto, IFormFile? ProfilePictureLink)
        {
            try
            {
                SetupUserViewData();

                if (!Helpers.ValidateDocumentUpload(ProfilePictureLink))
                {
                    Notyf?.Warning("Profile Picture Upload Error: Valid files are of extension pdf or jpg or jpeg");
                    return View(userDto);
                }

                if (ModelState.IsValid)
                {
                    userDto.ProfilePictureLink = ProfilePictureLink == null ? string.Empty : Helpers.DocUpload(ProfilePictureLink, "ProfilePicture", username);
                    _responseDto = _userRepository.CreateUser(userDto);
                    if (_responseDto.StatusCode == HttpStatusCode.OK)
                    {
                        Notyf?.Success(_responseDto.Message ?? "User created successfully");
                        _logger.LogInformation($"User {userDto.Email} created successfully");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Notyf?.Error(_responseDto.Message ?? "Error creating user");
                        return View(userDto);
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        Notyf?.Error(error.ErrorMessage);
                    }
                    return View(userDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating user: {ex.Message}");
                Notyf?.Error($"Error: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Edit(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                {
                    return NotFound();
                }

                SetupUserViewData();
                _responseUserDto = _userRepository.GetUserByGuid(guid);
                if (_responseUserDto?.StatusCode == HttpStatusCode.OK && _responseUserDto.Data != null)
                {
                    return View(_responseUserDto.Data);
                }
                else
                {
                    Notyf?.Error("User not found");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Edit: {ex.Message}");
                Notyf?.Error("Error loading edit user page");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(UserDto userDto, IFormFile? ProfilePictureLink)
        {
            try
            {
                ModelState.Remove(nameof(userDto.Password));
                ModelState.Remove(nameof(userDto.ConfirmPassword));
                SetupUserViewData();

                if (roleName == SD.Role_Superadmin || userId == userDto.UserId)
                {
                    if (ModelState.IsValid)
                    {
                        userDto.ProfilePictureLink = ProfilePictureLink == null ? string.Empty : Helpers.DocUpload(ProfilePictureLink, "ProfilePicture", username);
                        _responseDto = _userRepository.UpdateUser(userDto);
                        if (_responseDto.StatusCode == HttpStatusCode.OK)
                        {
                            Notyf?.Success(_responseDto.Message ?? "User updated successfully");
                            _logger.LogInformation($"User {userDto.Email} updated successfully");
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            Notyf?.Error(_responseDto.Message ?? "Error updating user");
                            return View(userDto);
                        }
                    }
                    else
                    {
                        var errors = ModelState.Values.SelectMany(v => v.Errors);
                        foreach (var error in errors)
                        {
                            Notyf?.Error(error.ErrorMessage);
                        }
                        return View(userDto);
                    }
                }
                else
                {
                    Notyf?.Warning($"{fullName} is not authorized to perform this task");
                    _logger.LogWarning($"Unauthorized edit attempt by {username} for user {userDto.UserId}");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating user {userDto.UserId}: {ex.Message}");
                Notyf?.Error($"Error: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "superadmin")]
        public IActionResult DeleteConfirmed(int UserId)
        {
            try
            {
                _responseDto = _userRepository.GetById(UserId);
                if (_responseDto?.Data != null)
                {
                    _responseDto = _userRepository.Delete(_responseDto.Data);
                    if (_responseDto.StatusCode == HttpStatusCode.OK)
                    {
                        Notyf?.Success(_responseDto.Message ?? "User deleted successfully");
                        _logger.LogInformation($"User {UserId} deleted successfully");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Notyf?.Error(_responseDto.Message ?? "Error deleting user");
                        return NotFound();
                    }
                }
                else
                {
                    Notyf?.Error("User not found");
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user {UserId}: {ex.Message}");
                Notyf?.Error($"Error deleting user: {ex.Message}");
                return NotFound();
            }
        }

        #region API CALLS

        [HttpGet]
        [Authorize(Roles = "superadmin,admin_tattoo")]
        public IActionResult GetAllUser()
        {
            //change the responseDTo
            try
            {
                if (roleName == SD.Role_Superadmin)
                {
                    _responseUserRoleDto = _userRepository.GetAllUser(SD.Role_Superadmin);
                }
                else
                {
                    _responseUserRoleDto = new ResponseDto<UserRoleDto>
                    {
                        StatusCode = HttpStatusCode.Unauthorized,
                        Message = "You do not have permission to view all users"
                    };
                    return BadRequest(_responseUserRoleDto);
                }

                if (_responseUserRoleDto?.StatusCode == HttpStatusCode.OK)
                {
                    return Ok(_responseUserRoleDto);
                }
                else
                {
                    return BadRequest(_responseUserRoleDto);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in GetAllUser: {ex.Message}");
                return StatusCode(500, new ResponseDto<UserRoleDto>
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Message = $"Error: {ex.Message}"
                });
            }
        }

        [Authorize(Roles = "superadmin")]
        [HttpGet]
        public IActionResult Delete(Guid guid)
        {
            try
            {
                if (guid == Guid.Empty)
                {
                    Notyf?.Error("Invalid user ID");
                    return NotFound();
                }

                var userResponse = _userRepository.GetUserByGuid(guid);
                if (userResponse?.Data == null)
                {
                    return NotFound();
                }

                var userById = _userRepository.GetById(userResponse.Data.UserId);
                if (userById?.Data == null)
                {
                    return NotFound();
                }

                _responseDto = _userRepository.Delete(userById.Data);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    Notyf?.Success(_responseDto.Message ?? "User deleted successfully");
                    _logger.LogInformation($"User {guid} deleted successfully via API");
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting user via API {guid}: {ex.Message}");
                return StatusCode(500, new { message = ex.Message });
            }
        }

        #endregion
    }
}
