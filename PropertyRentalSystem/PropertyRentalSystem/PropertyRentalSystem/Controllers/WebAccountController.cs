using Microsoft.AspNetCore.Mvc;
using PropertyRentalSystem.Helpers;
using PropertyRentalSystem.Repositories.Interfaces;
using PropertyRentalSystem.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace PropertyRentalSystem.Controllers
{
    public class WebAccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly IUserRepository _userRepository;
        private readonly ILogger<WebAccountController> _logger;

        public WebAccountController(
            IAuthService authService,
            IUserRepository userRepository,
            ILogger<WebAccountController> logger)
        {
            _authService = authService;
            _userRepository = userRepository;
            _logger = logger;
        }

        // GET: /WebAccount/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        // POST: /WebAccount/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _logger.LogInformation("Login attempt for email: {Email}", model.Email);

            var result = await _authService.LoginAsync(model.Email, model.Password);

            if (!result.Success)
            {
                _logger.LogWarning("Login failed for {Email}: {Message}", model.Email, result.Message);
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            // Store authentication data in session
            HttpContext.Session.SetString("JwtToken", result.Token!);
            HttpContext.Session.SetInt32("UserId", result.User!.Id);
            HttpContext.Session.SetString("UserEmail", result.User.Email);
            HttpContext.Session.SetString("UserName", $"{result.User.FirstName} {result.User.LastName}");
            HttpContext.Session.SetString("IsAuthenticated", "true");

            // Get and store user roles as JSON (not comma-separated string)
            var userRoles = await _userRepository.GetUserRolesAsync(result.User.Id);
            HttpContext.Session.SetUserRoles(userRoles.ToList()); // Use the SetUserRoles helper method

            _logger.LogInformation("User logged in successfully: {Email}", model.Email);
            TempData["SuccessMessage"] = "Login successful!";

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            // Always redirect to Home/Index after login
            // TODO: Change to role-based redirect after creating WebProperties views
            return RedirectToAction("Index", "Home");
        }

        // GET: /WebAccount/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // POST: /WebAccount/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _logger.LogInformation("Registration attempt for email: {Email}", model.Email);

            var result = await _authService.RegisterAsync(
                model.FirstName,
                model.LastName,
                model.Email,
                model.PhoneNumber,
                model.Password,
                model.Role
            );

            if (!result.Success)
            {
                _logger.LogWarning("Registration failed for {Email}: {Message}", model.Email, result.Message);
                ModelState.AddModelError(string.Empty, result.Message);
                return View(model);
            }

            _logger.LogInformation("User registered successfully: {Email}", model.Email);
            TempData["SuccessMessage"] = "Registration successful! Please login.";
            return RedirectToAction(nameof(Login));
        }

        // GET: /WebAccount/Logout
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            TempData["SuccessMessage"] = "You have been logged out successfully.";
            return RedirectToAction("Index", "Home");
        }

        // View Models
        public class LoginViewModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;

            [Display(Name = "Remember me?")]
            public bool RememberMe { get; set; }
        }

        public class RegisterViewModel
        {
            [Required]
            [Display(Name = "First Name")]
            [StringLength(100)]
            public string FirstName { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Last Name")]
            [StringLength(100)]
            public string LastName { get; set; } = string.Empty;

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; } = string.Empty;

            [Required]
            [Phone]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; } = string.Empty;

            [Required]
            [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; } = string.Empty;

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Confirm Password")]
            [Compare("Password", ErrorMessage = "Password and confirmation do not match")]
            public string ConfirmPassword { get; set; } = string.Empty;

            [Required]
            [Display(Name = "Register As")]
            public string Role { get; set; } = string.Empty;
        }
    }
}
