# ?? Migration from Razor Pages to MVC Architecture

## Overview

Your project has been refactored from a **Monolithic Razor Pages** architecture to a proper **MVC (Model-View-Controller)** pattern with clear separation between:

1. **API Controllers** (`/api/*`) - RESTful API endpoints
2. **Web Controllers** - Handle web UI requests and return Views
3. **Views** - Razor views for the UI
4. **Services** - Business logic
5. **Repositories** - Data access

---

## Architecture Changes

### Before (Razor Pages):
```
Pages/
??? Account/
?   ??? Login.cshtml
?   ??? Login.cshtml.cs (PageModel with OnGet/OnPost)
?   ??? Register.cshtml
?   ??? Register.cshtml.cs
??? Properties/
?   ??? Index.cshtml
?   ??? Index.cshtml.cs
??? ...
```

### After (MVC):
```
Controllers/
??? API/ (REST API)
?   ??? AuthController.cs
?   ??? PropertiesController.cs
?   ??? BookingsController.cs
?   ??? ReviewsController.cs
?   ??? PaymentsController.cs
??? Web/ (Web UI)
    ??? WebAccountController.cs
    ??? WebPropertiesController.cs
    ??? WebBookingsController.cs
    ??? HomeController.cs

Views/
??? WebAccount/
?   ??? Login.cshtml
?   ??? Register.cshtml
??? WebProperties/
?   ??? Index.cshtml
?   ??? Details.cshtml
?   ??? MyProperties.cshtml
??? Home/
?   ??? Index.cshtml
??? Shared/
    ??? _Layout.cshtml
    ??? _ValidationScriptsPartial.cshtml
```

---

## What Has Been Done

### ? Completed:

1. **Created API Controllers** (already done):
   - `/api/auth/*` - Authentication endpoints
   - `/api/properties/*` - Property management
   - `/api/bookings/*` - Booking management
   - `/api/reviews/*` - Review management
   - `/api/payments/*` - Payment processing

2. **Created Web Controllers**:
   - `WebAccountController` - Handles login/register web forms
   - `HomeController` - Home page and general pages

3. **Updated Program.cs**:
   - Changed from `AddRazorPages()` to `AddControllersWithViews()`
   - Added MVC routing with default route pattern
   - Kept API controllers for REST API

4. **Created Views Structure**:
   - `Views/` folder with proper MVC structure
   - `_Layout.cshtml` with MVC routing (`asp-controller`/`asp-action`)
   - `_ViewStart.cshtml` and `_ViewImports.cshtml`

---

## Migration Steps (What YOU Need to Do)

### Step 1: Delete Pages Folder

```bash
# Remove the entire Pages folder
Remove-Item -Path "D:\Rozer Internal\PropertyRentalSystem\PropertyRentalSystem\Pages" -Recurse -Force
```

**What this removes:**
- All `.cshtml` and `.cshtml.cs` files in Pages folder
- Account, Properties, Bookings, Reviews, Payments page models

---

### Step 2: Create Complete Web Controllers

You need to create web controllers for all pages. Here's the structure:

#### A. WebPropertiesController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using PropertyRentalSystem.Helpers;
using PropertyRentalSystem.Models;
using PropertyRentalSystem.Repositories.Interfaces;

namespace PropertyRentalSystem.Controllers
{
    public class WebPropertiesController : Controller
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IReviewRepository _reviewRepository;
        private readonly ILogger<WebPropertiesController> _logger;

        public WebPropertiesController(
            IPropertyRepository propertyRepository,
            IReviewRepository reviewRepository,
            ILogger<WebPropertiesController> logger)
        {
            _propertyRepository = propertyRepository;
            _reviewRepository = reviewRepository;
            _logger = logger;
        }

        // GET: /WebProperties/Index
        [HttpGet]
        public async Task<IActionResult> Index(string? city, DateTime? checkIn, DateTime? checkOut, int? guests)
        {
            var properties = await _propertyRepository.SearchPropertiesAsync(city, checkIn, checkOut, guests);
            
            ViewData["City"] = city;
            ViewData["CheckIn"] = checkIn;
            ViewData["CheckOut"] = checkOut;
            ViewData["Guests"] = guests;

            return View(properties);
        }

