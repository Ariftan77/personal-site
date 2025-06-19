// Pages/RssFeed.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArifTanPortfolio.Models;
using ArifTanPortfolio.Services;

namespace ArifTanPortfolio.Pages
{
    public class RssFeedModel : PageModel
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ILogger<RssFeedModel> _logger;

        public RssFeedModel(IPortfolioService portfolioService, ILogger<RssFeedModel> logger)
        {
            _portfolioService = portfolioService;
            _logger = logger;
        }

        public List<BlogPost> BlogPosts { get; set; } = new();
        public string Domain { get; set; } = "ariftan.dev";

        public async Task OnGetAsync()
        {
            try
            {
                // Get the most recent 20 blog posts for RSS feed
                var allPosts = await _portfolioService.GetPublishedBlogPostsAsync();
                BlogPosts = allPosts.Take(20).ToList();

                // Set domain from request if available
                if (HttpContext.Request.Host.HasValue)
                {
                    Domain = HttpContext.Request.Host.Value;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error generating RSS feed");
                BlogPosts = new List<BlogPost>();
            }
        }
    }
}