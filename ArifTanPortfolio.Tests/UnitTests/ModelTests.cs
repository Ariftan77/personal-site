using FluentAssertions;
using ArifTanPortfolio.Models;
using ArifTanPortfolio.Tests.TestHelpers;

namespace ArifTanPortfolio.Tests.UnitTests
{
    public class ModelTests
    {
        [Fact]
        public void Skill_ProficientcyLevel_ShouldReturnCorrectLevels()
        {
            // Arrange & Act & Assert
            var expertSkill = new Skill { Proficiency = 9 };
            expertSkill.ProficientcyLevel.Should().Be("Expert");

            var advancedSkill = new Skill { Proficiency = 8 };
            advancedSkill.ProficientcyLevel.Should().Be("Advanced");

            var intermediateSkill = new Skill { Proficiency = 6 };
            intermediateSkill.ProficientcyLevel.Should().Be("Intermediate");

            var beginnerSkill = new Skill { Proficiency = 3 };
            beginnerSkill.ProficientcyLevel.Should().Be("Beginner");
        }

        [Fact]
        public void Skill_ProficientcyLevel_BoundaryValues_ShouldReturnCorrectLevels()
        {
            // Arrange & Act & Assert
            var expertBoundary = new Skill { Proficiency = 9 };
            expertBoundary.ProficientcyLevel.Should().Be("Expert");

            var advancedBoundary = new Skill { Proficiency = 7 };
            advancedBoundary.ProficientcyLevel.Should().Be("Advanced");

            var intermediateBoundary = new Skill { Proficiency = 5 };
            intermediateBoundary.ProficientcyLevel.Should().Be("Intermediate");

            var beginnerBoundary = new Skill { Proficiency = 4 };
            beginnerBoundary.ProficientcyLevel.Should().Be("Beginner");

            var minimumValue = new Skill { Proficiency = 1 };
            minimumValue.ProficientcyLevel.Should().Be("Beginner");
        }

        [Fact]
        public void BlogPost_DefaultValues_ShouldBeSetCorrectly()
        {
            // Arrange & Act
            var blogPost = new BlogPost
            {
                Title = "Test Blog Post",
                Slug = "test-blog-post",
                Content = "Test content"
            };

            // Assert
            blogPost.Title.Should().Be("Test Blog Post");
            blogPost.Slug.Should().Be("test-blog-post");
            blogPost.Content.Should().Be("Test content");
            blogPost.IsPublished.Should().BeFalse(); // Default should be false
            blogPost.ViewCount.Should().Be(0); // Default should be 0
        }

        [Fact]
        public void Project_DefaultValues_ShouldBeSetCorrectly()
        {
            // Arrange & Act
            var project = new Project
            {
                Name = "Test Project",
                Description = "Test description"
            };

            // Assert
            project.Name.Should().Be("Test Project");
            project.Description.Should().Be("Test description");
            project.IsFeatured.Should().BeFalse(); // Default should be false
            project.SortOrder.Should().Be(0); // Default should be 0
        }

        [Fact]
        public void ContactMessage_DefaultValues_ShouldBeSetCorrectly()
        {
            // Arrange & Act
            var contactMessage = new ContactMessage
            {
                Name = "Test User",
                Email = "test@example.com",
                Message = "Test message"
            };

            // Assert
            contactMessage.Name.Should().Be("Test User");
            contactMessage.Email.Should().Be("test@example.com");
            contactMessage.Message.Should().Be("Test message");
            contactMessage.IsRead.Should().BeFalse(); // Default should be false
        }

        [Fact]
        public void Skill_DefaultValues_ShouldBeSetCorrectly()
        {
            // Arrange & Act
            var skill = new Skill
            {
                Name = "Test Skill",
                Proficiency = 8,
                Category = "Test Category"
            };

            // Assert
            skill.Name.Should().Be("Test Skill");
            skill.Proficiency.Should().Be(8);
            skill.Category.Should().Be("Test Category");
            skill.IsVisible.Should().BeTrue(); // Default should be true
            skill.IsShowOnHomePage.Should().BeFalse(); // Default should be false
            skill.SortOrder.Should().Be(0); // Default should be 0
        }

