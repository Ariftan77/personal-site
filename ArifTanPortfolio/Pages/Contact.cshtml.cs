using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArifTanPortfolio.Models;
using ArifTanPortfolio.Models.ViewModels;
using ArifTanPortfolio.Services;
using System.ComponentModel.DataAnnotations;

namespace ArifTanPortfolio.Pages
{
    [ValidateAntiForgeryToken]
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

        [TempData]
        public string? StatusMessage { get; set; }

        [TempData]
        public string? StatusType { get; set; } // "success" or "error"

        public void OnGet()
        {
            // Initialize empty contact form
            _logger.LogDebug("Contact page loaded");
        }

        public async Task<IActionResult> OnPostSendMessageAsync()
        {
            _logger.LogInformation("Contact form submission started from {Email}", Contact.Email);

            // Validate model state
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors.Select(e => e.ErrorMessage))
                    .ToList();

                _logger.LogWarning("Contact form validation failed. Errors: {Errors}", 
                    string.Join(", ", errors));

                if (IsAjaxRequest())
                {
                    return new JsonResult(new { 
                        success = false, 
                        message = "Please check your input and try again.",
                        errors = errors
                    });
                }

                StatusMessage = "Please correct the errors and try again.";
                StatusType = "error";
                return Page();
            }

            try
            {
                // Additional server-side validation
                var validationResult = ValidateContactSubmission();
                if (!validationResult.IsValid)
                {
                    _logger.LogWarning("Contact form additional validation failed: {Error}", 
                        validationResult.ErrorMessage);

                    if (IsAjaxRequest())
                    {
                        return new JsonResult(new { 
                            success = false, 
                            message = validationResult.ErrorMessage
                        });
                    }

                    StatusMessage = validationResult.ErrorMessage;
                    StatusType = "error";
                    return Page();
                }

                // Save contact message to database
                var contactMessage = new ContactMessage
                {
                    Name = Contact.Name.Trim(),
                    Email = Contact.Email.Trim().ToLowerInvariant(),
                    Subject = Contact.Subject?.Trim(),
                    Message = Contact.Message.Trim(),
                    Company = Contact.Company?.Trim(),
                    Phone = Contact.Phone?.Trim(),
                    DateSent = DateTime.UtcNow,
                    IsRead = false
                };

                try
                {
                    await _portfolioService.SaveContactMessageAsync(contactMessage);
                    _logger.LogInformation("Contact message saved successfully with ID {MessageId}", 
                        contactMessage.Id);
                }
                catch (InvalidOperationException ex) when (ex.Message.Contains("Too many messages"))
                {
                    // Handle rate limiting
                    _logger.LogWarning("Rate limit exceeded for {Email}", Contact.Email);
                    
                    var rateLimitMessage = "You've sent several messages recently. Please wait before sending another message.";
                    
                    if (IsAjaxRequest())
                    {
                        return new JsonResult(new { success = false, message = rateLimitMessage });
                    }
                    
                    StatusMessage = rateLimitMessage;
                    StatusType = "error";
                    return Page();
                }
                catch (ValidationException ex)
                {
                    _logger.LogWarning("Contact message validation failed: {Error}", ex.Message);
                    
                    if (IsAjaxRequest())
                    {
                        return new JsonResult(new { 
                            success = false, 
                            message = "Please check your information and try again."
                        });
                    }
                    
                    StatusMessage = "Please check your information and try again.";
                    StatusType = "error";
                    return Page();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to save contact message");
                    
                    if (IsAjaxRequest())
                    {
                        return new JsonResult(new { 
                            success = false, 
                            message = "Unable to save your message. Please try again."
                        });
                    }
                    
                    StatusMessage = "Unable to save your message. Please try again.";
                    StatusType = "error";
                    return Page();
                }

                // Send email notification
                var emailSent = await _emailService.SendContactEmailAsync(Contact);
                
                string successMessage;
                if (emailSent)
                {
                    _logger.LogInformation("Contact form submitted and email sent successfully by {Email}", 
                        Contact.Email);
                    successMessage = "Thank you! Your message has been sent successfully. I'll get back to you within 24-48 hours.";
                }
                else
                {
                    _logger.LogWarning("Contact message saved but email failed for {Email}", Contact.Email);
                    successMessage = "Your message has been saved successfully. However, there was an issue sending the notification email. I'll still get back to you soon!";
                }

                if (IsAjaxRequest())
                {
                    return new JsonResult(new { 
                        success = true, 
                        message = successMessage
                    });
                }

                StatusMessage = successMessage;
                StatusType = "success";
                
                // Clear the form after successful submission
                Contact = new ContactViewModel();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error processing contact form from {Email}", Contact.Email);

                const string errorMessage = "An unexpected error occurred. Please try again later or contact me directly via email.";

                if (IsAjaxRequest())
                {
                    return new JsonResult(new { 
                        success = false, 
                        message = errorMessage
                    });
                }

                StatusMessage = errorMessage;
                StatusType = "error";
                return Page();
            }
        }

        // =======================================================================================
        // HELPER METHODS
        // =======================================================================================

        private ValidationResult ValidateContactSubmission()
        {
            try
            {
                // Check for suspicious content (simple spam detection)
                var suspiciousPatterns = new[]
                {
                    "http://", "https://", "www.", "viagra", "casino", "loan", "bitcoin",
                    "cryptocurrency", "investment opportunity", "make money fast", "click here"
                };

                var messageContent = (Contact.Name + " " + Contact.Message + " " + (Contact.Subject ?? "")).ToLowerInvariant();
                
                foreach (var pattern in suspiciousPatterns)
                {
                    if (messageContent.Contains(pattern))
                    {
                        return new ValidationResult
                        {
                            IsValid = false,
                            ErrorMessage = "Your message contains content that appears to be spam. Please revise and try again."
                        };
                    }
                }

                // Check message length (additional validation)
                if (Contact.Message.Trim().Length < 10)
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        ErrorMessage = "Your message is too short. Please provide more details."
                    };
                }

                if (Contact.Message.Length > 2000)
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        ErrorMessage = "Your message is too long. Please keep it under 2000 characters."
                    };
                }

                // Validate email domain (basic check)
                if (Contact.Email.Contains("test.com") || Contact.Email.Contains("example.com") || 
                    Contact.Email.Contains("tempmail") || Contact.Email.Contains("10minutemail"))
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        ErrorMessage = "Please provide a valid email address."
                    };
                }

                // Check for obviously fake names
                var fakeName = Contact.Name.Trim().ToLowerInvariant();
                if (fakeName == "test" || fakeName == "testing" || fakeName == "john doe" || fakeName.Length < 2)
                {
                    return new ValidationResult
                    {
                        IsValid = false,
                        ErrorMessage = "Please provide your real name."
                    };
                }

                return new ValidationResult { IsValid = true };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating contact submission");
                return new ValidationResult
                {
                    IsValid = false,
                    ErrorMessage = "Validation error occurred"
                };
            }
        }

        private bool IsAjaxRequest()
        {
            return Request.Headers.XRequestedWith == "XMLHttpRequest";
        }
    }

    // Helper class for validation results
    public class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
