// Pages/BlogPost.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ArifTanPortfolio.Models;
using ArifTanPortfolio.Services;
using Markdig;

namespace ArifTanPortfolio.Pages
{
    public class BlogPostModel : PageModel
    {
        private readonly IPortfolioService _portfolioService;
        private readonly ILogger<BlogPostModel> _logger;
        private readonly MarkdownPipeline _markdownPipeline;

        public BlogPostModel(IPortfolioService portfolioService, ILogger<BlogPostModel> logger)
        {
            _portfolioService = portfolioService;
            _logger = logger;

            // Configure Markdig pipeline with syntax highlighting and advanced features
            _markdownPipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                // .UseSyntaxHighlighting()
                .UseEmojiAndSmiley()
                .UseBootstrap()
                .Build();
        }

        [BindProperty(SupportsGet = true)]
        public string Slug { get; set; } = string.Empty;

        public BlogPost? CurrentPost { get; set; }
        public List<BlogPost> RelatedPosts { get; set; } = new();
        public string RenderedContent { get; set; } = string.Empty;
        public int ReadingTimeMinutes { get; set; }
        public BlogPost? PreviousPost { get; set; }
        public BlogPost? NextPost { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            if (string.IsNullOrEmpty(Slug))
            {
                return NotFound();
            }

            try
            {
                // Get the blog post by slug
                CurrentPost = await _portfolioService.GetBlogPostBySlugAsync(Slug);

                if (CurrentPost == null || !CurrentPost.IsPublished)
                {
                    return NotFound();
                }

                // Render markdown content to HTML
                RenderedContent = Markdown.ToHtml(CurrentPost.Content ?? string.Empty, _markdownPipeline);

                // Calculate reading time (average 200 words per minute)
                ReadingTimeMinutes = CurrentPost.ReadTimeMinutes == 0 ? CalculateReadingTime(CurrentPost.Content ?? string.Empty) : CurrentPost.ReadTimeMinutes;

                // Get related posts (same tags, excluding current post)
                RelatedPosts = await _portfolioService.GetRelatedBlogPostsAsync(CurrentPost.Id, CurrentPost.Tags, 3);

                // Get previous and next posts for navigation
                var navigationPosts = await _portfolioService.GetAdjacentBlogPostsAsync(CurrentPost.Id);
                PreviousPost = navigationPosts.Previous;
                NextPost = navigationPosts.Next;

                // Update view count (optional)
                await _portfolioService.IncrementBlogPostViewsAsync(CurrentPost.Id);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading blog post with slug: {Slug}", Slug);
                return NotFound();
            }
        }

        private static int CalculateReadingTime(string content)
        {
            if (string.IsNullOrEmpty(content))
                return 1;

            // Remove markdown syntax for more accurate word count
            var plainText = System.Text.RegularExpressions.Regex.Replace(content, @"\[([^\]]*)\]\([^\)]*\)", "$1");
            plainText = System.Text.RegularExpressions.Regex.Replace(plainText, @"[#\*_`]", "");

            var wordCount = plainText.Split(new[] { ' ', '\n', '\r', '\t' }, StringSplitOptions.RemoveEmptyEntries).Length;
            var readingTime = Math.Max(1, (int)Math.Ceiling(wordCount / 200.0));

            return readingTime;
        }
    }
}