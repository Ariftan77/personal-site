// Pages/Blog/Post.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArifTanPortfolio.Models;
using ArifTanPortfolio.Services;

namespace ArifTanPortfolio.Pages.Blog
{
    public class BlogPostModel : PageModel
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ILogger<BlogPostModel> _logger;

        public BlogPostModel(IPortfolioService portfolioService, ILogger<BlogPostModel> logger)
        {
            _portfolioService = portfolioService;
            _logger = logger;
        }

        public BlogPost? Post { get; set; }
        public List<BlogPost> RelatedPosts { get; set; } = new();
        public BlogPost? PreviousPost { get; set; }
        public BlogPost? NextPost { get; set; }

        public async Task<IActionResult> OnGetAsync(string slug)
        {
            if (string.IsNullOrEmpty(slug))
            {
                return NotFound();
            }

            try
            {
                // Get the blog post by slug
                Post = await _portfolioService.GetBlogPostBySlugAsync(slug);
                
                if (Post == null)
                {
                    return NotFound();
                }

                // Get all published posts for navigation and related posts
                var allPosts = await _portfolioService.GetPublishedBlogPostsAsync();
                
                // Find previous and next posts
                var currentIndex = allPosts.FindIndex(p => p.Id == Post.Id);
                if (currentIndex > 0)
                {
                    NextPost = allPosts[currentIndex - 1]; // Next is more recent
                }
                if (currentIndex < allPosts.Count - 1)
                {
                    PreviousPost = allPosts[currentIndex + 1]; // Previous is older
                }

                // Get related posts (same tags or category)
                RelatedPosts = GetRelatedPosts(Post, allPosts);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading blog post with slug: {Slug}", slug);
                return NotFound();
            }
        }

        private List<BlogPost> GetRelatedPosts(BlogPost currentPost, List<BlogPost> allPosts)
        {
            var relatedPosts = new List<BlogPost>();
            
            if (string.IsNullOrEmpty(currentPost.Tags))
            {
                // If no tags, just return recent posts
                return allPosts
                    .Where(p => p.Id != currentPost.Id)
                    .Take(3)
                    .ToList();
            }

            var currentTags = currentPost.Tags.Split(',')
                .Select(t => t.Trim().ToLower())
                .ToList();

            // Find posts with matching tags
            var postsWithScores = allPosts
                .Where(p => p.Id != currentPost.Id && !string.IsNullOrEmpty(p.Tags))
                .Select(p => new
                {
                    Post = p,
                    Score = p.Tags.Split(',')
                        .Select(t => t.Trim().ToLower())
                        .Count(tag => currentTags.Contains(tag))
                })
                .Where(x => x.Score > 0)
                .OrderByDescending(x => x.Score)
                .ThenByDescending(x => x.Post.PublishedDate)
                .Take(3)
                .Select(x => x.Post)
                .ToList();

            // If we don't have enough related posts, fill with recent posts
            if (postsWithScores.Count < 3)
            {
                var recentPosts = allPosts
                    .Where(p => p.Id != currentPost.Id && !postsWithScores.Any(rp => rp.Id == p.Id))
                    .Take(3 - postsWithScores.Count);
                
                postsWithScores.AddRange(recentPosts);
            }

            return postsWithScores;
        }
    }
}