# Development Guide

This guide covers local development setup, coding standards, and development workflows for the Arif Tan Portfolio website.

## 🚦 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Git](https://git-scm.com/)
- [Visual Studio Code](https://code.visualstudio.com/) or [Visual Studio](https://visualstudio.microsoft.com/)
- [Entity Framework Core CLI](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)

### Initial Setup

1. **Clone the repository**
   ```bash
   git clone https://github.com/Ariftan77/personal-site.git
   cd personal-site
   ```

2. **Install Entity Framework CLI tools**
   ```bash
   dotnet tool install --global dotnet-ef
   ```

3. **Set up user secrets for development**
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "EmailSettings:Password" "your-gmail-app-password"
   ```

4. **Restore dependencies**
   ```bash
   dotnet restore
   ```

5. **Run the application**
   ```bash
   dotnet run
   ```

6. **Access the application**
   - HTTPS: `https://localhost:7155`
   - HTTP: `http://localhost:5000`

## 🏗️ Project Structure

```
ArifTanPortfolio/
├── Data/
│   ├── ApplicationDbContext.cs      # EF Core context
│   └── Migrations/                  # Database migrations
├── Models/
│   ├── BlogPost.cs                  # Domain models
│   ├── Project.cs
│   ├── Skill.cs
│   ├── ContactMessage.cs
│   └── ViewModels/                  # View models for pages
├── Services/
│   ├── IEmailService.cs             # Service interfaces
│   ├── EmailService.cs              # Email implementation
│   ├── IPortfolioService.cs
│   └── PortfolioService.cs          # Business logic
├── Pages/
│   ├── Index.cshtml                 # Homepage
│   ├── Portfolio.cshtml             # Portfolio page
│   ├── Blog.cshtml                  # Blog listing
│   ├── Contact.cshtml               # Contact form
│   └── Shared/                      # Shared layouts
├── Middleware/
│   ├── RateLimitingMiddleware.cs    # Rate limiting
│   └── SecurityHeadersMiddleware.cs # Security headers
├── wwwroot/
│   ├── css/                         # Custom styles
│   ├── js/                          # JavaScript files
│   └── images/                      # Static images
└── docs/                            # Documentation
```

## 🔧 Configuration

### Development Settings

The application uses multiple configuration sources:

1. **appsettings.json** - Base configuration
2. **appsettings.Development.json** - Development overrides
3. **User Secrets** - Sensitive data (email passwords)
4. **Environment Variables** - Production settings

### User Secrets Setup

```bash
# Initialize user secrets
dotnet user-secrets init

# Set email password
dotnet user-secrets set "EmailSettings:Password" "your-app-password"

# Set other sensitive settings
dotnet user-secrets set "EmailSettings:Username" "your-email@gmail.com"
```

### Environment Variables (Production)

```bash
export EmailSettings__Password="production-password"
export EmailSettings__Username="production-email@domain.com"
export ASPNETCORE_ENVIRONMENT="Production"
```

## 🗄️ Database Development

### Entity Framework Core

The application uses **SQLite** for development and can use **PostgreSQL** for production.

#### Common Commands

```bash
# Add new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Remove last migration
dotnet ef migrations remove

# Drop database (development only)
dotnet ef database drop

# Generate SQL script
dotnet ef migrations script
```

#### Database Schema

**Main Entities:**
- `Projects` - Portfolio projects with categories
- `BlogPosts` - Blog articles with SEO metadata
- `Skills` - Technical skills with proficiency levels
- `ContactMessages` - Contact form submissions

### Seed Data

Initial data is seeded automatically:
- Skills with categories and proficiency levels
- Sample projects with realistic data
- Blog posts with content

## 🎨 Frontend Development

### Technology Stack
- **Bootstrap 5** - UI framework
- **Vanilla JavaScript** - Client-side logic
- **Font Awesome** - Icons
- **Google Fonts** - Typography

### Key JavaScript Files

```
wwwroot/js/
├── site.js              # Global site functionality
├── contact-form.js      # Contact form handling
└── blog.js              # Blog search/filtering
```

### CSS Organization

```
wwwroot/css/
├── site.css             # Main styles
└── custom.css           # Custom overrides
```

## 🧪 Testing

### Running Tests

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run specific test class
dotnet test --filter "SecurityHeadersMiddlewareTests"
```

### Test Structure

```
ArifTanPortfolio.Tests/
├── Security/
│   ├── SecurityHeadersMiddlewareTests.cs
│   ├── RateLimitingMiddlewareTests.cs
│   └── ContactValidationTests.cs
└── Services/
    ├── EmailServiceTests.cs
    └── PortfolioServiceTests.cs
```

## 📝 Coding Standards

### C# Conventions

- **Naming**: PascalCase for classes/methods, camelCase for variables
- **Indentation**: 4 spaces
- **Line Length**: 120 characters max
- **Comments**: Business logic only, self-documenting code preferred

### Example Code Style

```csharp
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
}
```

### JavaScript Conventions

- **Naming**: camelCase
- **Indentation**: 2 spaces
- **Async/Await**: Preferred over promises
- **ES6**: Modern JavaScript features

### Example JavaScript Style

```javascript
async function handleContactFormSubmit(e) {
    e.preventDefault();
    
    const form = e.target;
    const formData = new FormData(form);
    
    try {
        const response = await fetch(form.action, {
            method: 'POST',
            body: formData
        });
        
        const result = await response.json();
        
        if (result.success) {
            showAlert('success', result.message);
            form.reset();
        } else {
            showAlert('danger', result.message);
        }
    } catch (error) {
        showAlert('danger', 'An error occurred. Please try again.');
    }
}
```

## 🔍 Debugging

### Development Tools

1. **Visual Studio Code Extensions**
   - C# for Visual Studio Code
   - ASP.NET Core Snippets
   - Entity Framework Snippets

2. **Browser Developer Tools**
   - Network tab for API calls
   - Console for JavaScript errors
   - Application tab for localStorage

3. **Logging**
   - Console logging in development
   - Structured logging with categories

### Common Issues

**Database Issues:**
```bash
# Reset database
dotnet ef database drop
dotnet ef database update
```

**User Secrets Issues:**
```bash
# List current secrets
dotnet user-secrets list

