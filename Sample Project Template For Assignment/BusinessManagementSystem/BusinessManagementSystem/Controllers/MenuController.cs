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
        protected readonly MenuRepository _menuRepository;
        private ResponseDto<Menu> _responseDto;
        private readonly dynamic parentList;
        private readonly Multiselect roleList;
        private ILogger<MenuController> _logger;
        public ModalView _modalView;
        public MenuController(MenuRepository menuRepository, INotyfService notyf, IEmailSender emailSender, ILogger<MenuController> logger, JavaScriptEncoder javaScriptEncoder) : base(notyf, emailSender, javaScriptEncoder)
        {
            _menuRepository = menuRepository;
            _responseDto = new ResponseDto<Menu>();
            _logger = logger;
            parentList = _menuRepository.ParentList();
            roleList = _menuRepository.RoleList();
            _modalView = new ModalView("Delete Confirmation !", "Delete", "Are you sure to delete the selected Menu?", "");

        }
        // GET: MenuController
        public ActionResult Index()
        {
            ViewBag.ModalInformation = _modalView;
            _responseDto = _menuRepository.GetAll();
            return View(_responseDto.Datas);
        }

        // GET: MenuController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: MenuController/Create
        public ActionResult Create()
        {
            ViewData["ParentList"] = new SelectList(parentList, "Parent", "Name");
            //ViewData["RoleList"] = new SelectList(roleList, "Id", "Name");
            _responseDto.Data = new Menu();
            _responseDto.Data.Multiselect = roleList;
            return View(_responseDto.Data);
        }

        // POST: MenuController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Menu menu)
        {
            if (ModelState.IsValid)
            {
                _responseDto = _menuRepository.CreateMenu(menu);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    _notyf.Success(_responseDto.Message);
                }
                else
                {
                    _notyf.Error(_responseDto.Message);
                }
            }
            else
            {
                IEnumerable<ModelError> errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
                foreach (var error in errors)
                {
                    _notyf.Error(error.ErrorMessage);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: MenuController/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0) return NotFound();
            else
            {
                _responseDto = _menuRepository.GetMenuById(id);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    ViewData["ParentList"] = new SelectList(parentList, "Parent", "Name");
                    //ViewData["RoleList"] = new SelectList(roleList, "Id", "Name");

                    return View(_responseDto.Data);
                }
                else
                {
                    //ViewData["ErrorMessage"] = _responseDto.Message;
                    _notyf.Error(_responseDto.Message);
                    _logger.LogError($"## {this.GetType().Name} Edit: Not Found {_responseDto.Message}");
                    return View();
                }
            }
        }

        // POST: MenuController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Menu menu)
        {
            if (ModelState.IsValid)
            {
                _responseDto = _menuRepository.UpdateMenu(menu);
                if (_responseDto.StatusCode == HttpStatusCode.OK)
                {
                    _notyf.Success(_responseDto.Message);
                }
                else
                {
                    _notyf.Error(_responseDto.Message);
                }
            }
            else
            {
                IEnumerable<ModelError> errors = ModelState.Values.SelectMany(v => v.Errors).ToList();
                foreach (var error in errors)
                {
                    _notyf.Error(error.ErrorMessage);
                }
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: MenuController/Delete/5
        public ActionResult Delete(int id)
        {
            if (roleName != SD.Role_Superadmin)
            {
                _notyf.Warning("Only Super Admin can delete");
                return RedirectToAction(nameof(Index));
            }
            var item = _menuRepository.GetMenuById(id);
            _responseDto = _menuRepository.Delete(item.Data);
            if (_responseDto.StatusCode == HttpStatusCode.OK)
            {
                _notyf.Success(_responseDto.Message);
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _notyf.Error("Menu Not Found");
                return RedirectToAction(nameof(Index));
            }
            
        }

        // POST: MenuController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            
            if (_responseDto.StatusCode == HttpStatusCode.OK)
            {
                _notyf.Success(_responseDto.Message);
            }
            else
            {
                _notyf.Error(_responseDto.Message);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
