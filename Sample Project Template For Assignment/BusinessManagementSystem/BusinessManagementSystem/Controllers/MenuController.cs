using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Repositories;
using BusinessManagementSystem.Services;
using BusinessManagementSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Text.Encodings.Web;

namespace BusinessManagementSystem.Controllers
{
    [Authorize(Roles = "superadmin,admin_tattoo,admin_kaffe,admin_apartment")]
    public class MenuController : BaseController
    {
        private readonly MenuRepository _menuRepository;
        private readonly ILogger<MenuController> _logger;
        private ResponseDto<Menu> _responseDto;
        private readonly ModalView _modalView;

        public MenuController(MenuRepository menuRepository, ILogger<MenuController> logger, ResponseDto<Menu> responseDto)
        {
            _menuRepository = menuRepository ?? throw new ArgumentNullException(nameof(menuRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _responseDto = responseDto;
            _modalView = new ModalView("Delete Confirmation!", "Delete", "Are you sure to delete the selected Menu?", "");
        }

        /// <summary>
        /// Setup common view data for menu creation/editing
        /// </summary>
        private void SetupMenuViewData()
        {
            try
            {
                var parentList = _menuRepository.ParentList();
                var roleList = _menuRepository.RoleList();
                ViewData["ParentList"] = new SelectList(parentList, "Parent", "Name");
                ViewData["RoleList"] = roleList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error setting up menu view data: {ex.Message}");
                Notyf?.Error("Error loading menu data");
            }
        }

        // GET: MenuController
        public ActionResult Index()
        {
            try
            {
                ViewBag.ModalInformation = _modalView;
                _responseDto = _menuRepository.GetAll();
                return View(_responseDto.Datas);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Index: {ex.Message}");
                Notyf?.Error("Error loading menus");
                return View(new List<Menu>());
            }
        }

        // GET: MenuController/Create
        public ActionResult Create()
        {
            try
            {
                SetupMenuViewData();
                var roleList = _menuRepository.RoleList();
                _responseDto.Data = new Menu { Multiselect = roleList };
                return View(_responseDto.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Create: {ex.Message}");
                Notyf?.Error("Error loading create menu page");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: MenuController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Menu menu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _responseDto = _menuRepository.CreateMenu(menu);
                    if (_responseDto.StatusCode == HttpStatusCode.OK)
                    {
                        Notyf?.Success(_responseDto.Message ?? "Menu created successfully");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Notyf?.Error(_responseDto.Message ?? "Error creating menu");
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        Notyf?.Error(error.ErrorMessage);
                    }
                }

                // Reload form on validation error
                SetupMenuViewData();
                var roleList = _menuRepository.RoleList();
                menu.Multiselect = roleList;
                return View(menu);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating menu: {ex.Message}");
                Notyf?.Error($"Error: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: MenuController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return NotFound();
                }

                _responseDto = _menuRepository.GetMenuById(id);
                if (_responseDto.StatusCode == HttpStatusCode.OK && _responseDto.Data != null)
                {
                    SetupMenuViewData();
                    return View(_responseDto.Data);
                }
                else
                {
                    Notyf?.Error(_responseDto.Message ?? "Menu not found");
                    _logger.LogError($"Menu Edit: Not Found - {_responseDto.Message}");
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error editing menu {id}: {ex.Message}");
                Notyf?.Error("Error loading edit menu page");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: MenuController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu menu)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    _responseDto = _menuRepository.UpdateMenu(menu);
                    if (_responseDto.StatusCode == HttpStatusCode.OK)
                    {
                        Notyf?.Success(_responseDto.Message ?? "Menu updated successfully");
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        Notyf?.Error(_responseDto.Message ?? "Error updating menu");
                    }
                }
                else
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        Notyf?.Error(error.ErrorMessage);
                    }
                }

                // Reload form on validation error
                SetupMenuViewData();
                return View(menu);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating menu {menu.Id}: {ex.Message}");
                Notyf?.Error($"Error: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: MenuController/Delete/5
        public ActionResult Delete(int id)
        {
            try
            {
                if (roleName != SD.Role_Superadmin)
                {
                    Notyf?.Warning("Only Super Admin can delete menus");
                    return RedirectToAction(nameof(Index));
                }

                if (id <= 0)
                {
                    return NotFound();
                }

                var menuResponse = _menuRepository.GetMenuById(id);
                if (menuResponse?.Data == null)
                {
                    Notyf?.Error("Menu not found");
                    return RedirectToAction(nameof(Index));
                }

                _responseDto = _menuRepository.Delete(menuResponse.Data);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    Notyf?.Success(_responseDto.Message ?? "Menu deleted successfully");
                }
                else
                {
                    Notyf?.Error(_responseDto.Message ?? "Error deleting menu");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting menu {id}: {ex.Message}");
                Notyf?.Error($"Error: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: MenuController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                if (roleName != SD.Role_Superadmin)
                {
                    Notyf?.Warning("Only Super Admin can delete menus");
                    return RedirectToAction(nameof(Index));
                }

                if (id <= 0)
                {
                    return NotFound();
                }

                var menuResponse = _menuRepository.GetMenuById(id);
                if (menuResponse?.Data == null)
                {
                    Notyf?.Error("Menu not found");
                    return RedirectToAction(nameof(Index));
                }

                _responseDto = _menuRepository.Delete(menuResponse.Data);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    Notyf?.Success(_responseDto.Message ?? "Menu deleted successfully");
                }
                else
                {
                    Notyf?.Error(_responseDto.Message ?? "Error deleting menu");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Delete POST for menu {id}: {ex.Message}");
                Notyf?.Error($"Error: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
