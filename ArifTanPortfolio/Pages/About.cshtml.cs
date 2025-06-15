// Pages/About.cshtml.cs
using ArifTanPortfolio.Models;
using ArifTanPortfolio.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArifTanPortfolio.Pages
{
    public class AboutModel : PageModel
    {
        private readonly ILogger<AboutModel> _logger;
        private readonly IPortfolioService _portfolioService;

        public AboutModel(ILogger<AboutModel> logger, IPortfolioService portfolioService)
        {
            _logger = logger;
            _portfolioService = portfolioService;
        }
        public List<Skill> ProgrammingLanguages { get; set; } = new();
        public List<Skill> Frameworks { get; set; } = new();
        public List<Skill> Databases { get; set; } = new();
        public List<Skill> Frontends { get; set; } = new();
        public List<Skill> CloudsAndDevOps { get; set; } = new();

        public async Task OnGet()
        {
            try
            {
                // Load top skills for the About page
                ProgrammingLanguages = await _portfolioService.GetSkillsByCategoryAsync("Programming Languages");
                Frameworks = await _portfolioService.GetSkillsByCategoryAsync("Frameworks");
                Databases = await _portfolioService.GetSkillsByCategoryAsync("Databases");
                CloudsAndDevOps = await _portfolioService.GetSkillsByCategoryAsync("Cloud & DevOps");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading top skills for About page");
                // Initialize empty list to prevent null reference errors
                ProgrammingLanguages = new List<Skill>();
                Frameworks = new List<Skill>();
                Databases = new List<Skill>();
                CloudsAndDevOps = new List<Skill>();
            }
        }
    }
}