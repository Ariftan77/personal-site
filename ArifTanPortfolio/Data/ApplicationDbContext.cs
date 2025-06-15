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
            modelBuilder.Entity<Skill>().HasData(
            // Programming Languages
            new Skill { Id = 1, Name = "C#", Proficiency = 9, Category = "Programming Languages", SortOrder = 1, Icon = "devicon-csharp-plain", IsVisible = true },
            new Skill { Id = 2, Name = "JavaScript", Proficiency = 8, Category = "Programming Languages", SortOrder = 2, Icon = "devicon-javascript-plain", IsVisible = true },
            new Skill { Id = 3, Name = "Python", Proficiency = 8, Category = "Programming Languages", SortOrder = 3, Icon = "devicon-python-plain", IsVisible = true },
            new Skill { Id = 4, Name = "Dart", Proficiency = 6, Category = "Programming Languages", SortOrder = 4, Icon = "devicon-dart-plain", IsVisible = true },

            // Frameworks (includes web frameworks and frontend frameworks)
            new Skill { Id = 5, Name = "ASP.NET Core", Proficiency = 9, Category = "Frameworks", SortOrder = 1, Icon = "devicon-dot-net-plain", IsVisible = true },
            new Skill { Id = 6, Name = "Node.js", Proficiency = 8, Category = "Frameworks", SortOrder = 2, Icon = "devicon-nodejs-plain", IsVisible = true },
            new Skill { Id = 7, Name = "Flutter", Proficiency = 6, Category = "Frameworks", SortOrder = 3, Icon = "devicon-flutter-plain", IsVisible = true },
            new Skill { Id = 8, Name = "Bootstrap", Proficiency = 8, Category = "Frameworks", SortOrder = 4, Icon = "devicon-bootstrap-plain", IsVisible = true },
            new Skill { Id = 9, Name = "ASP.NET MVC", Proficiency = 9, Category = "Frameworks", SortOrder = 5, Icon = "devicon-dot-net-plain", IsVisible = true },
            new Skill { Id = 10, Name = "Razor Pages", Proficiency = 8, Category = "Frameworks", SortOrder = 6, Icon = "devicon-dot-net-plain", IsVisible = true },
            new Skill { Id = 11, Name = "React", Proficiency = 6, Category = "Frameworks", SortOrder = 7, Icon = "devicon-react-original", IsVisible = true, Description = "Maintenance experience" },
            new Skill { Id = 12, Name = "SignalR", Proficiency = 8, Category = "Frameworks", SortOrder = 8, Icon = "devicon-dot-net-plain", IsVisible = true },
            new Skill { Id = 13, Name = "jQuery", Proficiency = 8, Category = "Frameworks", SortOrder = 9, Icon = "devicon-jquery-plain", IsVisible = true },
            new Skill { Id = 14, Name = "HTML/CSS", Proficiency = 8, Category = "Frameworks", SortOrder = 10, Icon = "devicon-html5-plain", IsVisible = true },
            new Skill { Id = 15, Name = "AJAX", Proficiency = 8, Category = "Frameworks", SortOrder = 11, Icon = "devicon-javascript-plain", IsVisible = true },

            // Cloud & DevOps
            new Skill { Id = 16, Name = "AWS", Proficiency = 8, Category = "Cloud & DevOps", SortOrder = 1, Icon = "devicon-amazonwebservices-plain", IsVisible = true },
            new Skill { Id = 17, Name = "Git", Proficiency = 9, Category = "Cloud & DevOps", SortOrder = 2, Icon = "devicon-git-plain", IsVisible = true },
            new Skill { Id = 18, Name = "Docker", Proficiency = 7, Category = "Cloud & DevOps", SortOrder = 3, Icon = "devicon-docker-plain", IsVisible = true },
            new Skill { Id = 19, Name = "Power Automate", Proficiency = 8, Category = "Cloud & DevOps", SortOrder = 4, Icon = "fas fa-bolt", IsVisible = true },
            new Skill { Id = 20, Name = "AWS S3", Proficiency = 8, Category = "Cloud & DevOps", SortOrder = 5, Icon = "devicon-amazonwebservices-plain", IsVisible = true },
            new Skill { Id = 21, Name = "AWS EC2", Proficiency = 7, Category = "Cloud & DevOps", SortOrder = 6, Icon = "devicon-amazonwebservices-plain", IsVisible = true },
            new Skill { Id = 22, Name = "AWS Lightsail", Proficiency = 7, Category = "Cloud & DevOps", SortOrder = 7, Icon = "devicon-amazonwebservices-plain", IsVisible = true },
            new Skill { Id = 23, Name = "Azure", Proficiency = 7, Category = "Cloud & DevOps", SortOrder = 8, Icon = "devicon-azure-plain", IsVisible = true },
            new Skill { Id = 24, Name = "Azure DevOps", Proficiency = 7, Category = "Cloud & DevOps", SortOrder = 9, Icon = "devicon-azure-plain", IsVisible = true },

            // Databases
            new Skill { Id = 25, Name = "SQL Server", Proficiency = 9, Category = "Databases", SortOrder = 1, Icon = "devicon-microsoftsqlserver-plain", IsVisible = true },
            new Skill { Id = 26, Name = "PostgreSQL", Proficiency = 9, Category = "Databases", SortOrder = 2, Icon = "devicon-postgresql-plain", IsVisible = true },
            new Skill { Id = 27, Name = "Entity Framework Core", Proficiency = 9, Category = "Databases", SortOrder = 3, Icon = "devicon-dot-net-plain", IsVisible = true },
            new Skill { Id = 28, Name = "SQLite", Proficiency = 9, Category = "Databases", SortOrder = 4, Icon = "devicon-sqlite-plain", IsVisible = true }
            ); 
            
            // Seed Featured Projects
            modelBuilder.Entity<Project>().HasData(
                new Project 
                { 
                    Id = 1, 
                    Name = "Enterprise Warehouse Management System", 
                    Description = "Comprehensive WMS handling inventory management, order processing, and real-time analytics for logistics operations",
                    LongDescription = "Led development of a full-scale warehouse management system that processes thousands of transactions daily across multiple warehouse locations. The system handles complex inventory tracking, order fulfillment workflows, and provides real-time analytics for operational decision-making. Built with scalable architecture to support business growth and integration with existing ERP systems.",
                    Technologies = "ASP.NET Core, PostgreSQL, SignalR, Entity Framework Core, AWS, JavaScript, Tailwind",
                    Category = "Enterprise Software",
                    IsFeatured = true,
                    SortOrder = 1,
                    StartDate = new DateTime(2025, 5, 13),
                    EndDate = new DateTime(2025, 6, 15),
                    Challenges = "Real-time inventory synchronization across multiple locations, performance optimization with large datasets, complex business logic implementation, integration with legacy ERP systems, ensuring 99.9% uptime for critical operations",
                    Solutions = "Implemented SignalR for real-time updates, optimized database queries with strategic indexing, applied clean architecture with domain-driven design, created robust API layer for integrations, implemented comprehensive logging and monitoring",
                    LessonsLearned = "Importance of scalable architecture design, performance optimization strategies for enterprise applications, effective team collaboration in complex projects, domain-driven design principles, real-time system challenges and solutions",
                    ImageGallery = "",
                    FeaturedImage = null,
                    LiveUrl = null,
                    GitHubUrl = null
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