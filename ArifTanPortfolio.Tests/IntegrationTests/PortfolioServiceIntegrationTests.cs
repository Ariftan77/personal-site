using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using ArifTanPortfolio.Services;
using ArifTanPortfolio.Models;
using ArifTanPortfolio.Tests.TestHelpers;

namespace ArifTanPortfolio.Tests.IntegrationTests
{
    public class PortfolioServiceIntegrationTests : IDisposable
    {
        private readonly Mock<ILogger<PortfolioService>> _mockLogger;
        private readonly IMemoryCache _cache;
        private readonly PortfolioService _service;
        private readonly Data.ApplicationDbContext _context;

        public PortfolioServiceIntegrationTests()
        {
            _mockLogger = new Mock<ILogger<PortfolioService>>();
            _cache = new MemoryCache(new MemoryCacheOptions());
            _context = TestDbContextFactory.CreateInMemoryDbContextWithData(Guid.NewGuid().ToString());
            _service = new PortfolioService(_context, _cache, _mockLogger.Object);
        }

        public void Dispose()
        {
            _context.Dispose();
            _cache.Dispose();
        }

        [Fact]
        public async Task EndToEnd_ContactMessageWorkflow_ShouldWorkCorrectly()
        {
            // Arrange
            var contactMessage = new ContactMessage
            {
                Name = "Integration Test User",
                Email = "integration@test.com",
                Subject = "Integration Test Subject",
                Message = "Integration test message content",
                Company = "Test Company",
                Phone = "+1234567890"
            };

            // Act - Save contact message
            await _service.SaveContactMessageAsync(contactMessage);

            // Assert - Verify message was saved
            var savedMessage = _context.ContactMessages.FirstOrDefault(m => m.Email == contactMessage.Email);
            savedMessage.Should().NotBeNull();
            savedMessage!.Name.Should().Be(contactMessage.Name);
            savedMessage.DateSent.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
            savedMessage.IsRead.Should().BeFalse();

            // Act - Get contact message stats
            var stats = await _service.GetContactMessageStatsAsync();

            // Assert - Verify stats include our message
            stats.Should().NotBeNull();
            stats.Should().ContainKey("Total");
            stats["Total"].Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task EndToEnd_BlogPostWorkflow_ShouldWorkCorrectly()
        {
            // Arrange
            var blogPost = new BlogPost
            {
                Title = "Integration Test Blog Post",
                Slug = "integration-test-blog-post",
                Content = "Integration test content",
                Excerpt = "Integration test excerpt",
                Category = "Integration",
                Tags = "Integration, Test, C#",
                IsPublished = true,
                PublishedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow,
                ReadTimeMinutes = 5,
                ViewCount = 0,
                Author = "Integration Test Author",
                AuthorEmail = "integration@test.com"
            };

            // Act - Add blog post to context
            _context.BlogPosts.Add(blogPost);
            await _context.SaveChangesAsync();

            // Assert - Verify blog post retrieval methods
            var retrievedPost = await _service.GetBlogPostBySlugAsync(blogPost.Slug);
            retrievedPost.Should().NotBeNull();
            retrievedPost!.Title.Should().Be(blogPost.Title);

            // Act - Increment view count
            await _service.IncrementBlogPostViewsAsync(retrievedPost.Id);

            // Assert - Verify view count was incremented
            var updatedPost = await _service.GetBlogPostBySlugAsync(blogPost.Slug);
            updatedPost!.ViewCount.Should().Be(1);

            // Act - Search for blog post
            var searchResults = await _service.SearchBlogPostsAsync("Integration Test");
            searchResults.Should().NotBeNull();
            searchResults.Should().HaveCount(1);
            searchResults.First().Title.Should().Be(blogPost.Title);

            // Act - Get blog posts by tag
            var tagResults = await _service.GetBlogPostsByTagAsync("Integration");
            tagResults.Should().NotBeNull();
            tagResults.Should().HaveCount(1);
            tagResults.First().Title.Should().Be(blogPost.Title);

            // Act - Get blog posts by category
            var categoryResults = await _service.GetBlogPostsByCategoryAsync("Integration");
            categoryResults.Should().NotBeNull();
            categoryResults.Should().HaveCount(1);
            categoryResults.First().Title.Should().Be(blogPost.Title);

            // Act - Get all blog tags
            var allTags = await _service.GetAllBlogTagsAsync();
            allTags.Should().NotBeNull();
            allTags.Should().Contain("Integration");
            allTags.Should().Contain("Test");
            allTags.Should().Contain("C#");
        }

        [Fact]
        public async Task EndToEnd_SkillsWorkflow_ShouldWorkCorrectly()
        {
            // Act - Get all skills by category
            var programmingSkills = await _service.GetSkillsByCategoryAsync("Programming Languages");
            programmingSkills.Should().NotBeNull();
            programmingSkills.Should().NotBeEmpty();
            programmingSkills.Should().AllSatisfy(s => s.Category.Should().Be("Programming Languages"));

            // Act - Get top skills
            var topSkills = await _service.GetTopSkillsAsync(5);
            topSkills.Should().NotBeNull();
            topSkills.Should().AllSatisfy(s => s.IsVisible.Should().BeTrue());
            topSkills.Should().BeInDescendingOrder(s => s.Proficiency);

            // Act - Get homepage skills
            var homePageSkills = await _service.GetHomePageSkillsAsync();
            homePageSkills.Should().NotBeNull();
            homePageSkills.Should().AllSatisfy(s => s.IsShowOnHomePage.Should().BeTrue());
            homePageSkills.Should().BeInDescendingOrder(s => s.Proficiency);

            // Act - Get skill categories
            var categories = await _service.GetSkillCategoriesAsync();
            categories.Should().NotBeNull();
            categories.Should().Contain("Programming Languages");
            categories.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task EndToEnd_ProjectsWorkflow_ShouldWorkCorrectly()
        {
            // Act - Get all projects
            var allProjects = await _service.GetAllProjectsAsync();
            allProjects.Should().NotBeNull();
            allProjects.Should().NotBeEmpty();

            // Act - Get featured projects
            var featuredProjects = await _service.GetFeaturedProjectsAsync();
            featuredProjects.Should().NotBeNull();
            featuredProjects.Should().AllSatisfy(p => p.IsFeatured.Should().BeTrue());

            // Act - Get specific project
            var firstProject = allProjects.First();
            var retrievedProject = await _service.GetProjectByIdAsync(firstProject.Id);
            retrievedProject.Should().NotBeNull();
            retrievedProject!.Id.Should().Be(firstProject.Id);
            retrievedProject.Name.Should().Be(firstProject.Name);
        }

        [Fact]
        public async Task EndToEnd_CachingBehavior_ShouldWorkCorrectly()
        {
            // Arrange - Clear cache to ensure fresh start
            _cache.Remove("homepage_skills");

            // Act - First call should hit database
            var skills1 = await _service.GetHomePageSkillsAsync();
            
            // Act - Second call should use cache
            var skills2 = await _service.GetHomePageSkillsAsync();

            // Assert - Both calls should return same data
            skills1.Should().NotBeNull();
            skills2.Should().NotBeNull();
            skills1.Should().BeEquivalentTo(skills2);

            // Act - Test cache with different method
            _cache.Remove("recent_blog_posts_3");
            var posts1 = await _service.GetRecentBlogPostsAsync(3);
            var posts2 = await _service.GetRecentBlogPostsAsync(3);

            // Assert - Both calls should return same data
            posts1.Should().NotBeNull();
            posts2.Should().NotBeNull();
            posts1.Should().BeEquivalentTo(posts2);
        }

        [Fact]
        public async Task EndToEnd_RateLimiting_ShouldPreventSpam()
        {
            // Arrange - Create multiple contact messages with same email
            var baseMessage = TestDataBuilder.CreateValidContactMessage();
            var spamMessage1 = new ContactMessage
            {
                Name = baseMessage.Name,
                Email = baseMessage.Email,
                Subject = "Spam Message 1",
                Message = baseMessage.Message,
                Company = baseMessage.Company,
                Phone = baseMessage.Phone
            };

            var spamMessage2 = new ContactMessage
            {
                Name = baseMessage.Name,
                Email = baseMessage.Email,
                Subject = "Spam Message 2",
                Message = baseMessage.Message,
                Company = baseMessage.Company,
                Phone = baseMessage.Phone
            };

            var spamMessage3 = new ContactMessage
            {
                Name = baseMessage.Name,
                Email = baseMessage.Email,
                Subject = "Spam Message 3",
                Message = baseMessage.Message,
                Company = baseMessage.Company,
                Phone = baseMessage.Phone
            };

            var spamMessage4 = new ContactMessage
            {
                Name = baseMessage.Name,
                Email = baseMessage.Email,
                Subject = "Spam Message 4",
                Message = baseMessage.Message,
                Company = baseMessage.Company,
                Phone = baseMessage.Phone
            };

            // Act - Save first three messages (should succeed)
            await _service.SaveContactMessageAsync(spamMessage1);
            await _service.SaveContactMessageAsync(spamMessage2);
            await _service.SaveContactMessageAsync(spamMessage3);

            // Act - Try to save fourth message (should fail due to rate limiting)
            var exception = await Record.ExceptionAsync(() => _service.SaveContactMessageAsync(spamMessage4));

            // Assert - Fourth message should be rejected
            exception.Should().NotBeNull();
            exception.Should().BeOfType<InvalidOperationException>();
        }

        [Fact]
        public async Task EndToEnd_DataValidation_ShouldWorkCorrectly()
        {
            // Arrange - Create various invalid contact messages
            var emptyNameMessage = TestDataBuilder.CreateValidContactMessage();
            emptyNameMessage.Name = "";

            var invalidEmailMessage = TestDataBuilder.CreateValidContactMessage();
            invalidEmailMessage.Email = "invalid-email";

            var emptyMessageContent = TestDataBuilder.CreateValidContactMessage();
            emptyMessageContent.Message = "";

            // Act & Assert - All invalid messages should be rejected
            await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(
                () => _service.SaveContactMessageAsync(emptyNameMessage));

            await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(
                () => _service.SaveContactMessageAsync(invalidEmailMessage));

            await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(
                () => _service.SaveContactMessageAsync(emptyMessageContent));
        }

        [Fact]
        public async Task EndToEnd_PaginationAndSorting_ShouldWorkCorrectly()
        {
            // Act - Get recent blog posts with different counts
            var posts1 = await _service.GetRecentBlogPostsAsync(1);
            var posts2 = await _service.GetRecentBlogPostsAsync(2);

            // Assert - Should return correct counts and be sorted by date
            posts1.Should().NotBeNull();
            posts1.Should().HaveCount(1);
            
            posts2.Should().NotBeNull();
            posts2.Should().HaveCount(2);
            posts2.Should().BeInDescendingOrder(p => p.PublishedDate);

            // Act - Get top skills with different counts
            var topSkills1 = await _service.GetTopSkillsAsync(1);
            var topSkills2 = await _service.GetTopSkillsAsync(2);

            // Assert - Should return correct counts and be sorted by proficiency
            topSkills1.Should().NotBeNull();
            topSkills1.Should().HaveCount(1);
            
            topSkills2.Should().NotBeNull();
            topSkills2.Should().HaveCount(2);
            topSkills2.Should().BeInDescendingOrder(s => s.Proficiency);
        }

        [Fact]
        public async Task EndToEnd_RelatedBlogPosts_ShouldWorkCorrectly()
        {
            // Arrange - Get a blog post with tags
            var blogPost = await _service.GetBlogPostBySlugAsync("published-blog-post-1");
            blogPost.Should().NotBeNull();

            // Act - Get related blog posts
            var relatedPosts = await _service.GetRelatedBlogPostsAsync(blogPost!.Id, blogPost.Tags, 3);

            // Assert - Should return related posts (excluding the current post)
            relatedPosts.Should().NotBeNull();
            relatedPosts.Should().NotContain(p => p.Id == blogPost.Id);
            
            // Act - Get adjacent blog posts
            var adjacentPosts = await _service.GetAdjacentBlogPostsAsync(blogPost.Id);

            // Assert - Should return previous and/or next posts
            adjacentPosts.Should().NotBeNull();
            // Note: Previous and Next might be null depending on position in sequence
        }
    }
}