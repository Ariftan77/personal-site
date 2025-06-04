using Microsoft.EntityFrameworkCore;
using ArifTanPortfolio.Models;

namespace ArifTanPortfolio.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        
        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Skill> Skills { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            
            // Configure BlogPost
            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.HasIndex(e => e.PublishedDate);
            });
            
            // Configure Project
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasIndex(e => e.SortOrder);
                entity.HasIndex(e => e.Category);
            });
            
            // Configure ContactMessage
            modelBuilder.Entity<ContactMessage>(entity =>
            {
                entity.HasIndex(e => e.DateSent);
            });
            
            // Configure Skill
            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.SortOrder);
            });
            
            // Seed initial data
            SeedData(modelBuilder);
        }
        
        private void SeedData(ModelBuilder modelBuilder)
        {
            // Seed Skills
            modelBuilder.Entity<Skill>().HasData(
                new Skill { Id = 1, Name = "C#", Proficiency = 9, Category = "Programming Languages", SortOrder = 1, Icon = "devicon-csharp-plain" },
                new Skill { Id = 2, Name = "ASP.NET Core", Proficiency = 9, Category = "Web Frameworks", SortOrder = 1, Icon = "devicon-dot-net-plain" },
                new Skill { Id = 3, Name = "JavaScript", Proficiency = 8, Category = "Programming Languages", SortOrder = 2, Icon = "devicon-javascript-plain" },
                new Skill { Id = 4, Name = "Python", Proficiency = 7, Category = "Programming Languages", SortOrder = 3, Icon = "devicon-python-plain" },
                new Skill { Id = 5, Name = "SQL Server", Proficiency = 8, Category = "Databases", SortOrder = 1, Icon = "devicon-microsoftsqlserver-plain" },
                new Skill { Id = 6, Name = "Azure", Proficiency = 7, Category = "Cloud Platforms", SortOrder = 1, Icon = "devicon-azure-plain" },
                new Skill { Id = 7, Name = "Docker", Proficiency = 7, Category = "DevOps", SortOrder = 1, Icon = "devicon-docker-plain" },
                new Skill { Id = 8, Name = "Git", Proficiency = 8, Category = "Version Control", SortOrder = 1, Icon = "devicon-git-plain" }
            );
            
            // Seed Featured Projects
            modelBuilder.Entity<Project>().HasData(
                new Project 
                { 
                    Id = 1, 
                    Name = "Advanced Logistics Platform", 
                    Description = "Comprehensive warehouse management system with real-time tracking and analytics",
                    LongDescription = "A full-featured WMS built with ASP.NET Core, handling inventory management, order processing, and real-time analytics for logistics operations.",
                    Technologies = "ASP.NET Core, Entity Framework, SQL Server, SignalR, Azure",
                    Category = "Enterprise Software",
                    IsFeatured = true,
                    SortOrder = 1,
                    StartDate = new DateTime(2023, 1, 1),
                    EndDate = new DateTime(2023, 8, 1)
                },
                new Project 
                { 
                    Id = 2, 
                    Name = "AI Document Processing", 
                    Description = "OCR and document automation system using machine learning",
                    LongDescription = "Automated document processing system that uses OCR and ML to extract and process information from various document types.",
                    Technologies = "Python, TensorFlow, Azure Cognitive Services, ASP.NET Core API",
                    Category = "Machine Learning",
                    IsFeatured = true,
                    SortOrder = 2,
                    StartDate = new DateTime(2023, 6, 1),
                    EndDate = new DateTime(2023, 12, 1)
                }
            );
        }
    }
}