        [Fact]
        public void BlogPost_WithTags_ShouldHandleTagsCorrectly()
        {
            // Arrange & Act
            var blogPost = new BlogPost
            {
                Title = "Test Blog Post",
                Slug = "test-blog-post",
                Content = "Test content",
                Tags = "C#, ASP.NET Core, Testing"
            };

            // Assert
            blogPost.Tags.Should().Be("C#, ASP.NET Core, Testing");
            blogPost.Tags.Should().Contain("C#");
            blogPost.Tags.Should().Contain("ASP.NET Core");
            blogPost.Tags.Should().Contain("Testing");
        }

        [Fact]
        public void Project_WithTechnologies_ShouldHandleTechnologiesCorrectly()
        {
            // Arrange & Act
            var project = new Project
            {
                Name = "Test Project",
                Description = "Test description",
                Technologies = "C#, ASP.NET Core, PostgreSQL, Docker"
            };

            // Assert
            project.Technologies.Should().Be("C#, ASP.NET Core, PostgreSQL, Docker");
            project.Technologies.Should().Contain("C#");
            project.Technologies.Should().Contain("ASP.NET Core");
            project.Technologies.Should().Contain("PostgreSQL");
            project.Technologies.Should().Contain("Docker");
        }

        [Fact]
        public void BlogPost_DateHandling_ShouldWorkCorrectly()
        {
            // Arrange
            var publishedDate = new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc);
            var createdDate = new DateTime(2023, 12, 1, 10, 0, 0, DateTimeKind.Utc);

            // Act
            var blogPost = new BlogPost
            {
                Title = "Test Blog Post",
                Slug = "test-blog-post",
                Content = "Test content",
                PublishedDate = publishedDate,
                CreatedDate = createdDate,
                IsPublished = true
            };

            // Assert
            blogPost.PublishedDate.Should().Be(publishedDate);
            blogPost.CreatedDate.Should().Be(createdDate);
            blogPost.IsPublished.Should().BeTrue();
        }

        [Fact]
        public void Project_DateHandling_ShouldWorkCorrectly()
        {
            // Arrange
            var startDate = new DateTime(2024, 1, 1);
            var endDate = new DateTime(2024, 6, 30);

            // Act
            var project = new Project
            {
                Name = "Test Project",
                Description = "Test description",
                StartDate = startDate,
                EndDate = endDate
            };

            // Assert
            project.StartDate.Should().Be(startDate);
            project.EndDate.Should().Be(endDate);
        }

        [Fact]
        public void ContactMessage_DateHandling_ShouldWorkCorrectly()
        {
            // Arrange
            var dateSent = DateTime.UtcNow;

            // Act
            var contactMessage = new ContactMessage
            {
                Name = "Test User",
                Email = "test@example.com",
                Message = "Test message",
                DateSent = dateSent
            };

            // Assert
            contactMessage.DateSent.Should().BeCloseTo(dateSent, TimeSpan.FromSeconds(1));
        }

        [Fact]
        public void Models_RequiredFields_ShouldBePresent()
        {
            // This test ensures that our test data builder creates valid models
            // Arrange & Act
            var skills = TestDataBuilder.CreateTestSkills();
            var projects = TestDataBuilder.CreateTestProjects();
            var blogPosts = TestDataBuilder.CreateTestBlogPosts();
            var contactMessage = TestDataBuilder.CreateValidContactMessage();

            // Assert
            skills.Should().NotBeEmpty();
            skills.Should().AllSatisfy(s => s.Name.Should().NotBeNullOrWhiteSpace());
            skills.Should().AllSatisfy(s => s.Category.Should().NotBeNullOrWhiteSpace());
            skills.Should().AllSatisfy(s => s.Proficiency.Should().BeInRange(1, 10));

            projects.Should().NotBeEmpty();
            projects.Should().AllSatisfy(p => p.Name.Should().NotBeNullOrWhiteSpace());
            projects.Should().AllSatisfy(p => p.Description.Should().NotBeNullOrWhiteSpace());

            blogPosts.Should().NotBeEmpty();
            blogPosts.Should().AllSatisfy(b => b.Title.Should().NotBeNullOrWhiteSpace());
            blogPosts.Should().AllSatisfy(b => b.Slug.Should().NotBeNullOrWhiteSpace());
            blogPosts.Should().AllSatisfy(b => b.Content.Should().NotBeNullOrWhiteSpace());

            contactMessage.Name.Should().NotBeNullOrWhiteSpace();
            contactMessage.Email.Should().NotBeNullOrWhiteSpace();
            contactMessage.Message.Should().NotBeNullOrWhiteSpace();
        }
    }
}