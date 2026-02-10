using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using PropertyRentalSystem.Services.Interfaces;

namespace PropertyRentalSystem.Services.Implementations
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var emailSettings = _configuration.GetSection("EmailSettings");
                
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(
                    emailSettings["SenderName"], 
                    emailSettings["SenderEmail"]
                ));
                message.To.Add(MailboxAddress.Parse(toEmail));
                message.Subject = subject;

                var builder = new BodyBuilder
                {
                    HtmlBody = body
                };
                message.Body = builder.ToMessageBody();

                using var client = new SmtpClient();
                
                // Connect to SMTP server
                await client.ConnectAsync(
                    emailSettings["SmtpServer"],
                    int.Parse(emailSettings["SmtpPort"] ?? "587"),
                    SecureSocketOptions.StartTls
                );

                // Authenticate
                if (bool.Parse(emailSettings["RequireAuthentication"] ?? "true"))
                {
                    await client.AuthenticateAsync(
                        emailSettings["SmtpUsername"],
                        emailSettings["SmtpPassword"]
                    );
                }

                // Send email
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                _logger.LogInformation($"Email sent successfully to {toEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send email to {toEmail}: {ex.Message}");
                // In production, you might want to throw or handle this differently
                // For now, we'll log and continue
            }
        }

        public async Task SendBookingConfirmationAsync(
            string toEmail, string guestName, string propertyTitle, 
            DateTime checkIn, DateTime checkOut, decimal totalPrice)
        {
            var subject = "Booking Confirmation - Property Rental System";
            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #007bff; color: white; padding: 20px; text-align: center; }}
                        .content {{ background-color: #f8f9fa; padding: 20px; }}
                        .details {{ background-color: white; padding: 15px; margin: 15px 0; border-radius: 5px; }}
                        .footer {{ text-align: center; padding: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Booking Confirmed!</h1>
                        </div>
                        <div class='content'>
                            <p>Dear {guestName},</p>
                            <p>Your booking has been confirmed. Here are the details:</p>
                            <div class='details'>
                                <p><strong>Property:</strong> {propertyTitle}</p>
                                <p><strong>Check-in:</strong> {checkIn:MMMM dd, yyyy}</p>
                                <p><strong>Check-out:</strong> {checkOut:MMMM dd, yyyy}</p>
                                <p><strong>Total Price:</strong> ${totalPrice:F2}</p>
                            </div>
                            <p>We look forward to hosting you!</p>
                        </div>
                        <div class='footer'>
                            <p>Property Rental System | © 2026 All rights reserved</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            await SendEmailAsync(toEmail, subject, body);
        }

        public async Task SendBookingCancellationAsync(
            string toEmail, string guestName, string propertyTitle, int bookingId)
        {
            var subject = "Booking Cancelled - Property Rental System";
            var body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background-color: #dc3545; color: white; padding: 20px; text-align: center; }}
                        .content {{ background-color: #f8f9fa; padding: 20px; }}
                        .footer {{ text-align: center; padding: 20px; color: #666; font-size: 12px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h1>Booking Cancelled</h1>
                        </div>
                        <div class='content'>
                            <p>Dear {guestName},</p>
                            <p>Your booking (ID: {bookingId}) for <strong>{propertyTitle}</strong> has been cancelled.</p>
                            <p>If you have any questions, please contact our support team.</p>
                        </div>
                        <div class='footer'>
                            <p>Property Rental System | © 2026 All rights reserved</p>
                        </div>
                    </div>
                </body>
                </html>
            ";

            await SendEmailAsync(toEmail, subject, body);
        }
    }
}
