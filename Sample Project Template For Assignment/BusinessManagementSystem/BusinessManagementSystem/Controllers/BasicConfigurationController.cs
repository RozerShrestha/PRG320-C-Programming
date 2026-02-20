using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Repositories;
using BusinessManagementSystem.Services;
using BusinessManagementSystem.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Text.Encodings.Web;

namespace BusinessManagementSystem.Controllers
{
    [Authorize(Roles = "superadmin")]
    public class BasicConfigurationController : BaseController
    {
        private readonly BasicConfigurationRepository _basicConfigurationRepository;
        private readonly ILogger<BasicConfigurationController> _logger;

        public BasicConfigurationController(BasicConfigurationRepository basicConfigurationRepository, ILogger<BasicConfigurationController> logger)
        {
            _basicConfigurationRepository = basicConfigurationRepository ?? throw new ArgumentNullException(nameof(basicConfigurationRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IActionResult Index()
        {
            try
            {
                var response = _basicConfigurationRepository.GetSingleOrDefault();
                return View(response?.Data);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error loading basic configurations: {ex.Message}");
                Notyf?.Error("Error loading configuration");
                return View(null);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Update(BasicConfiguration basicConfiguration)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors);
                    foreach (var error in errors)
                    {
                        Notyf?.Error(error.ErrorMessage);
                    }
                    return View(basicConfiguration);
                }

                var response = _basicConfigurationRepository.Update(basicConfiguration);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Notyf?.Success(response.Message ?? "Configuration updated successfully");
                }
                else
                {
                    Notyf?.Error(response.Message ?? "Error updating configuration");
                    _logger.LogError($"Error updating configuration: {response.Message}");
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error in Update: {ex.Message}");
                Notyf?.Error($"Error: {ex.Message}");
                return RedirectToAction(nameof(Index));
            }
        }
    }
}
