using AspNetCore;
using AspNetCoreHero.ToastNotification.Abstractions;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Models;
using BusinessManagementSystem.Repositories;
using BusinessManagementSystem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Text.Encodings.Web;

namespace BusinessManagementSystem.Controllers
{
    [Authorize(Roles = "superadmin,admin_tattoo,admin_kaffe,admin_apartment")]
    public class BasicConfigurationController : BaseController
    {
        private readonly BasicConfigurationRepository _basicConfigurationRepository;

        public BasicConfigurationController(BasicConfigurationRepository basicConfigurationRepository, INotyfService notyf,IEmailSender emailSender,ILogger<BasicConfigurationController> logger,JavaScriptEncoder javaScriptEncoder): base(notyf, emailSender, javaScriptEncoder)
        {
            _basicConfigurationRepository = basicConfigurationRepository;

        }

        public IActionResult Index()
        {
            var response = _basicConfigurationRepository.GetSingleOrDefault();
            return View(response.Data);
        }

        [HttpPost]
        public IActionResult Update(BasicConfiguration basicConfiguration)
        {
            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _notyf.Error(error.ErrorMessage);
                }
                return RedirectToAction(nameof(Index));
            }

            var response = _basicConfigurationRepository.Update(basicConfiguration);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                _notyf.Success("Update successful");
            }
            else
            {
                _notyf.Error(response.Message ?? "Update failed");
            }

            return RedirectToAction(nameof(Index));
        }
    }

}
