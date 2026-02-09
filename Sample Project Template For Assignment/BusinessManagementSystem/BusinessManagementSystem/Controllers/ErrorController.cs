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
        private readonly ILogger<ErrorController> _logger;

        public ErrorController(ILogger<ErrorController> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index(int code)
        {
            _logger.LogWarning($"Error page accessed with code: {code}");
            return View();
        }

        public IActionResult PageNotFound()
        {
            Notyf?.Warning("Page Not Found");
            _logger.LogWarning("404 - Page not found");
            return View();
        }

        public IActionResult AccessDenied()
        {
            Notyf?.Warning("You tried to view unauthorized data. Your access log has been saved and forwarded to HR");
            _logger.LogWarning("403 - Access denied");
            return View();
        }

        public IActionResult PageNotAllowed()
        {
            Notyf?.Warning("Page not allowed");
            _logger.LogWarning("405 - Method not allowed");
            return View();
        }
    }
}
