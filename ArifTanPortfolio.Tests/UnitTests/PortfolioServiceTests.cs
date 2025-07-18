using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using FluentAssertions;
using ArifTanPortfolio.Services;
using ArifTanPortfolio.Models;
using ArifTanPortfolio.Tests.TestHelpers;

namespace ArifTanPortfolio.Tests.UnitTests
{
    public class PortfolioServiceTests : IDisposable
    {
        private readonly Mock<ILogger<PortfolioService>> _mockLogger;
        private readonly IMemoryCache _cache;
        private readonly PortfolioService _service;
        private readonly Data.ApplicationDbContext _context;

        public PortfolioServiceTests()
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
        public async Task GetFeaturedProjectsAsync_ShouldReturnOnlyFeaturedProjects()
        {
            // Act
            var result = await _service.GetFeaturedProjectsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().AllSatisfy(p => p.IsFeatured.Should().BeTrue());
            result.Should().HaveCount(1);
            result.First().Name.Should().Be("Featured Project 1");
        }

        [Fact]
        public async Task GetAllProjectsAsync_ShouldReturnAllProjects()
        {
            // Act
            var result = await _service.GetAllProjectsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
            result.Should().Contain(p => p.IsFeatured);
            result.Should().Contain(p => !p.IsFeatured);
        }

        [Fact]
        public async Task GetProjectByIdAsync_WithValidId_ShouldReturnProject()
        {
            // Arrange
            var projectId = 1;

            // Act
            var result = await _service.GetProjectByIdAsync(projectId);

            // Assert
            result.Should().NotBeNull();
            result!.Id.Should().Be(projectId);
            result.Name.Should().Be("Featured Project 1");
        }

        [Fact]
        public async Task GetProjectByIdAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = 999;

            // Act
            var result = await _service.GetProjectByIdAsync(invalidId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetProjectByIdAsync_WithZeroId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = 0;

            // Act
            var result = await _service.GetProjectByIdAsync(invalidId);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetRecentBlogPostsAsync_ShouldReturnPublishedPostsOnly()
        {
            // Act
            var result = await _service.GetRecentBlogPostsAsync(3);

            // Assert
            result.Should().NotBeNull();
            result.Should().AllSatisfy(p => p.IsPublished.Should().BeTrue());
            result.Should().HaveCount(2); // Only 2 published posts in test data
            result.Should().BeInDescendingOrder(p => p.PublishedDate);
        }

        [Fact]
        public async Task GetRecentBlogPostsAsync_WithInvalidCount_ShouldUseDefaultCount()
        {
            // Act
            var result = await _service.GetRecentBlogPostsAsync(0);

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2); // Should return published posts with default fallback
        }

        [Fact]
        public async Task GetPublishedBlogPostsAsync_ShouldReturnOnlyPublishedPosts()
        {
            // Act
            var result = await _service.GetPublishedBlogPostsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().AllSatisfy(p => p.IsPublished.Should().BeTrue());
            result.Should().HaveCount(2);
            result.Should().BeInDescendingOrder(p => p.PublishedDate);
        }

        [Fact]
        public async Task GetBlogPostBySlugAsync_WithValidSlug_ShouldReturnBlogPost()
        {
            // Arrange
            var slug = "published-blog-post-1";

            // Act
            var result = await _service.GetBlogPostBySlugAsync(slug);

            // Assert
            result.Should().NotBeNull();
            result!.Slug.Should().Be(slug);
            result.Title.Should().Be("Published Blog Post 1");
        }

