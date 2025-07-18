using ArifTanPortfolio.Models;

namespace ArifTanPortfolio.Tests.TestHelpers
{
    public static class TestDataBuilder
    {
        public static List<Skill> CreateTestSkills()
        {
            return new List<Skill>
            {
                new Skill 
                { 
                    Id = 1, 
                    Name = "C#", 
                    Proficiency = 9, 
                    Category = "Programming Languages", 
                    SortOrder = 1, 
                    Icon = "devicon-csharp-plain", 
                    IsVisible = true, 
                    IsShowOnHomePage = true 
                },
                new Skill 
                { 
                    Id = 2, 
                    Name = "JavaScript", 
                    Proficiency = 8, 
                    Category = "Programming Languages", 
                    SortOrder = 2, 
                    Icon = "devicon-javascript-plain", 
                    IsVisible = true, 
                    IsShowOnHomePage = true 
                },
                new Skill 
                { 
                    Id = 3, 
                    Name = "Python", 
                    Proficiency = 7, 
                    Category = "Programming Languages", 
                    SortOrder = 3, 
                    Icon = "devicon-python-plain", 
                    IsVisible = true, 
                    IsShowOnHomePage = false 
                },
                new Skill 
                { 
                    Id = 4, 
                    Name = "Hidden Skill", 
                    Proficiency = 6, 
                    Category = "Programming Languages", 
                    SortOrder = 4, 
                    Icon = "devicon-test-plain", 
                    IsVisible = false, 
                    IsShowOnHomePage = false 
                }
            };
        }

        public static List<Project> CreateTestProjects()
        {
            return new List<Project>
            {
                new Project
                {
                    Id = 1,
                    Name = "Featured Project 1",
                    Description = "Test featured project",
                    LongDescription = "Long description for featured project",
                    Technologies = "C#, ASP.NET Core, PostgreSQL",
                    Category = "Enterprise Software",
                    IsFeatured = true,
                    SortOrder = 1,
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 6, 30)
                },
                new Project
                {
                    Id = 2,
                    Name = "Non-Featured Project",
                    Description = "Test non-featured project",
                    LongDescription = "Long description for non-featured project",
                    Technologies = "JavaScript, Node.js",
                    Category = "Web Development",
                    IsFeatured = false,
                    SortOrder = 2,
                    StartDate = new DateTime(2024, 2, 1),
                    EndDate = new DateTime(2024, 7, 31)
                }
            };
        }

        public static List<BlogPost> CreateTestBlogPosts()
        {
            return new List<BlogPost>
            {
                new BlogPost
                {
                    Id = 1,
                    Title = "Published Blog Post 1",
                    Slug = "published-blog-post-1",
                    Content = "Test content for blog post 1",
                    Excerpt = "Test excerpt",
                    Category = "Technical",
                    Tags = "C#, ASP.NET Core",
                    IsPublished = true,
                    PublishedDate = DateTime.UtcNow.AddDays(-5),
                    CreatedDate = DateTime.UtcNow.AddDays(-10),
                    ReadTimeMinutes = 5,
                    ViewCount = 100,
                    Author = "Arif Tan",
                    AuthorEmail = "ariftan7788@gmail.com"
                },
                new BlogPost
                {
                    Id = 2,
                    Title = "Published Blog Post 2",
                    Slug = "published-blog-post-2",
                    Content = "Test content for blog post 2",
                    Excerpt = "Test excerpt 2",
                    Category = "Technical",
                    Tags = "JavaScript, Node.js",
                    IsPublished = true,
                    PublishedDate = DateTime.UtcNow.AddDays(-3),
                    CreatedDate = DateTime.UtcNow.AddDays(-8),
                    ReadTimeMinutes = 8,
                    ViewCount = 50,
                    Author = "Arif Tan",
                    AuthorEmail = "ariftan7788@gmail.com"
                },
                new BlogPost
                {
                    Id = 3,
                    Title = "Draft Blog Post",
                    Slug = "draft-blog-post",
                    Content = "Test content for draft post",
                    Excerpt = "Test excerpt for draft",
                    Category = "Technical",
                    Tags = "Draft",
                    IsPublished = false,
                    PublishedDate = null,
                    CreatedDate = DateTime.UtcNow.AddDays(-2),
                    ReadTimeMinutes = 10,
                    ViewCount = 0,
                    Author = "Arif Tan",
                    AuthorEmail = "ariftan7788@gmail.com"
                }
            };
        }

        public static ContactMessage CreateValidContactMessage()
        {
            return new ContactMessage
            {
                Name = "Test User",
                Email = "test@example.com",
                Subject = "Test Subject",
                Message = "Test message content",
                Company = "Test Company",
                Phone = "+1234567890",
                DateSent = DateTime.UtcNow,
                IsRead = false
            };
        }

        public static ContactMessage CreateInvalidContactMessage()
        {
            return new ContactMessage
            {
                Name = "",
                Email = "invalid-email",
                Subject = "",
                Message = "",
                Company = "",
                Phone = "",
                DateSent = DateTime.UtcNow,
                IsRead = false
            };
        }
    }
}