using System.ComponentModel.DataAnnotations;

namespace ArifTanPortfolio.Models.ViewModels
{
    public class ContactViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Subject { get; set; }
        
        [Required(ErrorMessage = "Message is required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Message must be between 10 and 2000 characters")]
        public string Message { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Company { get; set; }
        
        [Phone]
        [StringLength(100)]
        public string? Phone { get; set; }
    }
}