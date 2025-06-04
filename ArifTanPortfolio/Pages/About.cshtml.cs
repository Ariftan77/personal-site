// Pages/About.cshtml.cs
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ArifTanPortfolio.Pages
{
    public class AboutModel : PageModel
    {
        private readonly ILogger<AboutModel> _logger;

        public AboutModel(ILogger<AboutModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            // About page doesn't need dynamic data currently
            // Can be extended to load certifications, timeline data from database
        }
    }
}