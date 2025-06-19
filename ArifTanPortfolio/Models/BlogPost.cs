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
        
        // Changed from Summary to Excerpt for better SEO naming
        [StringLength(500)]
        public string? Excerpt { get; set; } = string.Empty;
        
        [Required]
        [StringLength(200)]
        public string Slug { get; set; } = string.Empty;
        
        // Enhanced date tracking
        public DateTime? PublishedDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedDate { get; set; }
        
        public bool IsPublished { get; set; }
        
        [StringLength(500)]
        public string? Tags { get; set; } = string.Empty;
        
        // Added Category for better organization
        [StringLength(100)]
        public string? Category { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? FeaturedImage { get; set; }
        
        // Enhanced metrics
        public int ReadTimeMinutes { get; set; }
        public int ViewCount { get; set; } = 0;
        
        // SEO and metadata improvements
        [StringLength(160)]
        public string? MetaDescription { get; set; }
        
        [StringLength(200)]
        public string? MetaKeywords { get; set; }
        
        // Author information (for future multi-author support)
        [StringLength(100)]
        public string Author { get; set; } = "Arif Tan";
        
        [StringLength(200)]
        public string? AuthorEmail { get; set; } = "ariftan7788@gmail.com";
    }
}