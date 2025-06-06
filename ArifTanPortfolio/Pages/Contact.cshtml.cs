// Pages/Contact.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArifTanPortfolio.Models;
using ArifTanPortfolio.Models.ViewModels;
using ArifTanPortfolio.Services;

namespace ArifTanPortfolio.Pages
{
    public class ContactModel : PageModel
    {
        private readonly IEmailService _emailService;
        private readonly IPortfolioService _portfolioService;
        private readonly ILogger<ContactModel> _logger;

        public ContactModel(
            IEmailService emailService, 
            IPortfolioService portfolioService, 
            ILogger<ContactModel> logger)
        {
            _emailService = emailService;
            _portfolioService = portfolioService;
            _logger = logger;
        }

        [BindProperty]
        public ContactViewModel Contact { get; set; } = new();

        public void OnGet()
        {
            // Initialize empty contact form
        }

        public async Task<IActionResult> OnPostSendMessageAsync()
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => new { Field = x.Key, Errors = x.Value.Errors.Select(e => e.ErrorMessage) })
                    .ToList();

                _logger.LogWarning("Contact form validation failed. Errors: {Errors}", 
                    string.Join(", ", errors.SelectMany(e => e.Errors)));

                return new JsonResult(new { 
                    success = false, 
                    message = "Please check your input and try again.",
                    errors = errors
                });
            }

            try
            {
                // Save contact message to database
                var contactMessage = new ContactMessage
                {
                    Name = Contact.Name.Trim(),
                    Email = Contact.Email.Trim().ToLower(),
                    Subject = Contact.Subject?.Trim(),
                    Message = Contact.Message.Trim(),
                    Company = Contact.Company?.Trim(),
                    Phone = Contact.Phone?.Trim(),
                    DateSent = DateTime.UtcNow,
                    IsRead = false
                };

                await _portfolioService.SaveContactMessageAsync(contactMessage);
                _logger.LogInformation("Contact message saved to database from {Email}", contactMessage.Email);

                // Send email notification
                var emailSent = await _emailService.SendContactEmailAsync(Contact);

                if (emailSent)
                {
                    _logger.LogInformation("Contact form submitted and email sent successfully by {Email}", Contact.Email);
                    
                    return new JsonResult(new { 
                        success = true, 
                        message = "Thank you! Your message has been sent successfully. I'll get back to you within 24 hours." 
                    });
                }
                else
                {
                    _logger.LogWarning("Email sending failed for contact form submission by {Email}, but message was saved", Contact.Email);
                    
                    return new JsonResult(new { 
                        success = true, 
                        message = "Your message has been saved successfully. I'll get back to you soon! (Note: Email notification may be delayed)" 
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing contact form submission from {Email}", Contact.Email);
                
                return new JsonResult(new { 
                    success = false, 
                    message = "Sorry, there was an error processing your message. Please try again or contact me directly via email." 
                });
            }
        }

        // Helper method to validate business hours (optional)
        private bool IsWithinBusinessHours()
        {
            var now = DateTime.UtcNow.AddHours(7); // Convert to WIB (UTC+7)
            var hour = now.Hour;
            var dayOfWeek = now.DayOfWeek;
            
            // Business hours: 9 AM - 6 PM, Monday to Friday (WIB)
            return dayOfWeek >= DayOfWeek.Monday && 
                   dayOfWeek <= DayOfWeek.Friday && 
                   hour >= 9 && hour < 18;
        }

        // Get estimated response time based on current time
        private string GetEstimatedResponseTime()
        {
            if (IsWithinBusinessHours())
            {
                return "within 4 hours";
            }
            else
            {
                return "within 24 hours";
            }
        }
    }
}