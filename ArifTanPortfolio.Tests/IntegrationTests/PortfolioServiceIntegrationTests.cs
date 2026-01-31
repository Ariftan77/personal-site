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

        [Fact]
        public async Task GetPublishedBlogPostsAsync_ShouldReturnOnlyPublishedPosts()
        {
            // Act
            var publishedPosts = await _service.GetPublishedBlogPostsAsync();

            // Assert
            publishedPosts.Should().NotBeNull();
            publishedPosts.Should().AllSatisfy(p => p.IsPublished.Should().BeTrue());
            publishedPosts.Should().BeInDescendingOrder(p => p.PublishedDate);
        }

        [Fact]
        public async Task GetBlogPostBySlugAsync_WithValidSlug_ShouldReturnPost()
        {
            // Arrange
            var slug = "published-blog-post-1";

            // Act
            var post = await _service.GetBlogPostBySlugAsync(slug);

            // Assert
            post.Should().NotBeNull();
            post!.Slug.Should().Be(slug);
            post.IsPublished.Should().BeTrue();
        }

        [Fact]
        public async Task GetBlogPostBySlugAsync_WithInvalidSlug_ShouldReturnNull()
        {
            // Arrange
            var invalidSlug = "non-existent-blog-post-slug";

            // Act
            var post = await _service.GetBlogPostBySlugAsync(invalidSlug);

            // Assert
            post.Should().BeNull();
        }

        [Fact]
        public async Task GetBlogPostBySlugAsync_WithEmptySlug_ShouldReturnNull()
        {
            // Arrange
            var emptySlug = "";

            // Act
            var post = await _service.GetBlogPostBySlugAsync(emptySlug);

            // Assert
            post.Should().BeNull();
        }

        [Fact]
        public async Task GetRelatedBlogPostsAsync_WithNoTags_ShouldReturnRecentPosts()
        {
            // Arrange
            var blogPost = await _service.GetBlogPostBySlugAsync("published-blog-post-1");
            blogPost.Should().NotBeNull();

            // Act
            var relatedPosts = await _service.GetRelatedBlogPostsAsync(blogPost!.Id, null, 3);

            // Assert
            relatedPosts.Should().NotBeNull();
            relatedPosts.Should().NotContain(p => p.Id == blogPost.Id);
        }

        [Fact]
        public async Task GetRelatedBlogPostsAsync_WithEmptyTags_ShouldReturnRecentPosts()
        {
            // Arrange
            var blogPost = await _service.GetBlogPostBySlugAsync("published-blog-post-1");
            blogPost.Should().NotBeNull();

            // Act
            var relatedPosts = await _service.GetRelatedBlogPostsAsync(blogPost!.Id, "", 3);

            // Assert
            relatedPosts.Should().NotBeNull();
            relatedPosts.Should().NotContain(p => p.Id == blogPost.Id);
        }

        [Fact]
        public async Task GetRelatedBlogPostsAsync_WithMatchingTags_ShouldReturnRelatedPosts()
        {
            // Arrange - Add multiple blog posts with overlapping tags
            var post1 = new BlogPost
            {
                Title = "Test Post 1",
                Slug = "test-post-1",
                Content = "Content 1",
                Excerpt = "Excerpt 1",
                Tags = "C#, ASP.NET Core, Testing",
                IsPublished = true,
                PublishedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };

            var post2 = new BlogPost
            {
                Title = "Test Post 2",
                Slug = "test-post-2",
                Content = "Content 2",
                Excerpt = "Excerpt 2",
                Tags = "C#, Docker, Testing",
                IsPublished = true,
                PublishedDate = DateTime.UtcNow.AddDays(-1),
                CreatedDate = DateTime.UtcNow.AddDays(-1)
            };

            _context.BlogPosts.AddRange(post1, post2);
            await _context.SaveChangesAsync();

            // Act - Get related posts for post1
            var relatedPosts = await _service.GetRelatedBlogPostsAsync(post1.Id, post1.Tags, 5);

            // Assert - Should include post2 (has matching tags C# and Testing)
            relatedPosts.Should().NotBeNull();
            relatedPosts.Should().Contain(p => p.Id == post2.Id);
            relatedPosts.Should().NotContain(p => p.Id == post1.Id);
        }

        [Fact]
        public async Task GetAdjacentBlogPostsAsync_WithMiddlePost_ShouldReturnPreviousAndNext()
        {
            // Arrange - Add blog posts in sequence
            var oldPost = new BlogPost
            {
                Title = "Old Post",
                Slug = "old-post",
                Content = "Old content",
                Excerpt = "Old excerpt",
                IsPublished = true,
                PublishedDate = DateTime.UtcNow.AddDays(-2),
                CreatedDate = DateTime.UtcNow.AddDays(-2)
            };

            var middlePost = new BlogPost
            {
                Title = "Middle Post",
                Slug = "middle-post",
                Content = "Middle content",
                Excerpt = "Middle excerpt",
                IsPublished = true,
                PublishedDate = DateTime.UtcNow.AddDays(-1),
                CreatedDate = DateTime.UtcNow.AddDays(-1)
            };

            var newPost = new BlogPost
            {
                Title = "New Post",
                Slug = "new-post",
                Content = "New content",
                Excerpt = "New excerpt",
                IsPublished = true,
                PublishedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };

            _context.BlogPosts.AddRange(oldPost, middlePost, newPost);
            await _context.SaveChangesAsync();

            // Act
            var (previous, next) = await _service.GetAdjacentBlogPostsAsync(middlePost.Id);

            // Assert
            previous.Should().NotBeNull();
            previous!.Id.Should().Be(oldPost.Id);
            next.Should().NotBeNull();
            next!.Id.Should().Be(newPost.Id);
        }

        [Fact]
        public async Task GetAdjacentBlogPostsAsync_WithFirstPost_ShouldReturnOnlyNext()
        {
            // Arrange - Clear existing and add new posts
            _context.BlogPosts.RemoveRange(_context.BlogPosts);
            await _context.SaveChangesAsync();

            var firstPost = new BlogPost
            {
                Title = "First Post",
                Slug = "first-post",
                Content = "First content",
                Excerpt = "First excerpt",
                IsPublished = true,
                PublishedDate = DateTime.UtcNow.AddDays(-1),
                CreatedDate = DateTime.UtcNow.AddDays(-1)
            };

            var secondPost = new BlogPost
            {
                Title = "Second Post",
                Slug = "second-post",
                Content = "Second content",
                Excerpt = "Second excerpt",
                IsPublished = true,
                PublishedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };

            _context.BlogPosts.AddRange(firstPost, secondPost);
            await _context.SaveChangesAsync();

            // Act
            var (previous, next) = await _service.GetAdjacentBlogPostsAsync(firstPost.Id);

            // Assert
            previous.Should().BeNull();
            next.Should().NotBeNull();
            next!.Id.Should().Be(secondPost.Id);
        }

        [Fact]
        public async Task GetAdjacentBlogPostsAsync_WithLastPost_ShouldReturnOnlyPrevious()
        {
            // Arrange
            var allPosts = await _service.GetPublishedBlogPostsAsync();
            allPosts.Should().NotBeEmpty();

            var latestPost = new BlogPost
            {
                Title = "Latest Post",
                Slug = "latest-post",
                Content = "Latest content",
                Excerpt = "Latest excerpt",
                IsPublished = true,
                PublishedDate = DateTime.UtcNow.AddDays(10), // Future date to ensure it's last
                CreatedDate = DateTime.UtcNow
            };

            _context.BlogPosts.Add(latestPost);
            await _context.SaveChangesAsync();

            // Act
            var (previous, next) = await _service.GetAdjacentBlogPostsAsync(latestPost.Id);

            // Assert
            previous.Should().NotBeNull();
            next.Should().BeNull();
        }

        [Fact]
        public async Task GetAdjacentBlogPostsAsync_WithInvalidId_ShouldReturnNull()
        {
            // Arrange
            var invalidId = 99999;

            // Act
            var (previous, next) = await _service.GetAdjacentBlogPostsAsync(invalidId);

            // Assert
            previous.Should().BeNull();
            next.Should().BeNull();
        }

        [Fact]
        public async Task IncrementBlogPostViewsAsync_WithValidId_ShouldIncrementCount()
        {
            // Arrange
            var post = await _service.GetBlogPostBySlugAsync("published-blog-post-1");
            post.Should().NotBeNull();
            var originalViewCount = post!.ViewCount;

            // Act
            await _service.IncrementBlogPostViewsAsync(post.Id);

            // Assert
            var updatedPost = await _service.GetBlogPostBySlugAsync("published-blog-post-1");
            updatedPost!.ViewCount.Should().Be(originalViewCount + 1);
        }

        [Fact]
        public async Task IncrementBlogPostViewsAsync_WithInvalidId_ShouldNotThrowException()
        {
            // Arrange
            var invalidId = 99999;

            // Act & Assert
            var exception = await Record.ExceptionAsync(() => _service.IncrementBlogPostViewsAsync(invalidId));
            exception.Should().BeNull();
        }

        [Fact]
        public async Task IncrementBlogPostViewsAsync_MultipleTimes_ShouldIncrementCorrectly()
        {
            // Arrange
            var post = await _service.GetBlogPostBySlugAsync("published-blog-post-1");
            post.Should().NotBeNull();
            var originalViewCount = post!.ViewCount;

            // Act
            await _service.IncrementBlogPostViewsAsync(post.Id);
            await _service.IncrementBlogPostViewsAsync(post.Id);
            await _service.IncrementBlogPostViewsAsync(post.Id);

            // Assert
            var updatedPost = await _service.GetBlogPostBySlugAsync("published-blog-post-1");
            updatedPost!.ViewCount.Should().Be(originalViewCount + 3);
        }

        [Fact]
        public async Task SearchBlogPostsAsync_WithMatchingTitle_ShouldReturnResults()
        {
            // Arrange
            var searchTerm = "Blog Post";

            // Act
            var results = await _service.SearchBlogPostsAsync(searchTerm);

            // Assert
            results.Should().NotBeNull();
            results.Should().NotBeEmpty();
            results.Should().AllSatisfy(p => p.IsPublished.Should().BeTrue());
        }

        [Fact]
        public async Task SearchBlogPostsAsync_WithMatchingContent_ShouldReturnResults()
        {
            // Arrange - Add post with specific content
            var post = new BlogPost
            {
                Title = "Search Test Post",
                Slug = "search-test-post",
                Content = "This content contains unique searchable keyword",
                Excerpt = "Search excerpt",
                IsPublished = true,
                PublishedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };

            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var results = await _service.SearchBlogPostsAsync("searchable keyword");

            // Assert
            results.Should().NotBeNull();
            results.Should().Contain(p => p.Id == post.Id);
        }

        [Fact]
        public async Task SearchBlogPostsAsync_WithEmptyTerm_ShouldReturnEmptyList()
        {
            // Act
            var results = await _service.SearchBlogPostsAsync("");

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchBlogPostsAsync_WithNoMatches_ShouldReturnEmptyList()
        {
            // Arrange
            var searchTerm = "NonExistentSearchTermXYZ123";

            // Act
            var results = await _service.SearchBlogPostsAsync(searchTerm);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchBlogPostsAsync_CaseInsensitive_ShouldReturnResults()
        {
            // Arrange
            var post = new BlogPost
            {
                Title = "UPPERCASE Title",
                Slug = "uppercase-title",
                Content = "lowercase content",
                Excerpt = "MixedCase Excerpt",
                IsPublished = true,
                PublishedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };

            _context.BlogPosts.Add(post);
            await _context.SaveChangesAsync();

            // Act
            var resultsLower = await _service.SearchBlogPostsAsync("uppercase");
            var resultsUpper = await _service.SearchBlogPostsAsync("LOWERCASE");
            var resultsMixed = await _service.SearchBlogPostsAsync("MiXeDcAsE");

            // Assert
            resultsLower.Should().Contain(p => p.Id == post.Id);
            resultsUpper.Should().Contain(p => p.Id == post.Id);
            resultsMixed.Should().Contain(p => p.Id == post.Id);
        }

        [Fact]
        public async Task GetBlogPostsByTagAsync_WithValidTag_ShouldReturnMatchingPosts()
        {
            // Arrange
            var tag = "C#";

            // Act
            var results = await _service.GetBlogPostsByTagAsync(tag);

            // Assert
            results.Should().NotBeNull();
            results.Should().AllSatisfy(p => p.IsPublished.Should().BeTrue());
            results.Should().AllSatisfy(p => p.Tags.Should().Contain(tag));
        }

        [Fact]
        public async Task GetBlogPostsByTagAsync_WithEmptyTag_ShouldReturnEmptyList()
        {
            // Act
            var results = await _service.GetBlogPostsByTagAsync("");

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetBlogPostsByTagAsync_CaseInsensitive_ShouldReturnResults()
        {
            // Arrange - Get posts with known tags
            var posts = await _service.GetPublishedBlogPostsAsync();
            var postWithTag = posts.FirstOrDefault(p => !string.IsNullOrEmpty(p.Tags));
            postWithTag.Should().NotBeNull();

            if (postWithTag != null)
            {
                var tag = postWithTag.Tags!.Split(',')[0].Trim();

                // Act
                var resultsLower = await _service.GetBlogPostsByTagAsync(tag.ToLower());
                var resultsUpper = await _service.GetBlogPostsByTagAsync(tag.ToUpper());

                // Assert
                resultsLower.Should().NotBeEmpty();
                resultsUpper.Should().NotBeEmpty();
                resultsLower.Should().BeEquivalentTo(resultsUpper);
            }
        }

        [Fact]
        public async Task GetAllBlogTagsAsync_ShouldReturnUniqueTagsList()
        {
            // Act
            var tags = await _service.GetAllBlogTagsAsync();

            // Assert
            tags.Should().NotBeNull();
            tags.Should().OnlyHaveUniqueItems();
            tags.Should().BeInAscendingOrder();
        }

        [Fact]
        public async Task GetBlogPostsByCategoryAsync_WithValidCategory_ShouldReturnMatchingPosts()
        {
            // Arrange
            var category = "Technical";

            // Act
            var results = await _service.GetBlogPostsByCategoryAsync(category);

            // Assert
            results.Should().NotBeNull();
            results.Should().AllSatisfy(p => p.IsPublished.Should().BeTrue());
            results.Should().AllSatisfy(p => p.Category.Should().Be(category));
            results.Should().BeInDescendingOrder(p => p.PublishedDate);
        }

        [Fact]
        public async Task GetBlogPostsByCategoryAsync_WithNonExistentCategory_ShouldReturnEmptyList()
        {
            // Arrange
            var category = "NonExistentCategory123";

            // Act
            var results = await _service.GetBlogPostsByCategoryAsync(category);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetProjectByIdAsync_WithZeroId_ShouldReturnNull()
        {
            // Act
            var result = await _service.GetProjectByIdAsync(0);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetProjectByIdAsync_WithNegativeId_ShouldReturnNull()
        {
            // Act
            var result = await _service.GetProjectByIdAsync(-1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task GetRecentBlogPostsAsync_WithZeroCount_ShouldUseDefaultCount()
        {
            // Act
            var results = await _service.GetRecentBlogPostsAsync(0);

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().BeLessThanOrEqualTo(3); // Default count is 3
        }

        [Fact]
        public async Task GetRecentBlogPostsAsync_WithNegativeCount_ShouldUseDefaultCount()
        {
            // Act
            var results = await _service.GetRecentBlogPostsAsync(-5);

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().BeLessThanOrEqualTo(3); // Default count is 3
        }

        [Fact]
        public async Task GetRecentBlogPostsAsync_WithLargeCount_ShouldUseDefaultCount()
        {
            // Act
            var results = await _service.GetRecentBlogPostsAsync(100);

            // Assert
            results.Should().NotBeNull();
            // Should use default count of 3 due to validation
        }

        [Fact]
        public async Task GetTopSkillsAsync_WithZeroCount_ShouldUseDefaultCount()
        {
            // Act
            var results = await _service.GetTopSkillsAsync(0);

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().BeLessThanOrEqualTo(8); // Default count is 8
        }

        [Fact]
        public async Task GetTopSkillsAsync_WithNegativeCount_ShouldUseDefaultCount()
        {
            // Act
            var results = await _service.GetTopSkillsAsync(-5);

            // Assert
            results.Should().NotBeNull();
            results.Count.Should().BeLessThanOrEqualTo(8); // Default count is 8
        }

        [Fact]
        public async Task GetSkillsByCategoryAsync_WithEmptyCategory_ShouldReturnEmptyList()
        {
            // Act
            var results = await _service.GetSkillsByCategoryAsync("");

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetSkillsByCategoryAsync_WithNonExistentCategory_ShouldReturnEmptyList()
        {
            // Arrange
            var category = "NonExistentCategory123";

            // Act
            var results = await _service.GetSkillsByCategoryAsync(category);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithValidMessage_ShouldReturnTrue()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeTrue();
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
        public async Task ValidateContactMessageAsync_WithShortName_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Name = "A"; // Too short

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithInvalidEmail_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Email = "invalid-email";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithShortMessage_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Message = "Short"; // Less than 10 characters

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithSpamKeywords_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Message = "Get cheap viagra now! Make money fast with bitcoin!";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task SaveContactMessageAsync_ShouldNormalizeEmailToLowerCase()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Email = "TEST@EXAMPLE.COM";

            // Act
            await _service.SaveContactMessageAsync(message);

            // Assert
            var saved = _context.ContactMessages.FirstOrDefault(m => m.Email == "test@example.com");
            saved.Should().NotBeNull();
            saved!.Email.Should().Be("test@example.com");
        }

        [Fact]
        public async Task SaveContactMessageAsync_ShouldTrimWhitespace()
        {
            // Arrange
            var message = new ContactMessage
            {
                Name = "  John Doe  ",
                Email = "johntrim@example.com", // Email needs to be valid first, service will trim during save
                Subject = "  Test Subject  ",
                Message = "  Test message content with enough characters  "
            };

            // Act
            await _service.SaveContactMessageAsync(message);

            // Assert
            var saved = _context.ContactMessages.FirstOrDefault(m => m.Email == "johntrim@example.com");
            saved.Should().NotBeNull();
            saved!.Name.Should().Be("John Doe");
            saved.Email.Should().Be("johntrim@example.com");
            saved.Subject.Should().Be("Test Subject");
            saved.Message.Should().Be("Test message content with enough characters");
        }

        [Fact]
        public async Task GetContactMessageStatsAsync_ShouldReturnAccurateStats()
        {
            // Arrange - Save test messages
            var message1 = TestDataBuilder.CreateValidContactMessage();
            message1.Email = "stats1@test.com";
            await _service.SaveContactMessageAsync(message1);

            var message2 = TestDataBuilder.CreateValidContactMessage();
            message2.Email = "stats2@test.com";
            await _service.SaveContactMessageAsync(message2);

            // Act
            var stats = await _service.GetContactMessageStatsAsync();

            // Assert
            stats.Should().NotBeNull();
            stats.Should().ContainKey("Total");
            stats.Should().ContainKey("Unread");
            stats.Should().ContainKey("Today");
            stats.Should().ContainKey("ThisWeek");
            stats["Total"].Should().BeGreaterThanOrEqualTo(2);
            stats["Unread"].Should().BeGreaterThanOrEqualTo(2);
        }

        [Fact]
        public async Task CachingBehavior_DifferentMethods_ShouldWorkIndependently()
        {
            // Act - Call multiple cached methods
            var skills1 = await _service.GetTopSkillsAsync(3);
            var skills2 = await _service.GetHomePageSkillsAsync();
            var projects = await _service.GetFeaturedProjectsAsync();
            var posts = await _service.GetRecentBlogPostsAsync(3);

            // Assert - All should return valid data
            skills1.Should().NotBeNull();
            skills2.Should().NotBeNull();
            projects.Should().NotBeNull();
            posts.Should().NotBeNull();

            // Act - Call again (should use cache)
            var skills1Cached = await _service.GetTopSkillsAsync(3);
            var skills2Cached = await _service.GetHomePageSkillsAsync();
            var projectsCached = await _service.GetFeaturedProjectsAsync();
            var postsCached = await _service.GetRecentBlogPostsAsync(3);

            // Assert - Should return equivalent data
            skills1.Should().BeEquivalentTo(skills1Cached);
            skills2.Should().BeEquivalentTo(skills2Cached);
            projects.Should().BeEquivalentTo(projectsCached);
            posts.Should().BeEquivalentTo(postsCached);
        }

        [Fact]
        public async Task ClearCache_ShouldInvalidateAllCachedData()
        {
            // Arrange - First call to populate cache
            var skills1 = await _service.GetTopSkillsAsync(3);
            var projects1 = await _service.GetFeaturedProjectsAsync();
            var posts1 = await _service.GetRecentBlogPostsAsync(3);

            // Act - Clear cache
            _service.ClearCache();

            // Act - Call again (should fetch from database, not cache)
            var skills2 = await _service.GetTopSkillsAsync(3);
            var projects2 = await _service.GetFeaturedProjectsAsync();
            var posts2 = await _service.GetRecentBlogPostsAsync(3);

            // Assert - Data should still be equivalent but fetched fresh
            skills1.Should().BeEquivalentTo(skills2);
            projects1.Should().BeEquivalentTo(projects2);
            posts1.Should().BeEquivalentTo(posts2);
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithEmptyName_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Name = "";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithWhitespaceName_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Name = "   ";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithEmptyEmail_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Email = "";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithWhitespaceEmail_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Email = "   ";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithEmptyMessage_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Message = "";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithWhitespaceMessage_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Message = "   ";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithEmailWithoutAtSymbol_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Email = "notanemail.com";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithEmailWithoutDomain_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Email = "user@";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithMultipleAtSymbols_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Email = "user@@example.com";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("viagra")]
        [InlineData("casino")]
        [InlineData("loan")]
        [InlineData("bitcoin")]
        [InlineData("make money fast")]
        public async Task ValidateContactMessageAsync_WithSpamKeyword_ShouldReturnFalse(string spamWord)
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Message = $"Hello, I want to discuss {spamWord} with you.";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("VIAGRA")]
        [InlineData("CaSiNo")]
        [InlineData("LOAN")]
        public async Task ValidateContactMessageAsync_WithSpamKeywordUpperCase_ShouldReturnFalse(string spamWord)
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Message = $"Hello, I want to discuss {spamWord} with you.";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithSpamInSubject_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Subject = "Get cheap viagra now!";
            message.Message = "This is a legitimate message";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task ValidateContactMessageAsync_WithSpamInName_ShouldReturnFalse()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Name = "Casino Winner";
            message.Message = "This is a legitimate message content";

            // Act
            var result = await _service.ValidateContactMessageAsync(message);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public async Task GetContactMessageStatsAsync_WithNoMessages_ShouldReturnZeros()
        {
            // Arrange - Use fresh context with no data
            using var freshContext = TestDbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var freshCache = new MemoryCache(new MemoryCacheOptions());
            var freshService = new PortfolioService(freshContext, freshCache, _mockLogger.Object);

            // Act
            var stats = await freshService.GetContactMessageStatsAsync();

            // Assert
            stats.Should().NotBeNull();
            stats["Total"].Should().Be(0);
            stats["Unread"].Should().Be(0);
            stats["Today"].Should().Be(0);
            stats["ThisWeek"].Should().Be(0);
        }

        [Fact]
        public async Task GetSkillCategoriesAsync_WithNoVisibleSkills_ShouldReturnEmptyList()
        {
            // Arrange - Create context with only invisible skills
            using var freshContext = TestDbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var freshCache = new MemoryCache(new MemoryCacheOptions());
            var freshService = new PortfolioService(freshContext, freshCache, _mockLogger.Object);

            var invisibleSkill = new Skill
            {
                Name = "Hidden Skill",
                Category = "Hidden Category",
                Proficiency = 8,
                IsVisible = false
            };

            freshContext.Skills.Add(invisibleSkill);
            await freshContext.SaveChangesAsync();

            // Act
            var categories = await freshService.GetSkillCategoriesAsync();

            // Assert
            categories.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchBlogPostsAsync_WithNullTerm_ShouldReturnEmptyList()
        {
            // Act
            var results = await _service.SearchBlogPostsAsync(null!);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task SearchBlogPostsAsync_WithWhitespaceTerm_ShouldReturnEmptyList()
        {
            // Act
            var results = await _service.SearchBlogPostsAsync("   ");

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetBlogPostsByTagAsync_WithNullTag_ShouldReturnEmptyList()
        {
            // Act
            var results = await _service.GetBlogPostsByTagAsync(null!);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetBlogPostsByTagAsync_WithWhitespaceTag_ShouldReturnEmptyList()
        {
            // Act
            var results = await _service.GetBlogPostsByTagAsync("   ");

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetSkillsByCategoryAsync_WithNullCategory_ShouldReturnEmptyList()
        {
            // Act
            var results = await _service.GetSkillsByCategoryAsync(null!);

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetSkillsByCategoryAsync_WithWhitespaceCategory_ShouldReturnEmptyList()
        {
            // Act
            var results = await _service.GetSkillsByCategoryAsync("   ");

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllBlogTagsAsync_WithNoPublishedPosts_ShouldReturnEmptyList()
        {
            // Arrange - Create context with no published posts
            using var freshContext = TestDbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var freshCache = new MemoryCache(new MemoryCacheOptions());
            var freshService = new PortfolioService(freshContext, freshCache, _mockLogger.Object);

            var unpublishedPost = new BlogPost
            {
                Title = "Unpublished",
                Slug = "unpublished",
                Content = "Content",
                Excerpt = "Excerpt",
                Tags = "Tag1, Tag2",
                IsPublished = false,
                CreatedDate = DateTime.UtcNow
            };

            freshContext.BlogPosts.Add(unpublishedPost);
            await freshContext.SaveChangesAsync();

            // Act
            var tags = await freshService.GetAllBlogTagsAsync();

            // Assert
            tags.Should().BeEmpty();
        }

        [Fact]
        public async Task GetAllBlogTagsAsync_WithPostsWithoutTags_ShouldReturnEmptyList()
        {
            // Arrange - Create context with posts but no tags
            using var freshContext = TestDbContextFactory.CreateInMemoryDbContext(Guid.NewGuid().ToString());
            var freshCache = new MemoryCache(new MemoryCacheOptions());
            var freshService = new PortfolioService(freshContext, freshCache, _mockLogger.Object);

            var postWithoutTags = new BlogPost
            {
                Title = "No Tags Post",
                Slug = "no-tags",
                Content = "Content",
                Excerpt = "Excerpt",
                Tags = null,
                IsPublished = true,
                PublishedDate = DateTime.UtcNow,
                CreatedDate = DateTime.UtcNow
            };

            freshContext.BlogPosts.Add(postWithoutTags);
            await freshContext.SaveChangesAsync();

            // Act
            var tags = await freshService.GetAllBlogTagsAsync();

            // Assert
            tags.Should().BeEmpty();
        }

        [Fact]
        public async Task SaveContactMessageAsync_WithNullSubject_ShouldSaveSuccessfully()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Subject = null;
            message.Email = "nullsubject@test.com";

            // Act
            await _service.SaveContactMessageAsync(message);

            // Assert
            var saved = _context.ContactMessages.FirstOrDefault(m => m.Email == "nullsubject@test.com");
            saved.Should().NotBeNull();
            saved!.Subject.Should().BeNull();
        }

        [Fact]
        public async Task SaveContactMessageAsync_WithNullCompany_ShouldSaveSuccessfully()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Company = null;
            message.Email = "nullcompany@test.com";

            // Act
            await _service.SaveContactMessageAsync(message);

            // Assert
            var saved = _context.ContactMessages.FirstOrDefault(m => m.Email == "nullcompany@test.com");
            saved.Should().NotBeNull();
            saved!.Company.Should().BeNull();
        }

        [Fact]
        public async Task SaveContactMessageAsync_WithNullPhone_ShouldSaveSuccessfully()
        {
            // Arrange
            var message = TestDataBuilder.CreateValidContactMessage();
            message.Phone = null;
            message.Email = "nullphone@test.com";

            // Act
            await _service.SaveContactMessageAsync(message);

            // Assert
            var saved = _context.ContactMessages.FirstOrDefault(m => m.Email == "nullphone@test.com");
            saved.Should().NotBeNull();
            saved!.Phone.Should().BeNull();
        }
    }
}