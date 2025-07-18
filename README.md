# Arif Tan Portfolio Website

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-8.0-blue.svg)](https://docs.microsoft.com/en-us/aspnet/core/)
[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](https://github.com/Ariftan77/personal-site)
[![Security](https://img.shields.io/badge/security-enhanced-green.svg)](./docs/SECURITY.md)

> **Professional portfolio website showcasing software engineering expertise and career journey**

🌐 **Live Demo**: [https://ariftan.com](https://ariftan.com)

## 🚀 Features

### **Core Functionality**
- **📝 Dynamic Portfolio**: Project showcase with category filtering
- **✍️ Blog System**: Markdown-powered blog with SEO optimization
- **💼 Skills Management**: Categorized technical skills with proficiency levels
- **📬 Contact System**: Secure contact form with email integration
- **🔍 Search & Filter**: Smart filtering for projects and blog posts

### **Technical Features**
- **🛡️ Security**: Rate limiting, CSRF protection, security headers
- **⚡ Performance**: Optimized Entity Framework queries with caching
- **📱 Responsive**: Mobile-first design with Bootstrap 5
- **🔐 Configuration**: User secrets (dev) and environment variables (prod)

## 🏗️ Architecture

### **Technology Stack**
- **Backend**: ASP.NET Core 8.0 with Razor Pages
- **Database**: SQLite (dev) / PostgreSQL (prod) with Entity Framework Core
- **Frontend**: Bootstrap 5, JavaScript, HTML5/CSS3
- **Security**: Custom middleware, antiforgery tokens
- **Deployment**: Railway with Docker containerization

### **Project Structure**
```
ArifTanPortfolio/
├── Data/                 # Entity Framework context and migrations
├── Models/              # Domain models and ViewModels
├── Services/            # Business logic and email services
├── Pages/               # Razor Pages (MVC-style)
├── Middleware/          # Custom security middleware
├── wwwroot/             # Static files (CSS, JS, images)
└── docs/                # Documentation
```

### **Request Flow**
```
User Request → Security Middleware → Razor Page → Service Layer → Database
```

## 🚦 Quick Start

### **Prerequisites**
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Git

### **Local Development**
```bash
# 1. Clone the repository
git clone https://github.com/Ariftan77/personal-site.git
cd personal-site

# 2. Set up user secrets for email (development)
dotnet user-secrets init
dotnet user-secrets set "EmailSettings:Password" "your-email-password"

# 3. Install dependencies and run
dotnet restore
dotnet run

# 4. Open browser
https://localhost:7155
```

### **Database Setup**
```bash
# Database is automatically created on first run
# To reset database:
dotnet ef database drop
dotnet ef database update
```

## 🔧 Configuration

### **Development**
- **Secrets**: Stored in user secrets (`dotnet user-secrets`)
- **Database**: SQLite file (`app.db`)
- **Email**: Gmail SMTP (configured in user secrets)

### **Production (Railway)**
- **Secrets**: Environment variables (secure dashboard)
- **Database**: PostgreSQL (managed by Railway)
- **Email**: Gmail SMTP via app password
- **Domain**: ariftan.com with automatic HTTPS

```bash
# Railway environment variables
ASPNETCORE_ENVIRONMENT=Production
EmailSettings__Password=gmail-app-password
DATABASE_URL=postgresql://... (auto-provided)
```

## 🛡️ Security Features

- **Rate Limiting**: 5 requests/minute for contact forms, 60 for browsing
- **CSRF Protection**: Antiforgery tokens on all forms
- **Security Headers**: CSP, HSTS, X-Frame-Options, etc.
- **Input Validation**: Server-side validation with spam detection
- **Secrets Management**: No credentials in source code

[→ View detailed security documentation](./docs/SECURITY.md)

## 📊 Database Schema

### **Main Entities**
- **Projects**: Portfolio projects with categories and technologies
- **BlogPosts**: Blog articles with SEO metadata
- **Skills**: Technical skills with proficiency levels
- **ContactMessages**: Contact form submissions

[→ View database documentation](./docs/DATABASE.md)

## 🚀 Deployment

### **Railway Deployment**
The application is deployed on Railway using:
- **Docker**: Containerized deployment
- **PostgreSQL**: Managed database (optional)
- **Custom Domain**: ariftan.com with automatic HTTPS
- **Environment Variables**: Secure configuration management

[→ View deployment guide](./docs/DEPLOYMENT.md)

## 🛠️ Development

### **Adding New Features**
1. Create feature branch: `git checkout -b feature/new-feature`
2. Implement changes following existing patterns
3. Add/update tests
4. Update documentation
5. Submit pull request

### **Database Changes**
```bash
# Add new migration
dotnet ef migrations add MigrationName

# Update database
dotnet ef database update
```

[→ View development guide](./docs/DEVELOPMENT.md)

## 📈 Performance

- **Build Time**: ~2 seconds
- **Load Time**: <2 seconds (optimized assets)
- **Database Queries**: Optimized with strategic indexing
- **Security**: Rate limiting prevents abuse

## 🧪 Testing

```bash
# Run all tests
dotnet test

# Run with coverage
dotnet test --collect:"XPlat Code Coverage"
```

## 📝 Contributing

1. Fork the repository
2. Create feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open Pull Request

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## 👤 Author

**Arif Tan**
- **Portfolio**: [https://ariftan.com](https://ariftan.com)
- **LinkedIn**: [https://linkedin.com/in/ariftan2212](https://linkedin.com/in/ariftan2212)
- **GitHub**: [https://github.com/Ariftan77](https://github.com/Ariftan77)
- **Email**: [ariftan7788@gmail.com](mailto:ariftan7788@gmail.com)

## 🌟 Acknowledgments

- **ASP.NET Core Team** for the excellent framework
- **Bootstrap Team** for responsive design components
- **Entity Framework Team** for ORM capabilities
- **Singapore Tech Community** for inspiration

---

**Built with ❤️ for the Singapore tech community**

> This portfolio demonstrates enterprise-grade development practices suitable for Singapore's technology industry, featuring security best practices, clean architecture, and professional documentation.

## 🌏 About Me

- **Name**: Arif Tan  
- **Role**: Software Engineer (6+ years experience)  
- **Location**: Batam, Indonesia 🇮🇩  
- **Tech Stack**: C#, ASP.NET Core, Node.js, Python, JavaScript  
- **Career Goal**: Join a global tech company (e.g., Google, Microsoft, Meta)  
- **Vision**: Build a technology company to empower Batam’s tech ecosystem  

## 🛠 Tech Stack

- **Backend**: ASP.NET Core 8.0 (Razor Pages)
- **Frontend**: Bootstrap 5.3, minimal JavaScript
- **Database**: SQLite (dev) / PostgreSQL (prod) via Entity Framework Core
- **Hosting**: Railway with Docker containerization
- **Domain**: ariftan.com with automatic HTTPS
- **Version Control**: Git + GitHub

## 📁 Website Structure

- `/Pages`: Razor Pages (Home, About, Portfolio, Blog, Contact)
- `/Models`: BlogPost, Project, Skill, ContactMessage
- `/Services`: Business logic and helpers
- `/Data`: EF Core DbContext and migrations
- `/wwwroot`: Static assets (CSS, JS, images)

## 🚀 Key Features

- 📱 Responsive, mobile-first design
- ⚡ Fast loading with minimal JS
- 📝 Dynamic blog system (Markdown support planned)
- 🧑‍💻 Project portfolio with images and GitHub/demo links
- 📬 Contact form with email integration
- 🔍 SEO-optimized with structured data
- 📊 Google Analytics integration (optional)

## 🧱 Goals

- ✅ Attract tech recruiters and employers
- ✅ Establish professional branding
- ✅ Connect with the Batam tech community
- ✅ Share insights via technical blog posts

## 📦 Getting Started

```bash
git clone https://github.com/ariftan/personal-site.git
cd personal-site
dotnet run
