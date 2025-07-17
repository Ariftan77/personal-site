using System.ComponentModel.DataAnnotations;

namespace ArifTanPortfolio.Models
{
    public class Skill
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Range(1, 10)]
        public int Proficiency { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [StringLength(100)]
        public string? Icon { get; set; }

        public int SortOrder { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsShowOnHomePage { get; set; } = false;
        public string ProficientcyLevel
        {
            get
            {
                return Proficiency switch
                {
                    >= 9 => "Expert",
                    >= 7 => "Advanced",
                    >= 5 => "Intermediate",
                    _ => "Beginner"
                };
            }
        }
    }
}