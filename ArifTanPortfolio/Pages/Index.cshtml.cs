// Pages/Index.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArifTanPortfolio.Models;
using ArifTanPortfolio.Models.ViewModels;
using ArifTanPortfolio.Services;

namespace ArifTanPortfolio.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ILogger<IndexModel> _logger;

        public IndexModel(IPortfolioService portfolioService, ILogger<IndexModel> logger)
        {
            _portfolioService = portfolioService;
            _logger = logger;
        }

        // Public properties that the view can access
        public List<Project> FeaturedProjects { get; set; } = new();
        public List<Skill> TopSkills { get; set; } = new();
        public List<BlogPost> RecentBlogPosts { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                // Load data for homepage
                FeaturedProjects = await _portfolioService.GetFeaturedProjectsAsync();
                TopSkills = await _portfolioService.GetHomePageSkillsAsync();
                RecentBlogPosts = await _portfolioService.GetRecentBlogPostsAsync(3);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading homepage data");
                // Initialize empty lists to prevent null reference errors
                FeaturedProjects = new List<Project>();
                TopSkills = new List<Skill>();
                RecentBlogPosts = new List<BlogPost>();
            }
        }
    }
}