        // GET: /WebProperties/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id, DateTime? checkIn, DateTime? checkOut, int? guests)
        {
            var property = await _propertyRepository.GetPropertyWithDetailsAsync(id);
            
            if (property == null)
            {
                return NotFound();
            }

            ViewData["CheckIn"] = checkIn;
            ViewData["CheckOut"] = checkOut;
            ViewData["Guests"] = guests;

            return View(property);
        }

        // GET: /WebProperties/MyProperties (Host only)
        [HttpGet]
        public async Task<IActionResult> MyProperties()
        {
            if (!HttpContext.Session.IsAuthenticated())
            {
                return RedirectToAction("Login", "WebAccount", new { returnUrl = "/WebProperties/MyProperties" });
            }

            if (!HttpContext.Session.IsInRole("Host"))
            {
                TempData["ErrorMessage"] = "You must be a host to access this page.";
                return RedirectToAction("Index", "Home");
            }

            var userId = HttpContext.Session.GetUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "WebAccount");
            }

            var properties = await _propertyRepository.GetPropertiesByOwnerAsync(userId.Value);
            return View(properties);
        }

        // GET: /WebProperties/Create (Host only)
        [HttpGet]
        public IActionResult Create()
        {
            if (!HttpContext.Session.IsAuthenticated())
            {
                return RedirectToAction("Login", "WebAccount");
            }

            if (!HttpContext.Session.IsInRole("Host"))
            {
                TempData["ErrorMessage"] = "You must be a host to list properties.";
                return RedirectToAction("Index", "Home");
            }

            return View(new PropertyCreateViewModel());
        }

        // POST: /WebProperties/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PropertyCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = HttpContext.Session.GetUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "WebAccount");
            }

            var property = new Property
            {
                Title = model.Title,
                Description = model.Description,
                Address = model.Address,
                City = model.City,
                Country = model.Country,
                ZipCode = model.ZipCode,
                PricePerNight = model.PricePerNight,
                MaxGuests = model.MaxGuests,
                Bedrooms = model.Bedrooms,
                Bathrooms = model.Bathrooms,
                OwnerId = userId.Value,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _propertyRepository.AddAsync(property);

            // Add images
            if (!string.IsNullOrWhiteSpace(model.ImageUrls))
            {
                var urls = model.ImageUrls.Split('\n', StringSplitOptions.RemoveEmptyEntries);
                int order = 1;
                foreach (var url in urls)
                {
                    property.Images.Add(new PropertyImage
                    {
                        PropertyId = property.Id,
                        ImageUrl = url.Trim(),
                        IsPrimary = order == 1,
                        DisplayOrder = order++,
                        UploadedAt = DateTime.UtcNow
                    });
                }
                await _propertyRepository.UpdateAsync(property);
            }

            TempData["SuccessMessage"] = "Property listed successfully!";
            return RedirectToAction(nameof(MyProperties));
        }

        // View Model
        public class PropertyCreateViewModel
        {
            [Required]
            [StringLength(200)]
            [Display(Name = "Title")]
            public string Title { get; set; } = string.Empty;

            [Required]
            [StringLength(2000)]
            [Display(Name = "Description")]
            public string Description { get; set; } = string.Empty;

            [Required]
            [StringLength(500)]
            [Display(Name = "Address")]
            public string Address { get; set; } = string.Empty;

            [Required]
            [StringLength(100)]
            [Display(Name = "City")]
            public string City { get; set; } = string.Empty;

            [Required]
            [StringLength(100)]
            [Display(Name = "Country")]
            public string Country { get; set; } = string.Empty;

            [Required]
            [StringLength(20)]
            [Display(Name = "Zip Code")]
            public string ZipCode { get; set; } = string.Empty;

            [Required]
            [Range(0.01, 10000)]
            [Display(Name = "Price Per Night")]
            public decimal PricePerNight { get; set; }

            [Required]
            [Range(1, 100)]
            [Display(Name = "Max Guests")]
            public int MaxGuests { get; set; }

            [Required]
            [Range(1, 50)]
            [Display(Name = "Bedrooms")]
            public int Bedrooms { get; set; }

            [Required]
            [Range(1, 50)]
            [Display(Name = "Bathrooms")]
            public int Bathrooms { get; set; }

            [Display(Name = "Image URLs (one per line)")]
            public string? ImageUrls { get; set; }
        }
    }
}
```

#### B. WebBookingsController.cs

```csharp
using Microsoft.AspNetCore.Mvc;
using PropertyRentalSystem.Helpers;
using PropertyRentalSystem.Models;
using PropertyRentalSystem.Repositories.Interfaces;
using PropertyRentalSystem.Services.Interfaces;

