// Pages/Portfolio.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArifTanPortfolio.Models;
using ArifTanPortfolio.Services;

namespace ArifTanPortfolio.Pages
{
    public class PortfolioModel : PageModel
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ILogger<PortfolioModel> _logger;

        public PortfolioModel(IPortfolioService portfolioService, ILogger<PortfolioModel> logger)
        {
            _portfolioService = portfolioService;
            _logger = logger;
        }

        public List<Project> Projects { get; set; } = new();
        public List<string> Categories { get; set; } = new();

        public async Task OnGetAsync()
        {
            try
            {
                // Load all projects
                Projects = await _portfolioService.GetAllProjectsAsync();
                
                // Get unique categories for filtering
                Categories = Projects
                    .Select(p => p.Category)
                    .Distinct()
                    .OrderBy(c => c)
                    .ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading portfolio data");
                Projects = new List<Project>();
                Categories = new List<string>();
            }
        }
    }
}