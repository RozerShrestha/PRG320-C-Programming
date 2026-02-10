using PropertyRentalSystem.Models;
using PropertyRentalSystem.Repositories.Interfaces;
using PropertyRentalSystem.Services.Interfaces;

namespace PropertyRentalSystem.Services.Implementations
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IPropertyRepository _propertyRepository;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IEmailService _emailService;
        private readonly IUserRepository _userRepository;

        public BookingService(
            IBookingRepository bookingRepository,
            IPropertyRepository propertyRepository,
            IPaymentRepository paymentRepository,
            IEmailService emailService,
            IUserRepository userRepository)
        {
            _bookingRepository = bookingRepository;
            _propertyRepository = propertyRepository;
            _paymentRepository = paymentRepository;
            _emailService = emailService;
            _userRepository = userRepository;
        }

        public async Task<(bool Success, string Message, Booking? Booking)> CreateBookingAsync(
            int propertyId, int guestId, DateTime checkIn, DateTime checkOut, 
            int numberOfGuests, string? specialRequests)
        {
            // Validate dates
            if (checkIn < DateTime.Today)
            {
                return (false, "Check-in date cannot be in the past", null);
            }

            if (checkOut <= checkIn)
            {
                return (false, "Check-out date must be after check-in date", null);
            }

            // Get property
            var property = await _propertyRepository.GetByIdAsync(propertyId);
            if (property == null)
            {
                return (false, "Property not found", null);
            }

            if (!property.IsActive)
            {
                return (false, "Property is not available for booking", null);
            }

            // Check max guests
            if (numberOfGuests > property.MaxGuests)
            {
                return (false, $"Property can accommodate maximum {property.MaxGuests} guests", null);
            }

            // Check for date conflicts
            var hasConflict = await HasDateConflictAsync(propertyId, checkIn, checkOut);
            if (hasConflict)
            {
                return (false, "Selected dates are not available. Property is already booked for these dates.", null);
            }

            // Calculate total price
            var nights = (checkOut - checkIn).Days;
            var totalPrice = property.PricePerNight * nights;

            // Create booking
            var booking = new Booking
            {
                PropertyId = propertyId,
                GuestId = guestId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                NumberOfGuests = numberOfGuests,
                TotalPrice = totalPrice,
                SpecialRequests = specialRequests,
                Status = BookingStatus.Pending,
                CreatedAt = DateTime.UtcNow
            };

            await _bookingRepository.AddAsync(booking);

            // Create payment record
            var payment = new Payment
            {
                BookingId = booking.Id,
                Amount = totalPrice,
                Status = PaymentStatus.Unpaid,
                CreatedAt = DateTime.UtcNow
            };

            await _paymentRepository.AddAsync(payment);

            // Send confirmation email
            try
            {
                var guest = await _userRepository.GetByIdAsync(guestId);
                if (guest != null)
                {
                    await _emailService.SendBookingConfirmationAsync(
                        guest.Email,
                        $"{guest.FirstName} {guest.LastName}",
                        property.Title,
                        checkIn,
                        checkOut,
                        totalPrice
                    );
                }
            }
            catch
            {
                // Email sending failed, but booking was created
            }

            return (true, "Booking created successfully", booking);
        }

        public async Task<bool> HasDateConflictAsync(int propertyId, DateTime checkIn, DateTime checkOut, int? excludeBookingId = null)
        {
            return await _bookingRepository.HasDateConflictAsync(propertyId, checkIn, checkOut, excludeBookingId);
        }

        public async Task<(bool Success, string Message)> CancelBookingAsync(int bookingId, int userId)
        {
            var booking = await _bookingRepository.GetBookingWithDetailsAsync(bookingId);
            
            if (booking == null)
            {
                return (false, "Booking not found");
            }

            if (booking.GuestId != userId && booking.Property.OwnerId != userId)
            {
                return (false, "You are not authorized to cancel this booking");
            }

            if (booking.Status == BookingStatus.Cancelled)
            {
                return (false, "Booking is already cancelled");
            }

            if (booking.Status == BookingStatus.Completed)
            {
                return (false, "Cannot cancel a completed booking");
            }

            // Update booking status
            booking.Status = BookingStatus.Cancelled;
            booking.UpdatedAt = DateTime.UtcNow;
            await _bookingRepository.UpdateAsync(booking);

            // Update payment if exists
            if (booking.Payment != null && booking.Payment.Status == PaymentStatus.Paid)
            {
                booking.Payment.Status = PaymentStatus.Refunded;
                await _paymentRepository.UpdateAsync(booking.Payment);
            }

            // Send cancellation email
            try
            {
                await _emailService.SendBookingCancellationAsync(
                    booking.Guest.Email,
                    $"{booking.Guest.FirstName} {booking.Guest.LastName}",
                    booking.Property.Title,
                    booking.Id
                );
            }
            catch
            {
                // Email sending failed
            }

            return (true, "Booking cancelled successfully");
        }

        public async Task<(bool Success, string Message)> ConfirmBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            
            if (booking == null)
            {
                return (false, "Booking not found");
            }

            if (booking.Status != BookingStatus.Pending)
            {
                return (false, "Only pending bookings can be confirmed");
            }

            booking.Status = BookingStatus.Confirmed;
            booking.UpdatedAt = DateTime.UtcNow;
            await _bookingRepository.UpdateAsync(booking);

            return (true, "Booking confirmed successfully");
        }

        public async Task<(bool Success, string Message)> CompleteBookingAsync(int bookingId)
        {
            var booking = await _bookingRepository.GetByIdAsync(bookingId);
            
            if (booking == null)
            {
                return (false, "Booking not found");
            }

            if (booking.Status != BookingStatus.Confirmed)
            {
                return (false, "Only confirmed bookings can be completed");
            }

            if (booking.CheckOutDate > DateTime.UtcNow)
            {
                return (false, "Cannot complete booking before check-out date");
            }

            booking.Status = BookingStatus.Completed;
            booking.UpdatedAt = DateTime.UtcNow;
            await _bookingRepository.UpdateAsync(booking);

            return (true, "Booking completed successfully");
        }

        public async Task<Dictionary<int, List<DateTime>>> GetAvailabilityCalendarAsync(int propertyId, int month, int year)
        {
            var bookedDates = await _bookingRepository.GetBookedDatesAsync(propertyId, month, year);
            
            var calendar = new Dictionary<int, List<DateTime>>();
            var daysInMonth = DateTime.DaysInMonth(year, month);

            for (int day = 1; day <= daysInMonth; day++)
            {
                var date = new DateTime(year, month, day);
                var week = GetWeekOfMonth(date);

                if (!calendar.ContainsKey(week))
                {
                    calendar[week] = new List<DateTime>();
                }

                calendar[week].Add(date);
            }

            return calendar;
        }

        private int GetWeekOfMonth(DateTime date)
        {
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            return (date.Day + (int)firstDayOfMonth.DayOfWeek - 1) / 7 + 1;
        }
    }
}
