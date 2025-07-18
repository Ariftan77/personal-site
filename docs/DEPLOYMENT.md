# Deployment Guide

This guide covers deploying the Arif Tan Portfolio website to Railway, a modern cloud platform perfect for containerized applications.

## 🚀 Railway Deployment

Railway is an excellent choice for deploying .NET applications with the following benefits:
- **Automatic HTTPS** with custom domains
- **Zero-config deployments** from GitHub
- **Built-in PostgreSQL** database support
- **Environment variable** management
- **Automatic deployments** on git push
- **Affordable pricing** for personal projects

## 📦 Prerequisites

- **GitHub Repository**: Code must be in a GitHub repository
- **Railway Account**: Sign up at [railway.app](https://railway.app)
- **Docker**: The application uses Docker for containerization

## 🔧 Railway Setup

### 1. Create Railway Project

1. **Sign up/Login** to Railway
2. **Create New Project**
3. **Deploy from GitHub repo**
4. **Select your repository**: `personal-site`
5. **Railway automatically detects** the Dockerfile

### 2. Environment Variables

Set these environment variables in Railway dashboard:

```bash
# Required Environment Variables
ASPNETCORE_ENVIRONMENT=Production
EmailSettings__Password=your-gmail-app-password
EmailSettings__Username=your-email@gmail.com
EmailSettings__FromEmail=your-email@gmail.com
EmailSettings__FromName=Arif Tan Portfolio
EmailSettings__SmtpServer=smtp.gmail.com
EmailSettings__SmtpPort=587

# Optional (Railway sets these automatically)
PORT=8080
ASPNETCORE_URLS=http://+:8080
```

### 3. Database Configuration

**Option A: SQLite (Simple)**
```bash
# Uses SQLite file in container (data persists with Railway volumes)
ConnectionStrings__DefaultConnection=Data Source=/app/data/app.db
```

**Option B: PostgreSQL (Recommended for production)**
```bash
# Railway provides PostgreSQL addon
# Connection string automatically set by Railway
DATABASE_URL=postgresql://username:password@hostname:port/database
```

### 4. Custom Domain Setup

1. **Go to Settings** in Railway dashboard
2. **Add Custom Domain**: `ariftan.com`
3. **Configure DNS**:
   - Add CNAME record: `www.ariftan.com` → `your-app.railway.app`
   - Add A record: `ariftan.com` → Railway IP (provided in dashboard)
4. **SSL Certificate**: Automatically provided by Railway

## 🐳 Docker Configuration

### Dockerfile Explanation

```dockerfile
# Multi-stage build for optimization
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy project files first (for layer caching)
COPY ArifTanPortfolio/*.csproj ./ArifTanPortfolio/
COPY ArifTanPortfolio.Tests/*.csproj ./ArifTanPortfolio.Tests/

# Restore dependencies
RUN dotnet restore ArifTanPortfolio/ArifTanPortfolio.csproj

# Copy source and build
COPY . .
WORKDIR /app/ArifTanPortfolio
RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Install EF Core tools for migrations
RUN dotnet tool install --global dotnet-ef
ENV PATH="${PATH}:/root/.dotnet/tools"

# Copy published app
COPY --from=build /app/publish .

# Create data directory for SQLite
RUN mkdir -p /app/data

# Expose port for Railway
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

# Run migrations and start app
ENTRYPOINT ["sh", "-c", "dotnet ef database update --no-build && dotnet ArifTanPortfolio.dll"]
```

### Building Locally

```bash
# Build Docker image
docker build -t ariftan-portfolio .

# Run container locally
docker run -p 8080:8080 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e EmailSettings__Password=your-password \
  ariftan-portfolio

# Test the application
curl http://localhost:8080
```

## 🔐 Production Security

### Environment Variables Security

**Railway Dashboard**:
- Variables are encrypted at rest
- Only visible to project members
- Automatically injected into containers

**Gmail App Password Setup**:
```bash
# Enable 2FA on Gmail account
# Generate App Password in Gmail Settings
# Use App Password (not regular password)
EmailSettings__Password=abcd efgh ijkl mnop
```

### Database Security

**SQLite in Production**:
- Data persists with Railway volumes
- Automatic backups not included
- Suitable for personal projects

**PostgreSQL in Production**:
- Managed by Railway
- Automatic backups included
- Better for production workloads

## 📊 Monitoring & Logging

### Railway Dashboard

- **Deployment Logs**: View build and runtime logs
- **Metrics**: CPU, Memory, Network usage
- **Health Checks**: Container health monitoring
- **Crash Recovery**: Automatic restart on failures

### Application Logging

```csharp
// Structured logging in ASP.NET Core
public void ConfigureServices(IServiceCollection services)
{
    services.AddLogging(builder =>
    {
        builder.AddConsole();
        builder.AddFilter("Microsoft.AspNetCore", LogLevel.Warning);
    });
}
```

### Log Levels

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning",
      "Microsoft.EntityFrameworkCore": "Warning"
    }
  }
}
```

## 🚦 Deployment Pipeline

### Automatic Deployment

1. **Push to GitHub**: `git push origin main`
2. **Railway detects changes**: Automatic trigger
3. **Docker build**: Multi-stage build process
4. **Container deployment**: Zero-downtime deployment
5. **Health checks**: Verify deployment success

### Manual Deployment

```bash
# Deploy specific branch
railway deploy --branch feature/new-feature

