using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessManagementSystem.Dto;
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
    [Authorize(Roles = "superadmin")]
    public class RoleController : BaseController
    {
        private readonly RoleRepository _roleRepository;
        private readonly ILogger<RoleController> _logger;
        private ResponseDto<Role> _responseDto;
        private readonly ModalView _modalView;

        public RoleController(RoleRepository roleRepository, ILogger<RoleController> logger)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _responseDto = new ResponseDto<Role>();
            _modalView = new ModalView("Delete Confirmation!", "Delete", "Are you sure to delete the selected Role?", "");
        }

        public IActionResult Index()
        {
            try
            {
                ViewBag.ModalInformation = _modalView;
                _responseDto = _roleRepository.GetAll();
                return View(_responseDto.Datas);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Index: {ex.Message}");
                Notyf?.Error("Error loading roles");
                return View(new List<Role>());
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _responseDto = _roleRepository.Insert(role);
                    if (_responseDto.StatusCode == HttpStatusCode.OK)
                    {
                        Notyf?.Success(_responseDto.Message ?? "Role created successfully");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Notyf?.Error(_responseDto.Message ?? "Error creating role");
                        return View(role);
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        Notyf?.Error(error.ErrorMessage);
                    }
                    return View(role);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating role: {ex.Message}");
                Notyf?.Error($"Error: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Edit(int id)
        {
            try
            {
                if (id <= 0)
                {
                    Notyf?.Warning("Cannot Update Superadmin");
                    return RedirectToAction(nameof(Index));
                }

                _responseDto = _roleRepository.GetById(id);
                if (_responseDto.StatusCode == HttpStatusCode.OK && _responseDto.Data != null)
                {
                    return View(_responseDto.Data);
                }
                else
                {
                    Notyf?.Error(_responseDto.Message ?? "Role not found");
                    _logger.LogError($"Role Edit: Not Found - {_responseDto.Message}");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error editing role {id}: {ex.Message}");
                Notyf?.Error("Error loading edit role page");
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Role role)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _responseDto = _roleRepository.Update(role);
                    if (_responseDto.StatusCode == HttpStatusCode.OK)
                    {
                        Notyf?.Success(_responseDto.Message ?? "Role updated successfully");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Notyf?.Error(_responseDto.Message ?? "Error updating role");
                        return View(role);
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        Notyf?.Error(error.ErrorMessage);
                    }
                    return View(role);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating role {role.Id}: {ex.Message}");
                Notyf?.Error($"Error: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        public ActionResult Delete(int id)
        {
            try
            {
                if (roleName != SD.Role_Superadmin)
                {
                    Notyf?.Warning("Only Super Admin can delete roles");
                    return RedirectToAction(nameof(Index));
                }

                if (id <= 0)
                {
                    return NotFound();
                }

                var roleResponse = _roleRepository.GetById(id);
                if (roleResponse?.Data == null)
                {
                    Notyf?.Error("Role not found");
                    return RedirectToAction(nameof(Index));
                }

                _responseDto = _roleRepository.Delete(roleResponse.Data);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    Notyf?.Success(_responseDto.Message ?? "Role deleted successfully");
                }
                else
                {
                    Notyf?.Error(_responseDto.Message ?? "Error deleting role");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting role {id}: {ex.Message}");
                Notyf?.Error($"Error: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