namespace PropertyRentalSystem.Controllers
{
    public class WebBookingsController : Controller
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IBookingService _bookingService;
        private readonly ILogger<WebBookingsController> _logger;

        public WebBookingsController(
            IBookingRepository bookingRepository,
            IBookingService bookingService,
            ILogger<WebBookingsController> logger)
        {
            _bookingRepository = bookingRepository;
            _bookingService = bookingService;
            _logger = logger;
        }

        // GET: /WebBookings/MyBookings
        [HttpGet]
        public async Task<IActionResult> MyBookings(string? statusFilter = "All")
        {
            if (!HttpContext.Session.IsAuthenticated())
            {
                return RedirectToAction("Login", "WebAccount", new { returnUrl = "/WebBookings/MyBookings" });
            }

            var userId = HttpContext.Session.GetUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "WebAccount");
            }

            var bookings = await _bookingRepository.GetBookingsByGuestAsync(userId.Value);
            var bookingsList = bookings.ToList();

            // Filter by status
            if (statusFilter != "All" && Enum.TryParse<BookingStatus>(statusFilter, out var status))
            {
                bookingsList = bookingsList.Where(b => b.Status == status).ToList();
            }

            ViewData["StatusFilter"] = statusFilter;
            return View(bookingsList);
        }

        // GET: /WebBookings/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            if (!HttpContext.Session.IsAuthenticated())
            {
                return RedirectToAction("Login", "WebAccount", new { returnUrl = $"/WebBookings/Details/{id}" });
            }

            var booking = await _bookingRepository.GetBookingWithDetailsAsync(id);
            
            if (booking == null)
            {
                return NotFound();
            }

            var userId = HttpContext.Session.GetUserId();
            if (!userId.HasValue || booking.GuestId != userId.Value)
            {
                TempData["ErrorMessage"] = "You are not authorized to view this booking.";
                return RedirectToAction("Index", "Home");
            }

            return View(booking);
        }

        // POST: /WebBookings/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            if (!HttpContext.Session.IsAuthenticated())
            {
                return RedirectToAction("Login", "WebAccount");
            }

            var userId = HttpContext.Session.GetUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "WebAccount");
            }

            var result = await _bookingService.CancelBookingAsync(id, userId.Value);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
            }
            else
            {
                TempData["ErrorMessage"] = result.Message;
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // POST: /WebBookings/Create (create booking from property details)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Invalid booking information.";
                return RedirectToAction("Details", "WebProperties", new { id = model.PropertyId });
            }

            var userId = HttpContext.Session.GetUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "WebAccount", 
                    new { returnUrl = $"/WebProperties/Details/{model.PropertyId}" });
            }

            var result = await _bookingService.CreateBookingAsync(
                model.PropertyId,
                userId.Value,
                model.CheckInDate,
                model.CheckOutDate,
                model.NumberOfGuests,
                model.SpecialRequests
            );

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.Message;
                return RedirectToAction(nameof(Details), new { id = result.Booking!.Id });
            }

            TempData["ErrorMessage"] = result.Message;
            return RedirectToAction("Details", "WebProperties", new { id = model.PropertyId });
        }

        // View Model
        public class BookingCreateViewModel
        {
            public int PropertyId { get; set; }
            
            [Required]
            public DateTime CheckInDate { get; set; }
            
            [Required]
            public DateTime CheckOutDate { get; set; }
            
            [Required]
            [Range(1, 100)]
            public int NumberOfGuests { get; set; }
            
            [StringLength(1000)]
            public string? SpecialRequests { get; set; }
        }
    }
}
```

---

### Step 3: Create All Views

Create the following view files in the respective folders:

#### Views/WebAccount/Login.cshtml
```cshtml
@model PropertyRentalSystem.Controllers.WebAccountController.LoginViewModel
@{
    ViewData["Title"] = "Login";
}