# Deploy with custom environment
railway deploy --env staging
```

### Rollback Strategy

```bash
# List deployments
railway deployments

# Rollback to previous deployment
railway rollback [deployment-id]
```

## 🔄 Database Migrations

### Automatic Migrations

The Dockerfile includes automatic migration execution:

```bash
# Migrations run automatically on container startup
ENTRYPOINT ["sh", "-c", "dotnet ef database update --no-build && dotnet ArifTanPortfolio.dll"]
```

### Manual Migration Management

```bash
# Connect to Railway container
railway shell

# Run migrations manually
dotnet ef database update

# Check migration status
dotnet ef migrations list
```

### Migration Best Practices

1. **Test migrations locally** before deploying
2. **Backup database** before major schema changes
3. **Use descriptive migration names**
4. **Review generated SQL** for complex changes

## 📈 Performance Optimization

### Docker Optimization

```dockerfile
# Use specific .NET version
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS runtime

# Multi-stage build reduces image size
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

# Layer caching optimization
COPY *.csproj ./
RUN dotnet restore
COPY . .
RUN dotnet publish
```

### Railway Optimization

- **Resource Limits**: Set appropriate CPU/Memory limits
- **Health Checks**: Configure health check endpoints
- **Caching**: Implement response caching
- **CDN**: Use Railway's built-in CDN

## 🔧 Environment-Specific Configuration

### Development
```bash
ASPNETCORE_ENVIRONMENT=Development
# Uses user secrets for sensitive data
```

### Staging
```bash
ASPNETCORE_ENVIRONMENT=Staging
# Uses environment variables
# Separate Railway project for staging
```

### Production
```bash
ASPNETCORE_ENVIRONMENT=Production
# Uses environment variables
# Custom domain: ariftan.com
```

## 🚨 Troubleshooting

### Common Issues

**Build Failures**:
```bash
# Check Dockerfile syntax
docker build -t test .

# Review build logs in Railway dashboard
railway logs --build
```

**Runtime Issues**:
```bash
# Check application logs
railway logs

# Connect to container
railway shell

# Check environment variables
railway variables
```

**Database Issues**:
```bash
# Check migration status
railway shell
dotnet ef migrations list

# Reset database (development only)
railway shell
dotnet ef database drop
dotnet ef database update
```

### Health Check Endpoint

Add health check endpoint for monitoring:

```csharp
// Program.cs
builder.Services.AddHealthChecks();

app.MapHealthChecks("/health");
```

## 📊 Cost Optimization

### Railway Pricing

- **Hobby Plan**: $5/month (suitable for personal projects)
- **Pro Plan**: $20/month (for production workloads)
- **Usage-based**: Pay for actual resource consumption

### Cost Optimization Tips

1. **Use appropriate resource limits**
2. **Implement efficient caching**
3. **Optimize Docker image size**
4. **Monitor resource usage**

## 🌟 Production Checklist

### Pre-Deployment
- [ ] Environment variables configured
- [ ] Database migrations tested
- [ ] SSL certificate configured
- [ ] Custom domain set up
- [ ] Health checks implemented

### Post-Deployment
- [ ] Application accessible via custom domain
- [ ] Contact form working
- [ ] Database populated with seed data
- [ ] Security headers present
- [ ] Performance metrics monitoring

### Maintenance
- [ ] Regular dependency updates
- [ ] Security patch management
- [ ] Database backup strategy
- [ ] Performance monitoring
- [ ] Log analysis

## 📚 Additional Resources

- [Railway Documentation](https://docs.railway.app/)
- [.NET Docker Guide](https://docs.microsoft.com/en-us/dotnet/core/docker/)
- [ASP.NET Core Deployment](https://docs.microsoft.com/en-us/aspnet/core/host-and-deploy/)
- [Entity Framework Migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/)

## 🎯 Next Steps

1. **Set up Railway project**
2. **Configure environment variables**
3. **Deploy from GitHub**
4. **Set up custom domain**
5. **Configure monitoring**
6. **Test all functionality**

---

**Your portfolio will be live at `ariftan.com` with enterprise-grade hosting! 🚀**