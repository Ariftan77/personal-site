using System.ComponentModel.DataAnnotations;

namespace ArifTanPortfolio.Models
{
    public class BlogPost
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        [StringLength(300)]
        public string Summary { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Slug { get; set; } = string.Empty;
        
        public DateTime PublishedDate { get; set; }
        public DateTime LastModified { get; set; }
        
        public bool IsPublished { get; set; }
        
        [StringLength(500)]
        public string Tags { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? FeaturedImage { get; set; }
        
        public int ReadTimeMinutes { get; set; }
    }
}