using System.ComponentModel.DataAnnotations;

namespace ArifTanPortfolio.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [EmailAddress]
        [StringLength(200)]
        public string Email { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? Subject { get; set; }
        
        [Required]
        [StringLength(2000)]
        public string Message { get; set; } = string.Empty;
        
        public DateTime DateSent { get; set; }
        public bool IsRead { get; set; }
        
        [StringLength(100)]
        public string? Company { get; set; }
        
        [StringLength(100)]
        public string? Phone { get; set; }
    }
}