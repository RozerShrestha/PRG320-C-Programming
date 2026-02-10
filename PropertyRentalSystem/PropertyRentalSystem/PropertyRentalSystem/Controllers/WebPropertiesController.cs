using Microsoft.AspNetCore.Mvc;
using PropertyRentalSystem.Helpers;
using PropertyRentalSystem.Models;
using PropertyRentalSystem.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;

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
            _logger.LogInformation("Browsing properties");

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
            _logger.LogInformation("Viewing property details {PropertyId}", id);

            var property = await _propertyRepository.GetPropertyWithDetailsAsync(id);
            
            if (property == null)
            {
                TempData["ErrorMessage"] = "Property not found.";
                return RedirectToAction(nameof(Index));
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
                TempData["ErrorMessage"] = "Please login to view your properties.";
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

            _logger.LogInformation("Viewing properties for host {UserId}", userId);

            var properties = await _propertyRepository.GetPropertiesByOwnerAsync(userId.Value);
            return View(properties);
        }

        // GET: /WebProperties/Create (Host only)
        [HttpGet]
        public IActionResult Create()
        {
            if (!HttpContext.Session.IsAuthenticated())
            {
                TempData["ErrorMessage"] = "Please login to list a property.";
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

            _logger.LogInformation("Creating property by user {UserId}", userId);

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

            _logger.LogInformation("Property created successfully {PropertyId}", property.Id);
            TempData["SuccessMessage"] = "Property listed successfully!";
            return RedirectToAction(nameof(MyProperties));
        }

        // GET: /WebProperties/Edit/5 (Host only)
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
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

            var property = await _propertyRepository.GetPropertyWithDetailsAsync(id);

            if (property == null)
            {
                TempData["ErrorMessage"] = "Property not found.";
                return RedirectToAction(nameof(MyProperties));
            }

            if (property.OwnerId != userId.Value)
            {
                TempData["ErrorMessage"] = "You are not authorized to edit this property.";
                return RedirectToAction(nameof(MyProperties));
            }

            var imageUrls = property.Images?
                .OrderBy(img => img.DisplayOrder)
                .Select(img => img.ImageUrl)
                .ToList() ?? new List<string>();

            var model = new PropertyEditViewModel
            {
                Id = property.Id,
                Title = property.Title,
                Description = property.Description,
                Address = property.Address,
                City = property.City,
                Country = property.Country,
                ZipCode = property.ZipCode,
                PricePerNight = property.PricePerNight,
                MaxGuests = property.MaxGuests,
                Bedrooms = property.Bedrooms,
                Bathrooms = property.Bathrooms,
                IsActive = property.IsActive,
                ImageUrls = imageUrls.Any() ? string.Join("\n", imageUrls) : string.Empty
            };

            return View(model);
        }

        // POST: /WebProperties/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PropertyEditViewModel model)
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

            var property = await _propertyRepository.GetPropertyWithDetailsAsync(id);

            if (property == null)
            {
                TempData["ErrorMessage"] = "Property not found.";
                return RedirectToAction(nameof(MyProperties));
            }

            if (property.OwnerId != userId.Value)
            {
                TempData["ErrorMessage"] = "You are not authorized to edit this property.";
                return RedirectToAction(nameof(MyProperties));
            }

            property.Title = model.Title;
            property.Description = model.Description;
            property.Address = model.Address;
            property.City = model.City;
            property.Country = model.Country;
            property.ZipCode = model.ZipCode;
            property.PricePerNight = model.PricePerNight;
            property.MaxGuests = model.MaxGuests;
            property.Bedrooms = model.Bedrooms;
            property.Bathrooms = model.Bathrooms;
            property.IsActive = model.IsActive;
            property.UpdatedAt = DateTime.UtcNow;

            // Handle image updates
            if (model.ImageUrls != null)
            {
                // Remove existing images
                property.Images.Clear();

                // Add new images
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
                }
            }

            await _propertyRepository.UpdateAsync(property);

            _logger.LogInformation("Property updated successfully: {PropertyId}", id);
            TempData["SuccessMessage"] = "Property updated successfully!";
            return RedirectToAction(nameof(MyProperties));
        }

        // View Models
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

        public class PropertyEditViewModel
        {
            public int Id { get; set; }

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

            public bool IsActive { get; set; }

            [Display(Name = "Image URLs (one per line)")]
            public string? ImageUrls { get; set; }
        }
    }
}