# Clear all secrets
dotnet user-secrets clear
```

## 📦 Build and Deployment

### Development Build

```bash
# Build project
dotnet build

# Run with hot reload
dotnet watch run

# Publish for deployment
dotnet publish -c Release -o ./publish
```

### Production Considerations

1. **Environment Variables**: Set production secrets
2. **Database**: Use PostgreSQL for production
3. **SSL**: Ensure HTTPS is properly configured
4. **Performance**: Enable response compression

## 🚀 Feature Development Workflow

### Adding New Features

1. **Create Feature Branch**
   ```bash
   git checkout -b feature/new-feature-name
   ```

2. **Implement Feature**
   - Add models if needed
   - Create/update services
   - Add/update pages
   - Add tests

3. **Database Changes**
   ```bash
   dotnet ef migrations add AddNewFeature
   dotnet ef database update
   ```

4. **Test Changes**
   ```bash
   dotnet test
   dotnet run
   ```

5. **Commit and Push**
   ```bash
   git add .
   git commit -m "Add new feature: description"
   git push origin feature/new-feature-name
   ```

### Code Review Checklist

- [ ] Code follows established conventions
- [ ] Tests are added/updated
- [ ] Documentation is updated
- [ ] No sensitive data in code
- [ ] Performance considerations addressed
- [ ] Security best practices followed

## 🛠️ Maintenance

### Regular Tasks

1. **Update Dependencies**
   ```bash
   dotnet list package --outdated
   dotnet update
   ```

2. **Clean Build Artifacts**
   ```bash
   dotnet clean
   ```

3. **Database Maintenance**
   ```bash
   # Check for pending migrations
   dotnet ef migrations list
   ```

### Performance Monitoring

- Monitor application logs
- Check database query performance
- Review client-side performance metrics
- Monitor security middleware effectiveness

## 📚 Additional Resources

- [ASP.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
- [Entity Framework Core Documentation](https://docs.microsoft.com/en-us/ef/core/)
- [Bootstrap Documentation](https://getbootstrap.com/docs/)
- [JavaScript Modern Features](https://developer.mozilla.org/en-US/docs/Web/JavaScript)

---

**Happy coding! 🚀**