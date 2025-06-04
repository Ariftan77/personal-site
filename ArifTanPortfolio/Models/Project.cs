using System.ComponentModel.DataAnnotations;

namespace ArifTanPortfolio.Models
{
    public class Project
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Description { get; set; } = string.Empty;
        
        public string LongDescription { get; set; } = string.Empty;
        
        [Required]
        public string Technologies { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? LiveUrl { get; set; }
        
        [StringLength(500)]
        public string? GitHubUrl { get; set; }
        
        [StringLength(500)]
        public string? FeaturedImage { get; set; }
        
        public string ImageGallery { get; set; } = string.Empty;
        
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        
        public bool IsFeatured { get; set; }
        public int SortOrder { get; set; }
        
        [StringLength(100)]
        public string Category { get; set; } = string.Empty;
        
        public string Challenges { get; set; } = string.Empty;
        public string Solutions { get; set; } = string.Empty;
        public string LessonsLearned { get; set; } = string.Empty;
    }
}