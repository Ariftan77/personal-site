# Database Documentation

This document describes the database schema, Entity Framework configuration, and data management for the Arif Tan Portfolio website.

## 🗄️ Database Overview

The application uses **SQLite** for development and can use **PostgreSQL** for production deployment on Railway.

### Technology Stack
- **ORM**: Entity Framework Core 8.0
- **Database**: SQLite (dev) / PostgreSQL (prod)
- **Migrations**: Code-First approach
- **Seeding**: Automated seed data for development

## 📊 Database Schema

### Entity Relationship Overview

```
┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐
│   Projects  │    │  BlogPosts  │    │   Skills    │    │ContactMessages│
│             │    │             │    │             │    │             │
│ Id (PK)     │    │ Id (PK)     │    │ Id (PK)     │    │ Id (PK)     │
│ Name        │    │ Title       │    │ Name        │    │ Name        │
│ Description │    │ Slug        │    │ Category    │    │ Email       │
│ Category    │    │ Content     │    │ Proficiency │    │ Message     │
│ Technologies│    │ Category    │    │ IsVisible   │    │ DateSent    │
│ IsFeatured  │    │ Tags        │    │ SortOrder   │    │ IsRead      │
│ SortOrder   │    │ IsPublished │    │             │    │             │
└─────────────┘    └─────────────┘    └─────────────┘    └─────────────┘
```

## 🏗️ Entity Models

### Project Entity

```csharp
public class Project
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? LongDescription { get; set; }
    public string Technologies { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsFeatured { get; set; }
    public int SortOrder { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Challenges { get; set; }
    public string? Solutions { get; set; }
    public string? LessonsLearned { get; set; }
    public string? ImageGallery { get; set; }
    public string? FeaturedImage { get; set; }
    public string? LiveUrl { get; set; }
    public string? GitHubUrl { get; set; }
}
```

**Categories**:
- Enterprise Software
- Machine Learning
- Integration Platform
- Web Applications

### BlogPost Entity

```csharp
public class BlogPost
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string? Excerpt { get; set; }
    public string? Category { get; set; }
    public string? Tags { get; set; }
    public string? FeaturedImage { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedDate { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public int ReadTimeMinutes { get; set; }
    public int ViewCount { get; set; }
    public string? MetaDescription { get; set; }
    public string? MetaKeywords { get; set; }
    public string? Author { get; set; }
    public string? AuthorEmail { get; set; }
}
```

**Categories**:
- Technical Tutorial
- AI & Automation
- Career Development
- Software Architecture

### Skill Entity

```csharp
public class Skill
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Proficiency { get; set; } // 1-10 scale
    public string Category { get; set; } = string.Empty;
    public int SortOrder { get; set; }
    public string? Icon { get; set; }
    public string? Description { get; set; }
    public bool IsVisible { get; set; }
    public bool IsShowOnHomePage { get; set; }
}
```

**Categories**:
- Programming Languages
- Frameworks
- Cloud & DevOps
- Databases

### ContactMessage Entity

```csharp
public class ContactMessage
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Subject { get; set; }
    public string Message { get; set; } = string.Empty;
    public string? Company { get; set; }
    public string? Phone { get; set; }
    public DateTime DateSent { get; set; }
    public bool IsRead { get; set; }
}
```

## ⚙️ Entity Framework Configuration

### DbContext Configuration

```csharp
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<BlogPost> BlogPosts { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
    public DbSet<Skill> Skills { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure indexes for performance
        modelBuilder.Entity<BlogPost>(entity =>
        {
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.HasIndex(e => e.PublishedDate);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasIndex(e => e.SortOrder);
            entity.HasIndex(e => e.Category);
        });

        modelBuilder.Entity<ContactMessage>(entity =>
        {
            entity.HasIndex(e => e.DateSent);
        });

        modelBuilder.Entity<Skill>(entity =>
        {
            entity.HasIndex(e => e.Category);
            entity.HasIndex(e => e.SortOrder);
        });

        // Seed initial data
        SeedData(modelBuilder);
        SeedBlogPosts(modelBuilder);
    }
}
```

### Database Indexes

Strategic indexing for optimal query performance:

