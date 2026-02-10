using Microsoft.AspNetCore.Mvc;
using PropertyRentalSystem.Helpers;
using PropertyRentalSystem.Models;
using PropertyRentalSystem.Repositories.Interfaces;
using PropertyRentalSystem.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

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
                TempData["ErrorMessage"] = "Please login to view your bookings.";
                return RedirectToAction("Login", "WebAccount", new { returnUrl = "/WebBookings/MyBookings" });
            }

            var userId = HttpContext.Session.GetUserId();
            if (!userId.HasValue)
            {
                return RedirectToAction("Login", "WebAccount");
            }

            _logger.LogInformation("Viewing bookings for user {UserId}", userId);

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
                TempData["ErrorMessage"] = "Please login to view booking details.";
                return RedirectToAction("Login", "WebAccount", new { returnUrl = $"/WebBookings/Details/{id}" });
            }

            var booking = await _bookingRepository.GetBookingWithDetailsAsync(id);
            
            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found.";
                return RedirectToAction(nameof(MyBookings));
            }

            var userId = HttpContext.Session.GetUserId();
            if (!userId.HasValue || booking.GuestId != userId.Value)
            {
                TempData["ErrorMessage"] = "You are not authorized to view this booking.";
                return RedirectToAction(nameof(MyBookings));
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

            _logger.LogInformation("Cancelling booking {BookingId} for user {UserId}", id, userId);

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

            _logger.LogInformation("Creating booking for property {PropertyId} by user {UserId}", model.PropertyId, userId);

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
            [Display(Name = "Check-in Date")]
            public DateTime CheckInDate { get; set; }
            
            [Required]
            [Display(Name = "Check-out Date")]
            public DateTime CheckOutDate { get; set; }
            
            [Required]
            [Range(1, 100)]
            [Display(Name = "Number of Guests")]
            public int NumberOfGuests { get; set; }
            
            [StringLength(1000)]
            [Display(Name = "Special Requests")]
            public string? SpecialRequests { get; set; }
        }
    }
}
