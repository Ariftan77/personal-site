// Program.cs
using Microsoft.EntityFrameworkCore;
using ArifTanPortfolio.Data;
using ArifTanPortfolio.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorPages();

// Add Entity Framework
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add custom services
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPortfolioService, PortfolioService>();

// Add memory cache
builder.Services.AddMemoryCache();

// Add HTTP client for external API calls if needed
builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Configure routing
app.MapRazorPages();

// Custom route for blog posts
app.MapGet("/blog/{slug}", async (string slug, IPortfolioService portfolioService) =>
{
    var post = await portfolioService.GetBlogPostBySlugAsync(slug);
    if (post == null)
    {
        return Results.NotFound();
    }
    return Results.Redirect($"/Blog/Post?slug={slug}");
});

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.EnsureCreated();
}

app.Run();