```sql
-- Blog posts
CREATE INDEX IX_BlogPosts_Slug ON BlogPosts (Slug);
CREATE INDEX IX_BlogPosts_PublishedDate ON BlogPosts (PublishedDate);

-- Projects
CREATE INDEX IX_Projects_SortOrder ON Projects (SortOrder);
CREATE INDEX IX_Projects_Category ON Projects (Category);

-- Skills
CREATE INDEX IX_Skills_Category ON Skills (Category);
CREATE INDEX IX_Skills_SortOrder ON Skills (SortOrder);

-- Contact messages
CREATE INDEX IX_ContactMessages_DateSent ON ContactMessages (DateSent);
```

## 🌱 Seed Data

### Skills Seed Data

```csharp
private void SeedData(ModelBuilder modelBuilder)
{
    modelBuilder.Entity<Skill>().HasData(
        // Programming Languages
        new Skill { 
            Id = 1, 
            Name = "C#", 
            Proficiency = 9, 
            Category = "Programming Languages", 
            SortOrder = 1, 
            Icon = "devicon-csharp-plain", 
            IsVisible = true, 
            IsShowOnHomePage = true 
        },
        new Skill { 
            Id = 2, 
            Name = "JavaScript", 
            Proficiency = 8, 
            Category = "Programming Languages", 
            SortOrder = 2, 
            Icon = "devicon-javascript-plain", 
            IsVisible = true, 
            IsShowOnHomePage = true 
        },
        // ... more skills
    );
}
```

### Projects Seed Data

```csharp
modelBuilder.Entity<Project>().HasData(
    new Project
    {
        Id = 1,
        Name = "Multi-Warehouse Management System",
        Description = "Comprehensive WMS with real-time location tracking...",
        Category = "Enterprise Software",
        IsFeatured = true,
        SortOrder = 1,
        Technologies = "ASP.NET Core 8.0, PostgreSQL, SignalR...",
        // ... other properties
    },
    // ... more projects
);
```

## 🔄 Migrations

### Migration Commands

```bash
# Create new migration
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Rollback migration
dotnet ef database update PreviousMigrationName

# Remove last migration
dotnet ef migrations remove

# Generate SQL script
dotnet ef migrations script

# Drop database (development only)
dotnet ef database drop
```

### Migration Files Structure

```
Migrations/
├── 20250717153726_UpdateSeedData.cs
├── 20250717153726_UpdateSeedData.Designer.cs
├── 20250717154915_AddIsShowOnHomePageToSkills.cs
├── 20250717154915_AddIsShowOnHomePageToSkills.Designer.cs
├── 20250718120000_UpdateProjectCategories.cs
├── 20250718120000_UpdateProjectCategories.Designer.cs
└── ApplicationDbContextModelSnapshot.cs
```

### Migration Best Practices

1. **Descriptive Names**: Use clear, descriptive migration names
2. **Review Changes**: Always review generated migration code
3. **Test Locally**: Test migrations in development first
4. **Backup Data**: Backup production data before major changes
5. **Rollback Plan**: Have a rollback strategy for production

## 📈 Performance Optimization

### Query Optimization

```csharp
// Efficient queries with proper indexing
public async Task<List<Project>> GetFeaturedProjectsAsync()
{
    return await _context.Projects
        .Where(p => p.IsFeatured) // Uses index
        .OrderBy(p => p.SortOrder) // Uses index
        .ToListAsync();
}

// Optimized blog post queries
public async Task<BlogPost?> GetBlogPostBySlugAsync(string slug)
{
    return await _context.BlogPosts
        .Where(b => b.Slug == slug && b.IsPublished) // Uses unique index
        .FirstOrDefaultAsync();
}
```

### Connection String Optimization

```csharp
// SQLite optimization
"Data Source=app.db;Cache=Shared;Pooling=true"

// PostgreSQL optimization
"Host=localhost;Database=portfolio;Username=user;Password=pass;Pooling=true;MinPoolSize=1;MaxPoolSize=20"
```

## 🔒 Database Security

### SQL Injection Prevention

All queries use Entity Framework's built-in parameterization:

