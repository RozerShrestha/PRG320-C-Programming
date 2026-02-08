using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Repositories;
using BusinessManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace BusinessManagementSystem.Controllers
{
    [AllowAnonymous]
    public class ErrorController : BaseController
    {
        public ErrorController(ILogger<DashboardController> logger, INotyfService notyf, IEmailSender emailSender, JavaScriptEncoder javaScriptEncoder) : base(notyf, emailSender, javaScriptEncoder)
        {
        }
        public IActionResult Index(int code)
        {
            return View();
        }
        public IActionResult PageNotFound()
        {
            _notyf.Warning("Page Not Found");
            return View();
        }
        public IActionResult AccessDenied()
        {
            _notyf.Warning("you tried to view unauthorized data, your access log is saved and forwarded to HR");
            return View();
        }
        public IActionResult PageNotAllowed()
        {
            _notyf.Warning("Page not allowed");
            return View();
        }
    }
}
