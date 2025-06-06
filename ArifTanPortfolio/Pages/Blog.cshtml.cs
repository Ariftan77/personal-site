// Pages/Blog.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArifTanPortfolio.Models;
using ArifTanPortfolio.Services;

namespace ArifTanPortfolio.Pages
{
    public class BlogModel : PageModel
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ILogger<BlogModel> _logger;

        public BlogModel(IPortfolioService portfolioService, ILogger<BlogModel> logger)
        {
            _portfolioService = portfolioService;
            _logger = logger;
        }

        public List<BlogPost> BlogPosts { get; set; } = new();
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; } = 1;
        public int PageSize { get; set; } = 9;

        public async Task OnGetAsync(int page = 1)
        {
            try
            {
                CurrentPage = page;
                
                // Load all published blog posts
                var allPosts = await _portfolioService.GetPublishedBlogPostsAsync();
                
                // Calculate pagination
                var totalPosts = allPosts.Count;
                TotalPages = (int)Math.Ceiling((double)totalPosts / PageSize);
                
                // Get posts for current page
                BlogPosts = allPosts
                    .Skip((CurrentPage - 1) * PageSize)
                    .Take(PageSize)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading blog posts");
                BlogPosts = new List<BlogPost>();
            }
        }
    }
}