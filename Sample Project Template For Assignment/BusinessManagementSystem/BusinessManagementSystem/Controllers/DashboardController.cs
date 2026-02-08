using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Repositories;
using BusinessManagementSystem.Services;
using BusinessManagementSystem.Utility;
using BusinessManagementSystem.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Text.Encodings.Web;

namespace BusinessManagementSystem.Controllers
{
    [Authorize]
    public class DashboardController : BaseController
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<DashboardController> _logger;
        //private readonly ModalView _modalView;
        private ResponseDto<DashboardVM> _responseDto;
        public DashboardController(IWebHostEnvironment env, ILogger<DashboardController> logger, INotyfService notyf, IEmailSender emailSender, JavaScriptEncoder javaScriptEncoder) : base(notyf, emailSender, javaScriptEncoder)
        {
            _env = env;
            _logger = logger;
            //_modalView = new ModalView();
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