```csharp
// Safe: EF Core automatically parameterizes
var projects = await _context.Projects
    .Where(p => p.Category == category)
    .ToListAsync();

// Safe: Raw SQL with parameters
var projects = await _context.Projects
    .FromSqlRaw("SELECT * FROM Projects WHERE Category = {0}", category)
    .ToListAsync();
```

### Data Validation

```csharp
// Model validation attributes
public class ContactMessage
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(2000, MinimumLength = 10)]
    public string Message { get; set; } = string.Empty;
}
```

## 📊 Database Monitoring

### Query Logging

```csharp
// Enable query logging in development
protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
{
    if (Environment.IsDevelopment())
    {
        optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information);
    }
}
```

### Performance Monitoring

```csharp
// Monitor slow queries
public class QueryPerformanceMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        var stopwatch = Stopwatch.StartNew();
        await next(context);
        stopwatch.Stop();
        
        if (stopwatch.ElapsedMilliseconds > 1000)
        {
            _logger.LogWarning("Slow request: {Method} {Path} took {ElapsedMilliseconds}ms", 
                context.Request.Method, context.Request.Path, stopwatch.ElapsedMilliseconds);
        }
    }
}
```

## 🚀 Production Deployment

### Railway PostgreSQL

```bash
# Railway automatically provides PostgreSQL
# Connection string available as DATABASE_URL environment variable
export DATABASE_URL="postgresql://username:password@hostname:port/database"
```

### Migration Strategy

```dockerfile
# Dockerfile includes automatic migrations
ENTRYPOINT ["sh", "-c", "dotnet ef database update --no-build && dotnet ArifTanPortfolio.dll"]
```

### Database Backup

```bash
# PostgreSQL backup
pg_dump $DATABASE_URL > backup.sql

# SQLite backup
sqlite3 app.db ".backup backup.db"
```

## 🔧 Database Maintenance

### Regular Tasks

1. **Monitor Performance**: Review slow query logs
2. **Update Statistics**: Ensure query optimizer has current stats
3. **Index Maintenance**: Monitor index usage and fragmentation
4. **Data Cleanup**: Archive old contact messages if needed

### Development Database Reset

```bash
# Reset development database
dotnet ef database drop --force
dotnet ef database update
```

### Production Database Updates

```bash
# Generate SQL script for production
dotnet ef migrations script --from LastMigration --to CurrentMigration

# Apply in production with downtime
dotnet ef database update
```

## 📚 Common Queries

### Portfolio Queries

```csharp
// Get featured projects by category
var projects = await _context.Projects
    .Where(p => p.IsFeatured && p.Category == category)
    .OrderBy(p => p.SortOrder)
    .ToListAsync();

// Get project categories with counts
var categories = await _context.Projects
    .GroupBy(p => p.Category)
    .Select(g => new { Category = g.Key, Count = g.Count() })
    .ToListAsync();
```

### Blog Queries

```csharp
// Get published blog posts with pagination
var posts = await _context.BlogPosts
    .Where(b => b.IsPublished)
    .OrderByDescending(b => b.PublishedDate)
    .Skip(page * pageSize)
    .Take(pageSize)
    .ToListAsync();

// Search blog posts
var searchResults = await _context.BlogPosts
    .Where(b => b.IsPublished && 
        (b.Title.Contains(searchTerm) || b.Content.Contains(searchTerm)))
    .ToListAsync();
```

### Skills Queries

```csharp
// Get visible skills grouped by category
var skillGroups = await _context.Skills
    .Where(s => s.IsVisible)
    .GroupBy(s => s.Category)
    .OrderBy(g => g.Key)
    .ToListAsync();

// Get homepage skills
var homepageSkills = await _context.Skills
    .Where(s => s.IsShowOnHomePage)
    .OrderBy(s => s.SortOrder)
    .ToListAsync();
```

## 🎯 Future Enhancements

### Potential Schema Changes

1. **User Authentication**: Add user roles and permissions
2. **Comments System**: Add blog comments table
3. **Analytics**: Add page view tracking
4. **Tags**: Separate tags table for better normalization

### Performance Improvements

1. **Caching**: Implement Redis for frequently accessed data
2. **Read Replicas**: For high-traffic scenarios
3. **Database Sharding**: For massive scale (unlikely needed)

---

**Database is the foundation of your portfolio - keep it optimized! 🗄️**