# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This is **ArifTanPortfolio** - a personal portfolio website built with ASP.NET Core 8.0 and Razor Pages. The site showcases software engineering expertise, project portfolio, blog functionality, and professional experience for Arif Tan, a Principal Software Engineer based in Batam, Indonesia.

## Development Commands

### Build and Run
```bash
# Build the project
dotnet build

# Run the application (development)
dotnet run

# Run with specific profile
dotnet run --launch-profile https
```

### Database Operations
```bash
# Add new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update

# Drop database (development only)
dotnet ef database drop
```

### Testing
```bash
# Run all tests (when test project is added)
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## Architecture and Code Structure

### Project Structure
- **Models/**: Entity models (BlogPost, Project, Skill, ContactMessage) with data annotations
- **Data/**: ApplicationDbContext with EF Core configuration and seed data
- **Services/**: Business logic layer with interfaces (IPortfolioService, IEmailService)
- **Pages/**: Razor Pages with code-behind files (.cshtml.cs)
- **wwwroot/**: Static files (CSS, JS, images)
- **Migrations/**: Entity Framework Core migrations

### Key Architecture Patterns
1. **Repository Pattern**: Implemented through services layer
2. **Dependency Injection**: Services registered in Program.cs
3. **Razor Pages Architecture**: Page-focused development model
4. **Entity Framework Core**: Code-first approach with migrations

### Domain Models
- **BlogPost**: Full blog functionality with SEO metadata, tags, categories
- **Project**: Portfolio projects with detailed information, technologies, challenges
- **Skill**: Technical skills with categories and proficiency levels
- **ContactMessage**: Contact form submissions with validation

### Service Layer
- **IPortfolioService**: Core business logic for projects, blog posts, skills
- **IEmailService**: Email functionality for contact forms
- **MarkdownPipeline**: Configured for blog content processing

### Database Design
- **SQLite**: Primary database for development and production
- **Entity Framework Core**: ORM with fluent API configuration
- **Indexes**: Strategic indexing on frequently queried fields
- **Seed Data**: Comprehensive seed data for skills and projects

## Configuration

### Connection Strings
- Default connection uses SQLite with `app.db` file
- Entity Framework configured in Program.cs

### Email Settings
- SMTP configuration in appsettings.json
- MailKit integration for contact forms

### Site Settings
- Professional information and social links
- Configured in appsettings.json

## Development Practices

### Code Style
- Follow C# coding conventions
- Use async/await patterns consistently
- Implement proper error handling and logging
- Use dependency injection for services

### Entity Framework
- Use migrations for schema changes
- Implement proper entity relationships
- Use fluent API for complex configurations
- Include seed data for development

### Security
- Antiforgery tokens configured
- Input validation on all models
- Secure email configuration
- HTTPS redirection enabled

## Key Features

### Blog System
- Full blog functionality with Markdown support
- SEO optimization with meta tags
- Category and tag organization
- View count tracking
- Related posts functionality

### Portfolio Management
- Featured projects showcase
- Detailed project information
- Technology stack display
- Live URLs and GitHub links

### Contact System
- Contact form with validation
- Email integration
- Message storage in database

### Skills Display
- Categorized skill listing
- Proficiency levels
- Icon integration
- Sortable display

## Environment Setup

### Prerequisites
- .NET 8.0 SDK
- Entity Framework Core CLI tools
- SQLite (included with .NET)

### Configuration Files
- `appsettings.json`: Production settings
- `appsettings.Development.json`: Development overrides
- `launchSettings.json`: Launch profiles

### Local Development
1. Ensure database is created (happens automatically on first run)
2. Seed data is populated through migrations
3. Application runs on https://localhost:7155 by default

## Deployment

### Production Considerations
- SQLite database included in deployment
- Email credentials should be configured
- HTTPS enforced in production
- Static files served from wwwroot

### AWS Deployment
- EC2 instance deployment supported
- S3 integration for file storage
- Configured for AWS hosting

## Important Notes

- The site uses Razor Pages, not MVC pattern
- Blog content supports Markdown with advanced extensions
- All dates are stored in UTC
- Email functionality requires proper SMTP configuration
- Database is automatically created and seeded on first run