        [Fact]
        public async Task GetBlogPostBySlugAsync_WithInvalidSlug_ShouldReturnNull()
        {
            // Arrange
            var invalidSlug = "non-existent-slug";

            // Act
            var result = await _service.GetBlogPostBySlugAsync(invalidSlug);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetSkillsByCategoryAsync_ShouldReturnSkillsInCategory()
        {
            // Arrange
            var category = "Programming Languages";

            // Act
            var result = await _service.GetSkillsByCategoryAsync(category);

            // Assert
            result.Should().NotBeNull();
            result.Should().AllSatisfy(s => s.Category.Should().Be(category));
            result.Should().HaveCount(3); // 3 visible skills in Programming Languages category
            result.Should().BeInDescendingOrder(s => s.Proficiency);
        }

        [Fact]
        public async Task GetTopSkillsAsync_ShouldReturnTopSkillsOrderedByProficiency()
        {
            // Act
            var result = await _service.GetTopSkillsAsync(2);

            // Assert
            result.Should().NotBeNull();
            result.Should().AllSatisfy(s => s.IsVisible.Should().BeTrue());
            result.Should().HaveCount(2);
            result.Should().BeInDescendingOrder(s => s.Proficiency);
            result.First().Name.Should().Be("C#"); // Highest proficiency
        }

        [Fact]
        public async Task GetHomePageSkillsAsync_ShouldReturnOnlyHomePageSkills()
        {
            // Act
            var result = await _service.GetHomePageSkillsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().AllSatisfy(s => s.IsShowOnHomePage.Should().BeTrue());
            result.Should().HaveCount(2); // Only 2 skills marked for homepage
            result.Should().BeInDescendingOrder(s => s.Proficiency);
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithValidMessage_ShouldReturnTrue()
        {
            // Arrange
            var validMessage = TestDataBuilder.CreateValidContactMessage();

            // Act
            var result = await _service.ValidateContactMessageAsync(validMessage);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithInvalidMessage_ShouldReturnFalse()
        {
            // Arrange
            var invalidMessage = TestDataBuilder.CreateInvalidContactMessage();

            // Act
            var result = await _service.ValidateContactMessageAsync(invalidMessage);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithNullMessage_ShouldReturnFalse()
        {
            // Act
            var result = await _service.ValidateContactMessageAsync(null!);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task SaveContactMessageAsync_WithValidMessage_ShouldSaveSuccessfully()
        {
            // Arrange
            var validMessage = TestDataBuilder.CreateValidContactMessage();

            // Act
            await _service.SaveContactMessageAsync(validMessage);

            // Assert
            var savedMessage = _context.ContactMessages.FirstOrDefault(m => m.Email == validMessage.Email);
            savedMessage.Should().NotBeNull();
            savedMessage!.Name.Should().Be(validMessage.Name);
            savedMessage.DateSent.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(10));
            savedMessage.IsRead.Should().BeFalse();
        }

        [Fact]
        public async Task SaveContactMessageAsync_WithInvalidMessage_ShouldThrowValidationException()
        {
            // Arrange
            var invalidMessage = TestDataBuilder.CreateInvalidContactMessage();

            // Act & Assert
            await Assert.ThrowsAsync<System.ComponentModel.DataAnnotations.ValidationException>(
                () => _service.SaveContactMessageAsync(invalidMessage));
        }

        [Fact]
        public async Task GetSkillCategoriesAsync_ShouldReturnDistinctCategories()
        {
            // Act
            var result = await _service.GetSkillCategoriesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().Contain("Programming Languages");
            result.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task SearchBlogPostsAsync_WithValidTerm_ShouldReturnMatchingPosts()
        {
            // Arrange
            var searchTerm = "Blog Post 1";

            // Act
            var result = await _service.SearchBlogPostsAsync(searchTerm);

            // Assert
            result.Should().NotBeNull();
            result.Should().AllSatisfy(p => p.IsPublished.Should().BeTrue());
            result.Should().HaveCount(1);
            result.First().Title.Should().Contain("Blog Post 1");
        }

        [Fact]
        public async Task SearchBlogPostsAsync_WithEmptyTerm_ShouldReturnEmptyList()
        {
            // Arrange
            var searchTerm = "";

            // Act
            var result = await _service.SearchBlogPostsAsync(searchTerm);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Fact]
        public async Task GetBlogPostsByTagAsync_WithValidTag_ShouldReturnMatchingPosts()
        {
            // Arrange
            var tag = "C#";

            // Act
            var result = await _service.GetBlogPostsByTagAsync(tag);

            // Assert
            result.Should().NotBeNull();
            result.Should().AllSatisfy(p => p.IsPublished.Should().BeTrue());
            result.Should().HaveCount(1);
            result.First().Tags.Should().Contain(tag);
        }

        [Fact]
        public async Task GetAllBlogTagsAsync_ShouldReturnAllUniqueTags()
        {
            // Act
            var result = await _service.GetAllBlogTagsAsync();

            // Assert
            result.Should().NotBeNull();
            result.Should().Contain("C#");
            result.Should().Contain("ASP.NET Core");
            result.Should().Contain("JavaScript");
            result.Should().Contain("Node.js");
            result.Should().OnlyHaveUniqueItems();
        }

        [Fact]
        public async Task GetBlogPostsByCategoryAsync_WithValidCategory_ShouldReturnMatchingPosts()
        {
            // Arrange
            var category = "Technical";

            // Act
            var result = await _service.GetBlogPostsByCategoryAsync(category);

            // Assert
            result.Should().NotBeNull();
            result.Should().AllSatisfy(p => p.IsPublished.Should().BeTrue());
            result.Should().AllSatisfy(p => p.Category.Should().Be(category));
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task IncrementBlogPostViewsAsync_WithValidId_ShouldIncrementViewCount()
        {
            // Arrange
            var blogPostId = 1;
            var originalPost = await _service.GetBlogPostBySlugAsync("published-blog-post-1");
            var originalViewCount = originalPost!.ViewCount;

            // Act
            await _service.IncrementBlogPostViewsAsync(blogPostId);

            // Assert
            var updatedPost = await _service.GetBlogPostBySlugAsync("published-blog-post-1");
            updatedPost!.ViewCount.Should().Be(originalViewCount + 1);
        }

        [Fact]
        public async Task IncrementBlogPostViewsAsync_WithInvalidId_ShouldNotThrow()
        {
            // Arrange
            var invalidId = 999;

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _service.IncrementBlogPostViewsAsync(invalidId));
            exception.Should().BeNull();
        }

        // Test caching behavior
        [Fact]
        public async Task GetTopSkillsAsync_SecondCall_ShouldUseCachedData()
        {
            // Arrange
            var count = 2;

            // Act
            var result1 = await _service.GetTopSkillsAsync(count);
            var result2 = await _service.GetTopSkillsAsync(count);

            // Assert
            result1.Should().NotBeNull();
            result2.Should().NotBeNull();
            result1.Should().BeEquivalentTo(result2);
        }

        [Fact]
        public async Task GetHomePageSkillsAsync_SecondCall_ShouldUseCachedData()
        {
            // Act
            var result1 = await _service.GetHomePageSkillsAsync();
            var result2 = await _service.GetHomePageSkillsAsync();

            // Assert
            result1.Should().NotBeNull();
            result2.Should().NotBeNull();
            result1.Should().BeEquivalentTo(result2);
        }
    }
}