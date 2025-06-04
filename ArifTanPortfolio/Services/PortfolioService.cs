using Microsoft.EntityFrameworkCore;
using ArifTanPortfolio.Data;
using ArifTanPortfolio.Models;

namespace ArifTanPortfolio.Services
{
    public class PortfolioService : IPortfolioService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<PortfolioService> _logger;

        public PortfolioService(ApplicationDbContext context, ILogger<PortfolioService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Project>> GetFeaturedProjectsAsync()
        {
            return await _context.Projects
                .Where(p => p.IsFeatured)
                .OrderBy(p => p.SortOrder)
                .ToListAsync();
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _context.Projects
                .OrderBy(p => p.SortOrder)
                .ThenByDescending(p => p.StartDate)
                .ToListAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _context.Projects.FindAsync(id);
        }

        public async Task<List<BlogPost>> GetRecentBlogPostsAsync(int count = 3)
        {
            return await _context.BlogPosts
                .Where(b => b.IsPublished)
                .OrderByDescending(b => b.PublishedDate)
                .Take(count)
                .ToListAsync();
        }

        public async Task<List<BlogPost>> GetPublishedBlogPostsAsync()
        {
            return await _context.BlogPosts
                .Where(b => b.IsPublished)
                .OrderByDescending(b => b.PublishedDate)
                .ToListAsync();
        }

        public async Task<BlogPost?> GetBlogPostBySlugAsync(string slug)
        {
            return await _context.BlogPosts
                .FirstOrDefaultAsync(b => b.Slug == slug && b.IsPublished);
        }

        public async Task<List<Skill>> GetSkillsByCategoryAsync(string category)
        {
            return await _context.Skills
                .Where(s => s.Category == category && s.IsVisible)
                .OrderBy(s => s.SortOrder)
                .ToListAsync();
        }

        public async Task<List<Skill>> GetTopSkillsAsync(int count = 8)
        {
            return await _context.Skills
                .Where(s => s.IsVisible)
                .OrderByDescending(s => s.Proficiency)
                .ThenBy(s => s.SortOrder)
                .Take(count)
                .ToListAsync();
        }

        public async Task SaveContactMessageAsync(ContactMessage message)
        {
            message.DateSent = DateTime.UtcNow;
            _context.ContactMessages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}