<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-5">
            <div class="card shadow">
                <div class="card-header bg-primary text-white">
                    <h3 class="mb-0"><i class="bi bi-box-arrow-in-right"></i> Login</h3>
                </div>
                <div class="card-body">
                    <form asp-action="Login" method="post">
                        <div asp-validation-summary="ModelOnly" class="text-danger"></div>
                        
                        <input type="hidden" name="returnUrl" value="@ViewData["ReturnUrl"]" />

                        <div class="mb-3">
                            <label asp-for="Email" class="form-label"></label>
                            <input asp-for="Email" class="form-control" placeholder="Enter your email" />
                            <span asp-validation-for="Email" class="text-danger"></span>
                        </div>

                        <div class="mb-3">
                            <label asp-for="Password" class="form-label"></label>
                            <input asp-for="Password" class="form-control" type="password" placeholder="Enter your password" />
                            <span asp-validation-for="Password" class="text-danger"></span>
                        </div>

                        <div class="mb-3 form-check">
                            <input asp-for="RememberMe" class="form-check-input" />
                            <label asp-for="RememberMe" class="form-check-label"></label>
                        </div>

                        <div class="d-grid gap-2">
                            <button type="submit" class="btn btn-primary btn-lg">
                                <i class="bi bi-check-circle"></i> Login
                            </button>
                        </div>
                    </form>

                    <hr />

                    <div class="text-center">
                        <p>Don't have an account? <a asp-action="Register">Register here</a></p>
                    </div>
                </div>
            </div>

            <div class="card mt-3 border-info">
                <div class="card-body bg-light">
                    <h6 class="text-info"><i class="bi bi-info-circle"></i> Demo Credentials</h6>
                    <p class="mb-1"><small><strong>Admin:</strong> admin@propertyrentals.com / Admin@123</small></p>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

