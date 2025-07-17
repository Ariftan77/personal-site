using Microsoft.EntityFrameworkCore;
using ArifTanPortfolio.Data;
using ArifTanPortfolio.Models;
using Microsoft.Extensions.Caching.Memory;
using System.ComponentModel.DataAnnotations;

namespace ArifTanPortfolio.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMemoryCache _cache;
        private readonly ILogger<PortfolioService> _logger;
        
        // Cache keys
        private const string FEATURED_PROJECTS_KEY = "featured_projects";
        private const string ALL_PROJECTS_KEY = "all_projects";
        private const string SKILL_CATEGORIES_KEY = "skill_categories";
        private const string TOP_SKILLS_KEY = "top_skills";
        
        // Cache expiration times
        private readonly TimeSpan _defaultCacheExpiry = TimeSpan.FromMinutes(30);
        private readonly TimeSpan _longCacheExpiry = TimeSpan.FromHours(2);

        public PortfolioService(
            ApplicationDbContext context, 
            IMemoryCache cache,
            ILogger<PortfolioService> logger)
        {
            _context = context;
            _cache = cache;
            _logger = logger;
        }

        // =======================================================================================
        // EXISTING METHODS - ENHANCED WITH CACHING & ERROR HANDLING
        // =======================================================================================

        public async Task<List<Project>> GetFeaturedProjectsAsync()
        {
            try
            {
                // Check cache first
                if (_cache.TryGetValue(FEATURED_PROJECTS_KEY, out List<Project>? cachedProjects))
                {
                    _logger.LogDebug("Retrieved featured projects from cache");
                    return cachedProjects!;
                }

                // Get from database with optimizations
                var projects = await _context.Projects
                    .Where(p => p.IsFeatured)
                    .OrderBy(p => p.SortOrder)
                    .AsNoTracking()
                    .ToListAsync();

                // Cache the results
                _cache.Set(FEATURED_PROJECTS_KEY, projects, _defaultCacheExpiry);
                _logger.LogInformation("Retrieved {Count} featured projects from database", projects.Count);
                
                return projects;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving featured projects");
                // Return empty list instead of throwing - graceful degradation
                return new List<Project>();
            }
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            try
            {
                if (_cache.TryGetValue(ALL_PROJECTS_KEY, out List<Project>? cachedProjects))
                {
                    return cachedProjects!;
                }

                var projects = await _context.Projects
                    .OrderBy(p => p.SortOrder)
                    .ThenByDescending(p => p.StartDate)
                    .AsNoTracking()
                    .ToListAsync();

                _cache.Set(ALL_PROJECTS_KEY, projects, _defaultCacheExpiry);
                _logger.LogInformation("Retrieved {Count} total projects", projects.Count);
                
                return projects;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all projects");
                return new List<Project>();
            }
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid project ID: {ProjectId}", id);
                    return null;
                }

                var project = await _context.Projects
                    .AsNoTracking()
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (project == null)
                {
                    _logger.LogWarning("Project not found with ID: {ProjectId}", id);
                }

                return project;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving project with ID {ProjectId}", id);
                return null;
            }
        }

        public async Task<List<BlogPost>> GetRecentBlogPostsAsync(int count = 3)
        {
            try
            {
                if (count <= 0 || count > 50)
                {
                    _logger.LogWarning("Invalid count parameter: {Count}", count);
                    count = 3; // Default fallback
                }

                var cacheKey = $"recent_blog_posts_{count}";
                if (_cache.TryGetValue(cacheKey, out List<BlogPost>? cachedPosts))
                {
                    return cachedPosts!;
                }

                var posts = await _context.BlogPosts
                    .Where(b => b.IsPublished)
                    .OrderByDescending(b => b.PublishedDate)
                    .Take(count)
                    .AsNoTracking()
                    .ToListAsync();

                _cache.Set(cacheKey, posts, _defaultCacheExpiry);
                _logger.LogInformation("Retrieved {Count} recent blog posts", posts.Count);
                
                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving recent blog posts");
                return new List<BlogPost>();
            }
        }

        public async Task<List<BlogPost>> GetPublishedBlogPostsAsync()
        {
            try
            {
                const string cacheKey = "published_blog_posts";
                if (_cache.TryGetValue(cacheKey, out List<BlogPost>? cachedPosts))
                {
                    return cachedPosts!;
                }

                var posts = await _context.BlogPosts
                    .Where(b => b.IsPublished)
                    .OrderByDescending(b => b.PublishedDate)
                    .AsNoTracking()
                    .ToListAsync();

                _cache.Set(cacheKey, posts, _defaultCacheExpiry);
                _logger.LogInformation("Retrieved {Count} published blog posts", posts.Count);
                
                return posts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving published blog posts");
                return new List<BlogPost>();
            }
        }

        // public async Task<BlogPost?> GetBlogPostBySlugAsync(string slug)
        // {
        //     try
        //     {
        //         if (string.IsNullOrWhiteSpace(slug))
        //         {
        //             _logger.LogWarning("Invalid slug parameter: {Slug}", slug);
        //             return null;
        //         }

        //         var normalizedSlug = slug.Trim().ToLowerInvariant();
        //         var cacheKey = $"blog_post_{normalizedSlug}";
                
        //         if (_cache.TryGetValue(cacheKey, out BlogPost? cachedPost))
        //         {
        //             return cachedPost;
        //         }

        //         var post = await _context.BlogPosts
        //             .AsNoTracking()
        //             .FirstOrDefaultAsync(b => b.Slug.ToLower() == normalizedSlug && b.IsPublished);

        //         if (post != null)
        //         {
        //             _cache.Set(cacheKey, post, _longCacheExpiry);
        //             _logger.LogInformation("Retrieved blog post by slug: {Slug}", slug);
        //         }
        //         else
        //         {
        //             _logger.LogWarning("Blog post not found with slug: {Slug}", slug);
        //         }

        //         return post;
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, "Error retrieving blog post with slug {Slug}", slug);
        //         return null;
        //     }
        // }

        public async Task<List<Skill>> GetSkillsByCategoryAsync(string category)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(category))
                {
                    _logger.LogWarning("Invalid category parameter: {Category}", category);
                    return new List<Skill>();
                }

                var normalizedCategory = category.Trim();
                var cacheKey = $"skills_category_{normalizedCategory}";
                
                if (_cache.TryGetValue(cacheKey, out List<Skill>? cachedSkills))
                {
                    return cachedSkills!;
                }

                var skills = await _context.Skills
                    .Where(s => s.Category == normalizedCategory && s.IsVisible)
                    .OrderByDescending(s => s.Proficiency)
                    .ThenBy(s => s.SortOrder)
                    .AsNoTracking()
                    .ToListAsync();

                _cache.Set(cacheKey, skills, _longCacheExpiry);
                _logger.LogInformation("Retrieved {Count} skills for category: {Category}", 
                    skills.Count, category);
                
                return skills;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving skills for category {Category}", category);
                return new List<Skill>();
            }
        }

        public async Task<List<Skill>> GetTopSkillsAsync(int count = 8)
        {
            try
            {
                if (count <= 0 || count > 100)
                {
                    _logger.LogWarning("Invalid count parameter: {Count}", count);
                    count = 8; // Default fallback
                }

                var cacheKey = $"{TOP_SKILLS_KEY}_{count}";
                if (_cache.TryGetValue(cacheKey, out List<Skill>? cachedSkills))
                {
                    return cachedSkills!;
                }

                var skills = await _context.Skills
                    .Where(s => s.IsVisible)
                    .OrderByDescending(s => s.Proficiency)
                    .ThenBy(s => s.SortOrder)
                    .Take(count)
                    .AsNoTracking()
                    .ToListAsync();

                _cache.Set(cacheKey, skills, _longCacheExpiry);
                _logger.LogInformation("Retrieved top {Count} skills", skills.Count);
                
                return skills;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving top skills");
                return new List<Skill>();
            }
        }

        public async Task<List<Skill>> GetHomePageSkillsAsync()
        {
            try
            {
                const string cacheKey = "homepage_skills";
                if (_cache.TryGetValue(cacheKey, out List<Skill>? cachedSkills))
                {
                    return cachedSkills!;
                }

                var skills = await _context.Skills
                    .Where(s => s.IsShowOnHomePage)
                    .OrderByDescending(s => s.Proficiency)
                    .ThenBy(s => s.SortOrder)
                    .AsNoTracking()
                    .ToListAsync();

                _cache.Set(cacheKey, skills, _longCacheExpiry);
                _logger.LogInformation("Retrieved {Count} homepage skills", skills.Count);
                
                return skills;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving homepage skills");
                return new List<Skill>();
            }
        }

        public async Task SaveContactMessageAsync(ContactMessage message)
        {
            try
            {
                // Validate input
                if (!await ValidateContactMessageAsync(message))
                {
                    _logger.LogWarning("Contact message validation failed for {Email}", message.Email);
                    throw new ValidationException("Contact message validation failed");
                }

                // Sanitize and normalize data
                message.Name = message.Name.Trim();
                message.Email = message.Email.Trim().ToLowerInvariant();
                message.Subject = message.Subject?.Trim();
                message.Message = message.Message.Trim();
                message.Company = message.Company?.Trim();
                message.Phone = message.Phone?.Trim();
                message.DateSent = DateTime.UtcNow;
                message.IsRead = false;

                // Check for rate limiting (simple spam protection)
                var recentMessageCount = await _context.ContactMessages
                    .Where(cm => cm.Email == message.Email && 
                                cm.DateSent > DateTime.UtcNow.AddHours(-1))
                    .CountAsync();

                if (recentMessageCount >= 3)
                {
                    _logger.LogWarning("Rate limit exceeded for email: {Email}", message.Email);
                    throw new InvalidOperationException("Too many messages sent recently. Please try again later.");
                }

                _context.ContactMessages.Add(message);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Contact message saved successfully from {Email} with ID {MessageId}", 
                    message.Email, message.Id);
            }
            catch (Exception ex) when (!(ex is ValidationException || ex is InvalidOperationException))
            {
                _logger.LogError(ex, "Error saving contact message from {Email}", message.Email);
                throw new Exception("Failed to save contact message", ex);
            }
        }

        // =======================================================================================
        // NEW ENHANCED METHODS
        // =======================================================================================

        public async Task<List<string>> GetSkillCategoriesAsync()
        {
            try
            {
                if (_cache.TryGetValue(SKILL_CATEGORIES_KEY, out List<string>? cachedCategories))
                {
                    return cachedCategories!;
                }

                var categories = await _context.Skills
                    .Where(s => s.IsVisible)
                    .Select(s => s.Category)
                    .Distinct()
                    .OrderBy(c => c)
                    .AsNoTracking()
                    .ToListAsync();

                _cache.Set(SKILL_CATEGORIES_KEY, categories, _longCacheExpiry);
                _logger.LogInformation("Retrieved {Count} skill categories", categories.Count);
                
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving skill categories");
                return new List<string>();
            }
        }

        public async Task<Dictionary<string, int>> GetContactMessageStatsAsync()
        {
            try
            {
                var stats = new Dictionary<string, int>();
                
                var totalMessages = await _context.ContactMessages.CountAsync();
                var unreadMessages = await _context.ContactMessages.Where(cm => !cm.IsRead).CountAsync();
                var todayMessages = await _context.ContactMessages
                    .Where(cm => cm.DateSent.Date == DateTime.UtcNow.Date)
                    .CountAsync();
                var thisWeekMessages = await _context.ContactMessages
                    .Where(cm => cm.DateSent >= DateTime.UtcNow.AddDays(-7))
                    .CountAsync();

                stats.Add("Total", totalMessages);
                stats.Add("Unread", unreadMessages);
                stats.Add("Today", todayMessages);
                stats.Add("ThisWeek", thisWeekMessages);

                _logger.LogInformation("Retrieved contact message statistics");
                return stats;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving contact message statistics");
                return new Dictionary<string, int>();
            }
        }

        public async Task<bool> ValidateContactMessageAsync(ContactMessage message)
        {
            try
            {
                // Standard model validation
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(message);
                
                if (!Validator.TryValidateObject(message, validationContext, validationResults, true))
                {
                    var errors = validationResults.Select(vr => vr.ErrorMessage ?? "Unknown validation error").ToList();
                    _logger.LogWarning("Contact message validation failed: {Errors}", string.Join(", ", errors));
                    return false;
                }

                // Additional business validation
                if (string.IsNullOrWhiteSpace(message.Name) || message.Name.Length < 2)
                {
                    _logger.LogWarning("Invalid name in contact message: {Name}", message.Name);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(message.Email) || !IsValidEmail(message.Email))
                {
                    _logger.LogWarning("Invalid email in contact message: {Email}", message.Email);
                    return false;
                }

                if (string.IsNullOrWhiteSpace(message.Message) || message.Message.Length < 10)
                {
                    _logger.LogWarning("Invalid message content in contact message");
                    return false;
                }

                // Simple spam detection
                var messageContent = (message.Name + " " + message.Message + " " + message.Subject).ToLowerInvariant();
                var suspiciousPatterns = new[] { "viagra", "casino", "loan", "bitcoin", "make money fast" };
                
                if (suspiciousPatterns.Any(pattern => messageContent.Contains(pattern)))
                {
                    _logger.LogWarning("Suspicious content detected in contact message from {Email}", message.Email);
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating contact message");
                return false;
            }
        }
        // New blog-specific methods
        public async Task<BlogPost?> GetBlogPostBySlugAsync(string slug)
        {
            return await _context.BlogPosts
                .Where(bp => bp.Slug == slug && bp.IsPublished)
                .FirstOrDefaultAsync();
        }

        public async Task<List<BlogPost>> GetRelatedBlogPostsAsync(int currentPostId, string? tags, int count = 3)
        {
            try
            {
                if (string.IsNullOrEmpty(tags))
                {
                    // If no tags, return recent posts excluding current
                    return await _context.BlogPosts
                        .Where(bp => bp.IsPublished && bp.Id != currentPostId)
                        .OrderByDescending(bp => bp.PublishedDate)
                        .Take(count)
                        .ToListAsync();
                }

                var tagList = tags.Split(',')
                    .Select(t => t.Trim().ToLower())
                    .Where(t => !string.IsNullOrEmpty(t))
                    .ToList();

                if (!tagList.Any())
                {
                    // If no valid tags after processing, return recent posts
                    return await _context.BlogPosts
                        .Where(bp => bp.IsPublished && bp.Id != currentPostId)
                        .OrderByDescending(bp => bp.PublishedDate)
                        .Take(count)
                        .ToListAsync();
                }

                // Get candidates from database first (limit to reasonable number for performance)
                var candidatePosts = await _context.BlogPosts
                    .Where(bp => bp.IsPublished && 
                                bp.Id != currentPostId && 
                                !string.IsNullOrEmpty(bp.Tags))
                    .OrderByDescending(bp => bp.PublishedDate)
                    .Take(50) // Limit candidates for performance
                    .ToListAsync();

                // Score posts by number of matching tags
                var scoredPosts = candidatePosts
                    .Select(post => new
                    {
                        Post = post,
                        Score = post.Tags!.Split(',')
                            .Select(t => t.Trim().ToLower())
                            .Count(postTag => tagList.Contains(postTag))
                    })
                    .Where(sp => sp.Score > 0) // Only posts with at least one matching tag
                    .OrderByDescending(sp => sp.Score) // Order by relevance
                    .ThenByDescending(sp => sp.Post.PublishedDate) // Then by recency
                    .Select(sp => sp.Post)
                    .Take(count)
                    .ToList();

                // If we don't have enough related posts, fill with recent posts
                if (scoredPosts.Count < count)
                {
                    var usedIds = scoredPosts.Select(p => p.Id).ToList();
                    var additionalPosts = candidatePosts
                        .Where(p => !usedIds.Contains(p.Id))
                        .Take(count - scoredPosts.Count)
                        .ToList();

                    scoredPosts.AddRange(additionalPosts);
                }

                return scoredPosts;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting related blog posts for post {PostId}", currentPostId);
                
                // Fallback to recent posts
                return await _context.BlogPosts
                    .Where(bp => bp.IsPublished && bp.Id != currentPostId)
                    .OrderByDescending(bp => bp.PublishedDate)
                    .Take(count)
                    .ToListAsync();
            }
        }

        public async Task<(BlogPost? Previous, BlogPost? Next)> GetAdjacentBlogPostsAsync(int currentPostId)
        {
            var currentPost = await _context.BlogPosts
                .Where(bp => bp.Id == currentPostId && bp.IsPublished)
                .FirstOrDefaultAsync();

            if (currentPost?.PublishedDate == null)
                return (null, null);

            var previousPost = await _context.BlogPosts
                .Where(bp => bp.IsPublished && bp.PublishedDate < currentPost.PublishedDate)
                .OrderByDescending(bp => bp.PublishedDate)
                .FirstOrDefaultAsync();

            var nextPost = await _context.BlogPosts
                .Where(bp => bp.IsPublished && bp.PublishedDate > currentPost.PublishedDate)
                .OrderBy(bp => bp.PublishedDate)
                .FirstOrDefaultAsync();

            return (previousPost, nextPost);
        }

        public async Task IncrementBlogPostViewsAsync(int blogPostId)
        {
            try
            {
                var post = await _context.BlogPosts.FindAsync(blogPostId);
                if (post != null)
                {
                    post.ViewCount++;
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error incrementing view count for blog post {BlogPostId}", blogPostId);
                // Don't throw - view count increment shouldn't break page loading
            }
        }

        public async Task<List<BlogPost>> SearchBlogPostsAsync(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new List<BlogPost>();

            var lowerSearchTerm = searchTerm.ToLower();
            
            return await _context.BlogPosts
                .Where(bp => bp.IsPublished && 
                           (bp.Title.ToLower().Contains(lowerSearchTerm) ||
                            bp.Content.ToLower().Contains(lowerSearchTerm) ||
                            bp.Excerpt.ToLower().Contains(lowerSearchTerm) ||
                            (bp.Tags != null && bp.Tags.ToLower().Contains(lowerSearchTerm))))
                .OrderByDescending(bp => bp.PublishedDate)
                .ToListAsync();
        }

        public async Task<List<BlogPost>> GetBlogPostsByTagAsync(string tag)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tag))
                    return new List<BlogPost>();

                var normalizedTag = tag.Trim().ToLower();
                
                // Get candidate posts from database first
                var candidatePosts = await _context.BlogPosts
                    .Where(bp => bp.IsPublished && 
                                !string.IsNullOrEmpty(bp.Tags) &&
                                bp.Tags.ToLower().Contains(normalizedTag))
                    .OrderByDescending(bp => bp.PublishedDate)
                    .ToListAsync();

                // Refine with exact tag matching on client side
                var exactMatches = candidatePosts
                    .Where(bp => bp.Tags!.Split(',')
                        .Select(t => t.Trim().ToLower())
                        .Contains(normalizedTag))
                    .ToList();

                return exactMatches;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting blog posts by tag: {Tag}", tag);
                return new List<BlogPost>();
            }
        }

        public async Task<List<string>> GetAllBlogTagsAsync()
        {
            var allTags = await _context.BlogPosts
                .Where(bp => bp.IsPublished && !string.IsNullOrEmpty(bp.Tags))
                .Select(bp => bp.Tags)
                .ToListAsync();

            return allTags
                .SelectMany(tags => tags.Split(','))
                .Select(tag => tag.Trim())
                .Where(tag => !string.IsNullOrEmpty(tag))
                .Distinct()
                .OrderBy(tag => tag)
                .ToList();
        }

        public async Task<List<BlogPost>> GetBlogPostsByCategoryAsync(string category)
        {
            return await _context.BlogPosts
                .Where(bp => bp.IsPublished && bp.Category == category)
                .OrderByDescending(bp => bp.PublishedDate)
                .ToListAsync();
        }
        // =======================================================================================
        // HELPER METHODS
        // =======================================================================================

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Method to clear cache when data is updated
        public void ClearCache()
        {
            var cacheKeys = new[]
            {
                FEATURED_PROJECTS_KEY,
                ALL_PROJECTS_KEY,
                SKILL_CATEGORIES_KEY,
                TOP_SKILLS_KEY,
                "published_blog_posts"
            };

            foreach (var key in cacheKeys)
            {
                _cache.Remove(key);
            }

            _logger.LogInformation("Cache cleared for portfolio service");
        }
    }
}