using ArifTanPortfolio.Models;

namespace ArifTanPortfolio.Services
{
    public interface IPortfolioService
    {
        Task<List<Project>> GetFeaturedProjectsAsync();
        Task<List<Project>> GetAllProjectsAsync();
        Task<Project?> GetProjectByIdAsync(int id);
        Task<List<BlogPost>> GetRecentBlogPostsAsync(int count = 3);
        Task<List<BlogPost>> GetPublishedBlogPostsAsync();
        Task<BlogPost?> GetBlogPostBySlugAsync(string slug);
        Task<List<Skill>> GetSkillsByCategoryAsync(string category);
        Task<List<Skill>> GetTopSkillsAsync(int count = 8);
        Task SaveContactMessageAsync(ContactMessage message);
    }
}