#### Views/WebAccount/Register.cshtml
```cshtml
@model PropertyRentalSystem.Controllers.WebAccountController.RegisterViewModel
@{
    ViewData["Title"] = "Register";
}

<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card shadow">
                <div class="card-header bg-success text-white">
                    <h3 class="mb-0"><i class="bi bi-person-plus-fill"></i> Register</h3>
                </div>
                <div class="card-body">
                    <form asp-action="Register" method="post">
                        <div asp-validation-summary="ModelOnly" class="text-danger"></div>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label asp-for="FirstName" class="form-label"></label>
                                <input asp-for="FirstName" class="form-control" />
                                <span asp-validation-for="FirstName" class="text-danger"></span>
                            </div>

                            <div class="col-md-6 mb-3">
                                <label asp-for="LastName" class="form-label"></label>
                                <input asp-for="LastName" class="form-control" />
                                <span asp-validation-for="LastName" class="text-danger"></span>
                            </div>
                        </div>

                        <div class="mb-3">
                            <label asp-for="Email" class="form-label"></label>
                            <input asp-for="Email" class="form-control" />
                            <span asp-validation-for="Email" class="text-danger"></span>
                        </div>

                        <div class="mb-3">
                            <label asp-for="PhoneNumber" class="form-label"></label>
                            <input asp-for="PhoneNumber" class="form-control" />
                            <span asp-validation-for="PhoneNumber" class="text-danger"></span>
                        </div>

                        <div class="mb-3">
                            <label asp-for="Password" class="form-label"></label>
                            <input asp-for="Password" class="form-control" type="password" />
                            <span asp-validation-for="Password" class="text-danger"></span>
                        </div>

                        <div class="mb-3">
                            <label asp-for="ConfirmPassword" class="form-label"></label>
                            <input asp-for="ConfirmPassword" class="form-control" type="password" />
                            <span asp-validation-for="ConfirmPassword" class="text-danger"></span>
                        </div>

                        <div class="mb-3">
                            <label asp-for="Role" class="form-label"></label>
                            <select asp-for="Role" class="form-select">
                                <option value="">Select Role</option>
                                <option value="Guest">Guest (Book properties)</option>
                                <option value="Host">Host (List properties)</option>
                            </select>
                            <span asp-validation-for="Role" class="text-danger"></span>
                        </div>

                        <div class="d-grid gap-2">
                            <button type="submit" class="btn btn-success btn-lg">
                                <i class="bi bi-check-circle"></i> Register
                            </button>
                        </div>
                    </form>

                    <hr />

                    <div class="text-center">
                        <p>Already have an account? <a asp-action="Login">Login here</a></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

#### Views/Home/Index.cshtml
```cshtml
@{
    ViewData["Title"] = "Home";
}

<div class="container">
    <div class="jumbotron bg-light p-5 rounded-3 mt-5">
        <h1 class="display-4">Welcome to Property Rental System</h1>
        <p class="lead">Find your perfect rental property or list your own!</p>
        <hr class="my-4">
        <p>Browse thousands of properties or become a host and start earning.</p>
        <a class="btn btn-primary btn-lg" asp-controller="WebProperties" asp-action="Index" role="button">
            <i class="bi bi-search"></i> Browse Properties
        </a>
        <a class="btn btn-success btn-lg" asp-controller="WebAccount" asp-action="Register" role="button">
            <i class="bi bi-person-plus"></i> Become a Host
        </a>
    </div>
</div>
```

---

## Step 4: Build and Test

```bash
# Build the project
dotnet build

# Run the application
dotnet run
```

---

## URL Routing Changes

### Before (Razor Pages):
```
/Account/Login
/Account/Register
/Properties/Index
/Properties/Details?id=1
/Bookings/MyBookings
```

### After (MVC):
```
/WebAccount/Login
/WebAccount/Register
/WebProperties/Index
/WebProperties/Details/1
/WebBookings/MyBookings
```

### API Endpoints (Unchanged):
```
/api/auth/login
/api/auth/register
/api/properties
/api/bookings
/api/reviews
/api/payments
```

---

## Summary of Changes

### ? What's Been Done:
1. **Program.cs** updated to use MVC routing
2. **API Controllers** already created (5 controllers)
3. **Web Controllers** partially created (WebAccountController, HomeController)
4. **Views** structure created
5. **_Layout.cshtml** updated with MVC routing

### ?? What YOU Need to Complete:
1. **Delete Pages folder completely**
2. **Create remaining Web Controllers**:
   - Complete WebPropertiesController
   - Create WebBookingsController
   - Create WebReviewsController
   - Create WebPaymentsController
3. **Create all Views** for each controller action
4. **Copy existing .cshtml content** from Pages to Views (update routing)

---

## Benefits of This Architecture

1. **Clear Separation**: API for external clients, Web Controllers for UI
2. **RESTful API**: Can be consumed by mobile apps, SPAs
3. **Testable**: Each layer can be tested independently
4. **Scalable**: Easy to add new endpoints
5. **Standard MVC Pattern**: Industry standard for web applications

---

## Next Steps

1. Complete the migration by creating all web controllers
2. Create all views
3. Delete the Pages folder
4. Test all functionality
5. Update any hardcoded URLs in JavaScript/CSS

?? **Your application will now follow proper MVC architecture with Controllers handling ALL backend logic!**
