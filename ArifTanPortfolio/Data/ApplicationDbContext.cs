using Microsoft.EntityFrameworkCore;
using ArifTanPortfolio.Models;

namespace ArifTanPortfolio.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<BlogPost> BlogPosts { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure BlogPost
            modelBuilder.Entity<BlogPost>(entity =>
            {
                entity.HasIndex(e => e.Slug).IsUnique();
                entity.HasIndex(e => e.PublishedDate);
            });

            // Configure Project
            modelBuilder.Entity<Project>(entity =>
            {
                entity.HasIndex(e => e.SortOrder);
                entity.HasIndex(e => e.Category);
            });

            // Configure ContactMessage
            modelBuilder.Entity<ContactMessage>(entity =>
            {
                entity.HasIndex(e => e.DateSent);
            });

            // Configure Skill
            modelBuilder.Entity<Skill>(entity =>
            {
                entity.HasIndex(e => e.Category);
                entity.HasIndex(e => e.SortOrder);
            });

            // Seed initial data
            SeedData(modelBuilder);
            SeedBlogPosts(modelBuilder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Skill>().HasData(
            // Programming Languages
            new Skill { Id = 1, Name = "C#", Proficiency = 9, Category = "Programming Languages", SortOrder = 1, Icon = "devicon-csharp-plain", IsVisible = true, IsShowOnHomePage = true },
            new Skill { Id = 2, Name = "JavaScript", Proficiency = 8, Category = "Programming Languages", SortOrder = 2, Icon = "devicon-javascript-plain", IsVisible = true, IsShowOnHomePage = true },
            new Skill { Id = 3, Name = "Python", Proficiency = 8, Category = "Programming Languages", SortOrder = 3, Icon = "devicon-python-plain", IsVisible = true, IsShowOnHomePage = false },
            new Skill { Id = 4, Name = "Dart", Proficiency = 6, Category = "Programming Languages", SortOrder = 4, Icon = "devicon-dart-plain", IsVisible = true, IsShowOnHomePage = false },

            // Frameworks (includes web frameworks and frontend frameworks)
            new Skill { Id = 5, Name = "ASP.NET Core", Proficiency = 9, Category = "Frameworks", SortOrder = 1, Icon = "devicon-dot-net-plain", IsVisible = true, IsShowOnHomePage = true },
            new Skill { Id = 6, Name = "Node.js", Proficiency = 8, Category = "Frameworks", SortOrder = 2, Icon = "devicon-nodejs-plain", IsVisible = true, IsShowOnHomePage = true },
            new Skill { Id = 7, Name = "Flutter", Proficiency = 6, Category = "Frameworks", SortOrder = 3, Icon = "devicon-flutter-plain", IsVisible = false, IsShowOnHomePage = false },
            new Skill { Id = 8, Name = "Bootstrap", Proficiency = 8, Category = "Frameworks", SortOrder = 4, Icon = "devicon-bootstrap-plain", IsVisible = false, IsShowOnHomePage = false },
            new Skill { Id = 9, Name = "ASP.NET MVC", Proficiency = 9, Category = "Frameworks", SortOrder = 5, Icon = "devicon-dot-net-plain", IsVisible = true, IsShowOnHomePage = false },
            new Skill { Id = 10, Name = "Razor Pages", Proficiency = 8, Category = "Frameworks", SortOrder = 6, Icon = "devicon-dot-net-plain", IsVisible = false, IsShowOnHomePage = false },
            new Skill { Id = 11, Name = "React", Proficiency = 6, Category = "Frameworks", SortOrder = 7, Icon = "devicon-react-original", IsVisible = false, IsShowOnHomePage = false, Description = "Maintenance experience" },
            new Skill { Id = 12, Name = "SignalR", Proficiency = 8, Category = "Frameworks", SortOrder = 8, Icon = "devicon-dot-net-plain", IsVisible = false, IsShowOnHomePage = false },
            new Skill { Id = 13, Name = "jQuery", Proficiency = 8, Category = "Frameworks", SortOrder = 9, Icon = "devicon-jquery-plain", IsVisible = false, IsShowOnHomePage = false },
            new Skill { Id = 14, Name = "HTML/CSS", Proficiency = 8, Category = "Frameworks", SortOrder = 10, Icon = "devicon-html5-plain", IsVisible = false, IsShowOnHomePage = false },
            new Skill { Id = 15, Name = "AJAX", Proficiency = 8, Category = "Frameworks", SortOrder = 11, Icon = "devicon-javascript-plain", IsVisible = true, IsShowOnHomePage = false },

            // Cloud & DevOps
            new Skill { Id = 16, Name = "Git", Proficiency = 9, Category = "Cloud & DevOps", SortOrder = 2, Icon = "devicon-git-plain", IsVisible = true, IsShowOnHomePage = true },
            new Skill { Id = 17, Name = "AWS (S3, RDS)", Proficiency = 8, Category = "Cloud & DevOps", SortOrder = 1, Icon = "devicon-amazonwebservices-plain", IsVisible = true, IsShowOnHomePage = true },
            new Skill { Id = 18, Name = "AWS EC2 & Lightsail", Proficiency = 8, Category = "Cloud & DevOps", SortOrder = 5, Icon = "devicon-amazonwebservices-plain", IsVisible = true, IsShowOnHomePage = false },
            new Skill { Id = 19, Name = "Docker", Proficiency = 7, Category = "Cloud & DevOps", SortOrder = 3, Icon = "devicon-docker-plain", IsVisible = true, IsShowOnHomePage = true },
            new Skill { Id = 20, Name = "Power Automate", Proficiency = 8, Category = "Cloud & DevOps", SortOrder = 4, Icon = "fas fa-bolt", IsVisible = false, IsShowOnHomePage = false },
            new Skill { Id = 21, Name = "Azure", Proficiency = 7, Category = "Cloud & DevOps", SortOrder = 8, Icon = "devicon-azure-plain", IsVisible = false, IsShowOnHomePage = false },
            new Skill { Id = 22, Name = "Azure DevOps", Proficiency = 7, Category = "Cloud & DevOps", SortOrder = 9, Icon = "devicon-azure-plain", IsVisible = false, IsShowOnHomePage = false },

            // Databases
            new Skill { Id = 23, Name = "SQL Server", Proficiency = 9, Category = "Databases", SortOrder = 1, Icon = "devicon-microsoftsqlserver-plain", IsVisible = true, IsShowOnHomePage = false },
            new Skill { Id = 24, Name = "PostgreSQL", Proficiency = 9, Category = "Databases", SortOrder = 2, Icon = "devicon-postgresql-plain", IsVisible = true, IsShowOnHomePage = true },
            new Skill { Id = 25, Name = "Entity Framework Core", Proficiency = 9, Category = "Databases", SortOrder = 3, Icon = "devicon-dot-net-plain", IsVisible = true, IsShowOnHomePage = false },
            new Skill { Id = 26, Name = "SQLite", Proficiency = 9, Category = "Databases", SortOrder = 4, Icon = "devicon-sqlite-plain", IsVisible = true, IsShowOnHomePage = false }
            );

            // Seed Featured Projects - Updated with Real Project Details
            modelBuilder.Entity<Project>().HasData(
                new Project
                {
                    Id = 1,
                    Name = "Multi-Warehouse Management System",
                    Description = "Comprehensive WMS with real-time location tracking, multi-client inventory management, and cross-warehouse operations",
                    LongDescription = "Led development of an enterprise-scale warehouse management system serving multiple warehouse locations with real-time inventory tracking and location-based operations. The system supports multi-client operations with separate inventory management per client while maintaining unified warehouse operations. Features real-time dashboard with SignalR for location grid updates, comprehensive reporting, and scalable architecture designed for warehouse network expansion.",
                    Technologies = "ASP.NET Core 8.0, PostgreSQL, SignalR, Entity Framework Core, Clean Architecture, Domain-Driven Design, Tailwind CSS, AWS (S3, EC2, RDS), REST APIs, JavaScript",
                    Category = "Enterprise Software",
                    IsFeatured = true,
                    SortOrder = 1,
                    StartDate = new DateTime(2025, 5, 13),
                    EndDate = null, // Ongoing project
                    Challenges = "Multi-tenant domain modeling with DDD bounded contexts, real-time location synchronization across warehouse networks, implementing Clean Architecture in complex business domain, CQRS pattern implementation for read/write separation, performance optimization for concurrent warehouse operations",
                    Solutions = "Implemented SignalR for real-time location grid dashboard, designed multi-tenant domain architecture with client isolation using DDD bounded contexts, applied Clean Architecture with separated domain/application/infrastructure layers, optimized database queries with strategic indexing, created robust API layer following CQRS patterns, implemented comprehensive logging and monitoring with AWS services",
                    LessonsLearned = "Domain-Driven Design for complex business logic, Clean Architecture implementation in enterprise systems, CQRS patterns for scalable applications, multi-tenant bounded context design, advanced SignalR for real-time systems, AWS cloud architecture integration",
                    ImageGallery = "",
                    FeaturedImage = null,
                    LiveUrl = null,
                    GitHubUrl = null
                },
                new Project
                {
                    Id = 2,
                    Name = "AI-Powered WhatsApp Health Assistant Bot",
                    Description = "Conversational AI system for Indonesian health product business with advanced token optimization and real-time conversation management",
                    LongDescription = "Architected and developed an enterprise-grade conversational AI system using TypeScript/Node.js for WhatsApp Business automation targeting Indonesian health product business. The system features advanced token optimization algorithms achieving 60-70% API cost reduction, real-time conversation state management supporting 1000+ concurrent users with sub-second response times. Specialized for Indonesian market with Bahasa Indonesia natural language processing, local payment integration (COD, e-wallets), and comprehensive analytics dashboard for business insights and customer journey tracking.",
                    Technologies = "TypeScript, Node.js, Express.js, Redis, Claude AI API, WhatsApp Business API, SQLite, Docker",
                    Category = "Machine Learning",
                    IsFeatured = true,
                    SortOrder = 2,
                    StartDate = new DateTime(2025, 6, 20),
                    EndDate = new DateTime(2025, 7, 15),
                    Challenges = "Token optimization for cost efficiency, real-time conversation state management at scale, Indonesian language processing for health domain, WhatsApp Business API integration complexities, concurrent user session handling, local payment method ",
                    Solutions = "Implemented advanced token optimization algorithms with caching and compression achieving 60-70% cost reduction, built Redis-based conversation state management for 1000+ concurrent users, developed specialized Bahasa Indonesia NLP for health consultations, created robust WhatsApp Business API integration with proper error handling.",
                    LessonsLearned = "AI API cost optimization strategies, real-time conversation management architecture, Indonesian market localization requirements, WhatsApp Business API best practices, scalable chat system design patterns, importance of analytics in conversational AI systems",
                    ImageGallery = "",
                    FeaturedImage = null,
                    LiveUrl = null,
                    GitHubUrl = "https://github.com/Ariftan77/whatsapp-health-assistant"
                },               
                new Project
                {
                    Id = 3,
                    Name = "Warehouse Job Management Web Application",
                    Description = "Enterprise warehouse operations management system for international client with inventory and invoice modules",
                    LongDescription = "Technical lead for developing a comprehensive warehouse operations management system for a global client. The system handles warehouse job operations, packaging material inventory management, and invoice processing. Built using layered architecture principles with a team of 3 engineers, delivering significant operational efficiency improvements for a client transitioning from manual processes to digital warehouse management.",
                    Technologies = "ASP.NET Core 7.0, PostgreSQL, Entity Framework Core, Clean Architecture, Domain-Driven Design, Bootstrap, AWS (S3, EC2, RDS), REST APIs, JavaScript",
                    Category = "Enterprise Software",
                    IsFeatured = true,
                    SortOrder = 3,
                    StartDate = new DateTime(2024, 6, 1),
                    EndDate = new DateTime(2024, 7, 31),
                    Challenges = "Domain modeling for warehouse operations using DDD principles, implementing Clean Architecture in enterprise client environment, complex inventory and invoice domain logic, tight MVP delivery timeline with proper architectural foundation",
                    Solutions = "Applied Clean Architecture principles with clear domain/application/infrastructure separation, designed domain models for warehouse operations using DDD tactical patterns, implemented repository and unit of work patterns, created comprehensive API layer with proper dependency inversion, deployed on AWS EC2",
                    LessonsLearned = "Clean Architecture implementation in enterprise projects, Domain-Driven Design tactical patterns, leading development teams using modern architectural approaches, client stakeholder management, importance of architectural decisions in project success",
                    ImageGallery = "",
                    FeaturedImage = null,
                    LiveUrl = null,
                    GitHubUrl = null
                },
                new Project
                {
                    Id = 4,
                    Name = "Multi-Client EDI Processing & Integration Platform",
                    Description = "Automated EDI file processing system with SFTP monitoring, API integrations, and multi-client support for logistics operations",
                    LongDescription = "Developed a comprehensive EDI processing platform serving multiple logistics clients with automated file processing, SFTP monitoring, and API integrations. The system includes SFTP folder watcher programs, EDI file processing engines, XML generation APIs, and automated notification systems. Handled diverse client requirements with custom processing logic for each client while maintaining a unified platform architecture.",
                    Technologies = "Node.js, SFTP protocols, XML processing, Email APIs, Microsoft Teams integration, Task Scheduling, File System monitoring",
                    Category = "Integration Platform",
                    IsFeatured = true,
                    SortOrder = 4,
                    StartDate = new DateTime(2023, 3, 1),
                    EndDate = new DateTime(2023, 9, 30),
                    Challenges = "Multiple client EDI format variations, reliable SFTP monitoring and file processing, error handling and notification systems, automated workflow management, maintaining system reliability for critical logistics operations",
                    Solutions = "Built robust SFTP folder watcher with Node.js, implemented flexible EDI parsing engine supporting multiple formats, created comprehensive error handling with email and Teams notifications, designed modular architecture for easy client onboarding, automated file processing with proper logging and monitoring",
                    LessonsLearned = "Node.js for enterprise file processing, SFTP protocol implementation, building reliable automated systems, multi-client platform architecture, importance of comprehensive error handling in logistics systems",
                    ImageGallery = "",
                    FeaturedImage = null,
                    LiveUrl = null,
                    GitHubUrl = null
                },
                new Project
                {
                    Id = 5,
                    Name = "OCR Document Processing & Logistics Container System",
                    Description = "Python-based OCR system for manifest processing",
                    LongDescription = "Developed an intelligent document processing system using Python OCR to extract data from manifest files. The OCR system automatically processes uploaded documents via task scheduler, extracts relevant data, consolidates information, and saves to database.",
                    Technologies = "Python, OCR libraries, Task Scheduler, SQL Server, File processing",
                    Category = "Machine Learning",
                    IsFeatured = false,
                    SortOrder = 5,
                    StartDate = new DateTime(2023, 10, 1),
                    EndDate = new DateTime(2024, 1, 31),
                    Challenges = "Accurate OCR data extraction from varying document formats, automated file processing reliability, system deployment",
                    Solutions = "Implemented robust Python OCR processing with error handling, designed efficient data consolidation algorithms.",
                    LessonsLearned = "Python OCR implementation for production systems, automated document processing workflows.",
                    ImageGallery = "",
                    FeaturedImage = null,
                    LiveUrl = null,
                    GitHubUrl = null
                },
                new Project
                {
                    Id = 6,
                    Name = "Electronics Manufacturing WMS & Desktop Applications",
                    Description = "Comprehensive warehouse management system with desktop applications for electronics manufacturing operations",
                    LongDescription = "Contributed to developing a complete warehouse management system from scratch for electronics manufacturing operations. The system included desktop applications for warehouse operations, production tracking, finished goods management, and shipment processing. Handled high-volume manufacturing workflows with integration to existing ERP systems and custom label printing solutions.",
                    Technologies = "C# Windows Forms, Entity Framework, SQL Server, Web Services, Crystal Reports, Manufacturing integrations",
                    Category = "Enterprise Software",
                    IsFeatured = false,
                    SortOrder = 6,
                    StartDate = new DateTime(2018, 7, 1),
                    EndDate = new DateTime(2020, 9, 30),
                    Challenges = "Complex manufacturing workflow integration, high-volume transaction processing, ERP system integration, custom reporting requirements, user training and system adoption in manufacturing environment",
                    Solutions = "Built robust desktop applications using C# Windows Forms, implemented efficient database design with Entity Framework, created seamless ERP integrations, developed comprehensive reporting system, provided extensive user training and support",
                    LessonsLearned = "Desktop application development for manufacturing, database design for high-volume operations, ERP system integration patterns, importance of user training in system adoption, manufacturing industry requirements and workflows",
                    ImageGallery = "",
                    FeaturedImage = null,
                    LiveUrl = null,
                    GitHubUrl = null
                },
                new Project
                {
                    Id = 7,
                    Name = "Personal Portfolio Website",
                    Description = "Professional portfolio website showcasing software engineering expertise and career journey",
                    LongDescription = "Designed and developed a comprehensive personal portfolio website to showcase software engineering expertise, project portfolio, and professional journey. The website features responsive design, modern UI/UX, project galleries, blog functionality, and contact forms. Built with focus on performance, SEO optimization, and professional presentation for career advancement opportunities.",
                    Technologies = "ASP.NET Core 8.0, Razor Pages, Bootstrap 5, Entity Framework Core, SQLite, HTML/CSS, JavaScript",
                    Category = "Web Applications",
                    IsFeatured = false,
                    SortOrder = 7,
                    StartDate = new DateTime(2025, 6, 1),
                    EndDate = new DateTime(2025, 6, 20),
                    Challenges = "Professional design and user experience, responsive layout across devices, SEO optimization, content management system design, performance optimization, modern web development practices",
                    Solutions = "Implemented clean and modern design with Bootstrap 5, created responsive layouts for all device sizes, optimized for search engines with proper meta tags and structured data, built custom CMS functionality with Entity Framework, optimized loading performance and user experience",
                    LessonsLearned = "Modern web development practices, responsive design principles, SEO optimization techniques, personal branding through technology, importance of professional online presence in career development",
                    ImageGallery = "",
                    FeaturedImage = null,
                    LiveUrl = "https://ariftan.com",
                    GitHubUrl = "https://github.com/Ariftan77/personal-site"
                }
            );
        }
        // Fixed SeedBlogPosts method with static dates
        private void SeedBlogPosts(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BlogPost>().HasData(
                new BlogPost
                {
                    Id = 1,
                    Title = "Building Enterprise WMS with Clean Architecture: Real-World Implementation",
                    Slug = "building-enterprise-wms-clean-architecture-real-world",
                    Content = @"# Building Enterprise WMS with Clean Architecture: Real-World Implementation

        Leading the development of a multi-warehouse management system has been one of the most challenging and rewarding projects of my career. This post shares the practical lessons learned while implementing Clean Architecture and Domain-Driven Design in a real enterprise environment.

        ## The Challenge

        Our client needed a comprehensive warehouse management system that could:
        - Handle multiple warehouse locations
        - Support multi-client operations with data isolation  
        - Provide real-time inventory tracking
        - Scale for future warehouse network expansion

        ## Clean Architecture in Practice

        ### Domain Layer Design

        The heart of our system lies in properly modeled domain entities:

        ```csharp
        public class WarehouseLocation : Entity
        {
            public LocationId Id { get; private set; }
            public string LocationCode { get; private set; }
            public ClientId ClientId { get; private set; }
            public WarehouseId WarehouseId { get; private set; }
            
            private readonly List<InventoryItem> _inventoryItems = new();
            public IReadOnlyList<InventoryItem> InventoryItems => _inventoryItems.AsReadOnly();
            
            public Result<InventoryItem> AddInventory(Product product, Quantity quantity, UserId userId)
            {
                if (quantity.IsZero())
                    return Result.Failure<InventoryItem>(""Quantity must be greater than zero"");
                    
                var inventoryItem = InventoryItem.Create(product, quantity, Id, userId);
                _inventoryItems.Add(inventoryItem);
                
                AddDomainEvent(new InventoryAddedEvent(Id, product.Id, quantity));
                
                return Result.Success(inventoryItem);
            }
        }
        ```

        ### Application Layer Use Cases

        ```csharp
        public class TransferInventoryUseCase : ITransferInventoryUseCase
        {
            private readonly IWarehouseLocationRepository _locationRepository;
            private readonly IInventoryTransferService _transferService;
            private readonly IUnitOfWork _unitOfWork;
            
            public async Task<r> ExecuteAsync(TransferInventoryCommand command)
            {
                var sourceLocation = await _locationRepository.GetByIdAsync(command.SourceLocationId);
                var targetLocation = await _locationRepository.GetByIdAsync(command.TargetLocationId);
                
                var transferResult = _transferService.TransferInventory(
                    sourceLocation, 
                    targetLocation, 
                    command.ProductId, 
                    command.Quantity,
                    command.UserId);
                    
                if (transferResult.IsFailure)
                    return transferResult;
                    
                await _unitOfWork.SaveChangesAsync();
                return Result.Success();
            }
        }
        ```

        ## Real-Time Updates with SignalR

        One of the key requirements was real-time location grid updates for warehouse operators:

        ```csharp
        [Authorize]
        public class WarehouseHub : Hub
        {
            public async Task JoinWarehouseGroup(string warehouseId)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $""warehouse_{warehouseId}"");
            }
            
            public async Task NotifyLocationUpdate(string warehouseId, LocationUpdateDto update)
            {
                await Clients.Group($""warehouse_{warehouseId}"")
                    .SendAsync(""LocationUpdated"", update);
            }
        }
        ```

        ## Multi-Tenant Architecture

        Ensuring client data isolation was critical:

        ```csharp
        public class WarehouseLocationRepository : IWarehouseLocationRepository
        {
            private readonly ApplicationDbContext _context;
            private readonly ICurrentUserService _currentUser;
            
            public async Task<List<WarehouseLocation>> GetByWarehouseAsync(WarehouseId warehouseId)
            {
                var clientId = _currentUser.GetClientId();
                
                return await _context.WarehouseLocations
                    .Where(l => l.WarehouseId == warehouseId && l.ClientId == clientId)
                    .ToListAsync();
            }
        }
        ```

        ## Performance Optimization

        Strategic database indexing proved crucial for performance:

        ```sql
        CREATE INDEX IX_WarehouseLocation_ClientId_WarehouseId 
        ON WarehouseLocations (ClientId, WarehouseId);

        CREATE INDEX IX_InventoryItem_LocationId_ProductId 
        ON InventoryItems (LocationId, ProductId) 
        INCLUDE (Quantity, LastUpdated);
        ```

        ## Key Lessons Learned

        ### 1. Domain Modeling is Critical
        Spending time upfront to understand the domain and model it correctly pays massive dividends. The domain model becomes the single source of truth for business rules.

        ### 2. Clean Architecture Enables Flexibility
        When requirements changed, our Clean Architecture made it relatively easy to adapt. Business logic stayed in the domain, and we could modify infrastructure without affecting core functionality.

        ### 3. Real-time Features Need Careful Design
        SignalR is powerful, but managing connections and state in a multi-tenant environment requires careful consideration of scalability and security.

        ### 4. Performance Matters from Day One
        Implementing performance monitoring and optimization early saves significant refactoring time later.

        ## Technical Stack

        - **Backend**: ASP.NET Core 8.0, Entity Framework Core
        - **Architecture**: Clean Architecture, Domain-Driven Design
        - **Real-time**: SignalR for live updates
        - **Database**: PostgreSQL with strategic indexing
        - **Cloud**: AWS (EC2, RDS, S3)
        - **Frontend**: Tailwind CSS, JavaScript

        ## Results

        The system now successfully handles:
        - Multiple warehouse locations with unified operations
        - Real-time inventory tracking across the network
        - Multi-client data isolation and security
        - Scalable architecture ready for expansion

        ## Conclusion

        This project reinforced my belief that good architecture is not just about following patterns—it's about solving real business problems while building systems that can evolve with changing requirements.

        The experience has prepared me well for tackling similar challenges at global technology companies, where scalable architecture and clean code are essential for success.

        *Follow my blog for more insights about enterprise software development and my journey toward global technology companies.*",
                    Excerpt = "Deep dive into building enterprise warehouse management systems using Clean Architecture and Domain-Driven Design, with real-world examples and practical lessons learned from leading development teams.",
                    Category = "Technical Tutorial",
                    Tags = "Clean Architecture, DDD, Enterprise Software, ASP.NET Core, SignalR, Multi-tenant",
                    FeaturedImage = "/images/blog/enterprise-wms-architecture.jpg",
                    IsPublished = true,
                    PublishedDate = new DateTime(2025, 6, 16, 10, 0, 0, DateTimeKind.Utc), // Static date
                    CreatedDate = new DateTime(2025, 6, 16, 10, 0, 0, DateTimeKind.Utc),   // Static date
                    UpdatedDate = null,
                    ReadTimeMinutes = 8,
                    ViewCount = 0,
                    MetaDescription = "Learn how to build enterprise warehouse management systems using Clean Architecture and Domain-Driven Design with real-world examples and lessons learned.",
                    MetaKeywords = "Clean Architecture, Enterprise Software, WMS, DDD, ASP.NET Core, Multi-tenant",
                    Author = "Arif Tan",
                    AuthorEmail = "ariftan7788@gmail.com"
                },
                new BlogPost
                {
                    Id = 2,
                    Title = "Building an AI-Powered WhatsApp Health Assistant: Technical Deep Dive",
                    Slug = "building-ai-powered-whatsapp-health-assistant-technical-deep-dive",
                    Content = @"# Building an AI-Powered WhatsApp Health Assistant: Technical Deep Dive

        Creating an intelligent health assistant for WhatsApp Business posed unique challenges: handling Indonesian language nuances, managing API costs, and ensuring real-time responsiveness for health consultations. This post details the technical architecture and optimization strategies that achieved 60-70% cost reduction while maintaining excellent user experience.

        ## The Business Challenge

        Indonesian health product businesses face several obstacles:
        - Limited availability of health consultants
        - High operational costs for 24/7 support
        - Language barriers with international health AI solutions
        - Complex local payment methods (COD, e-wallets)
        - Need for immediate health guidance

        ## Architecture Overview

        The system architecture follows a microservices approach optimized for conversational AI:

        ```typescript
        // Core conversation handler
        export class ConversationManager {
            private redis: RedisClient;
            private aiService: ClaudeAIService;
            private sessionStore: SessionStore;
            
            constructor(
                private config: ConversationConfig,
                private logger: Logger
            ) {
                this.redis = new RedisClient(config.redis);
                this.aiService = new ClaudeAIService(config.claude);
                this.sessionStore = new SessionStore(this.redis);
            }
            
            async handleMessage(message: WhatsAppMessage): Promise<AIResponse> {
                const session = await this.sessionStore.getSession(message.from);
                const optimizedPrompt = await this.optimizePrompt(message, session);
                
                return await this.aiService.generateResponse(optimizedPrompt, {
                    maxTokens: this.calculateOptimalTokens(session),
                    temperature: 0.7,
                    stopSequences: [""END_CONVERSATION""]
                });
            }
        }
        ```

        ## Token Optimization: Achieving 60-70% Cost Reduction

        The most critical aspect was optimizing AI API costs without compromising response quality:

        ### 1. Dynamic Context Compression

        ```typescript
        export class ContextCompressor {
            private compressionRatio: number = 0.4;
            
            async compressContext(conversation: ConversationHistory): Promise<string> {
                const importantMessages = this.extractKeyMessages(conversation);
                const compressedContext = await this.summarizeContext(importantMessages);
                
                return this.formatOptimalPrompt(compressedContext);
            }
            
            private extractKeyMessages(conversation: ConversationHistory): Message[] {
                return conversation.messages.filter(msg => 
                    msg.type === 'health_symptom' || 
                    msg.type === 'product_inquiry' ||
                    msg.importance > 0.7
                );
            }
        }
        ```

        ### 2. Intelligent Caching Strategy

        ```typescript
        export class ResponseCache {
            private cache: Map<string, CachedResponse> = new Map();
            
            async getCachedResponse(messageHash: string): Promise<CachedResponse | null> {
                const cached = this.cache.get(messageHash);
                
                if (cached && !this.isExpired(cached)) {
                    return cached;
                }
                
                return null;
            }
            
            async cacheResponse(messageHash: string, response: AIResponse): Promise<void> {
                const compressedResponse = await this.compressResponse(response);
                
                this.cache.set(messageHash, {
                    response: compressedResponse,
                    timestamp: Date.now(),
                    hitCount: 0
                });
            }
        }
        ```

        ## Real-Time Conversation State Management

        Managing 1000+ concurrent conversations required sophisticated state handling:

        ```typescript
        export class SessionStore {
            private redis: RedisClient;
            private readonly SESSION_TTL = 3600; // 1 hour
            
            async getSession(userId: string): Promise<ConversationSession> {
                const sessionKey = `session:${userId}`;
                const sessionData = await this.redis.get(sessionKey);
                
                if (!sessionData) {
                    return await this.createNewSession(userId);
                }
                
                return JSON.parse(sessionData);
            }
            
            async updateSession(userId: string, update: Partial<ConversationSession>): Promise<void> {
                const sessionKey = `session:${userId}`;
                const currentSession = await this.getSession(userId);
                
                const updatedSession = { ...currentSession, ...update };
                
                await this.redis.setex(
                    sessionKey, 
                    this.SESSION_TTL, 
                    JSON.stringify(updatedSession)
                );
            }
        }
        ```

        ## Indonesian Language Processing

        Health consultations require precise language understanding:

        ```typescript
        export class IndonesianHealthProcessor {
            private healthKeywords: Map<string, HealthCategory> = new Map([
                ['sakit kepala', HealthCategory.HEADACHE],
                ['demam', HealthCategory.FEVER],
                ['batuk', HealthCategory.COUGH],
                ['flu', HealthCategory.COLD],
                ['vitamin', HealthCategory.SUPPLEMENTS]
            ]);
            
            async processHealthQuery(message: string): Promise<HealthAnalysis> {
                const normalizedMessage = this.normalizeIndonesian(message);
                const detectedSymptoms = this.extractSymptoms(normalizedMessage);
                const severity = this.assessSeverity(detectedSymptoms);
                
                return {
                    symptoms: detectedSymptoms,
                    severity,
                    recommendedProducts: await this.getRecommendations(detectedSymptoms),
                    requiresHumanConsultation: severity > 0.8
                };
            }
        }
        ```

        ## WhatsApp Business API Integration

        Robust integration with proper error handling and rate limiting:

        ```typescript
        export class WhatsAppService {
            private client: WhatsAppBusinessClient;
            private rateLimiter: RateLimiter;
            
            async sendMessage(to: string, message: WhatsAppMessage): Promise<SendResult> {
                await this.rateLimiter.waitForToken();
                
                try {
                    const result = await this.client.sendMessage({
                        to,
                        type: message.type,
                        text: message.text,
                        template: message.template
                    });
                    
                    return { success: true, messageId: result.id };
                } catch (error) {
                    this.logger.error('Failed to send WhatsApp message', { error, to });
                    return { success: false, error: error.message };
                }
            }
        }
        ```

        ## Analytics and Business Intelligence

        Comprehensive tracking for business insights:

        ```typescript
        export class AnalyticsService {
            private db: SQLiteDatabase;
            
            async trackConversation(event: ConversationEvent): Promise<void> {
                await this.db.run(`
                    INSERT INTO conversation_analytics 
                    (user_id, event_type, timestamp, metadata)
                    VALUES (?, ?, ?, ?)
                `, [
                    event.userId,
                    event.type,
                    event.timestamp,
                    JSON.stringify(event.metadata)
                ]);
            }
            
            async generateBusinessReport(dateRange: DateRange): Promise<BusinessReport> {
                const metrics = await this.db.all(`
                    SELECT 
                        COUNT(*) as total_conversations,
                        AVG(response_time) as avg_response_time,
                        SUM(cost_saved) as total_cost_saved
                    FROM conversation_analytics
                    WHERE timestamp BETWEEN ? AND ?
                `, [dateRange.start, dateRange.end]);
                
                return new BusinessReport(metrics);
            }
        }
        ```

        ## Performance Optimization Results

        The optimization strategies delivered significant improvements:

        ### Cost Reduction Metrics:
        - **60-70% API cost reduction** through intelligent caching and compression
        - **40% reduction in token usage** via context optimization
        - **85% cache hit rate** for common health queries

        ### Performance Metrics:
        - **Sub-second response times** for 95% of queries
        - **1000+ concurrent users** with stable performance
        - **99.9% uptime** with proper error handling

        ## Deployment Architecture

        ```dockerfile
        # Multi-stage Docker build for production
        FROM node:18-alpine AS builder
        WORKDIR /app
        COPY package*.json ./
        RUN npm ci --only=production
        
        FROM node:18-alpine AS production
        WORKDIR /app
        COPY --from=builder /app/node_modules ./node_modules
        COPY . .
        
        EXPOSE 3000
        CMD [""node"", ""dist/index.js""]
        ```

        ## Key Technical Lessons

        ### 1. AI Cost Optimization is Critical
        Without proper token management, AI costs can spiral quickly. Intelligent caching and compression are essential for commercial viability.

        ### 2. Real-time State Management at Scale
        Redis-based session management with proper TTL and cleanup strategies enables handling thousands of concurrent conversations.

        ### 3. Indonesian Language Processing
        Generic international AI models need significant customization for Indonesian health domain terminology and cultural context.

        ### 4. WhatsApp Business API Reliability
        Proper error handling, rate limiting, and retry mechanisms are crucial for maintaining service quality.

        ## Technical Stack Summary

        - **Runtime**: Node.js 18 with TypeScript
        - **AI Integration**: Claude AI API with custom optimization
        - **State Management**: Redis for session storage
        - **Database**: SQLite for analytics and logging
        - **Messaging**: WhatsApp Business API
        - **Deployment**: Docker with multi-stage builds
        - **Monitoring**: Custom analytics dashboard

        ## Future Enhancements

        - Integration with Indonesian health databases
        - Voice message processing capabilities
        - Advanced symptom analysis algorithms
        - Multi-language support expansion
        - Telemedicine integration

        ## Conclusion

        This project demonstrated that with proper architecture and optimization, AI-powered conversational systems can deliver both excellent user experience and commercial viability. The 60-70% cost reduction while maintaining quality responses proves that technical excellence directly translates to business value.

        The experience has prepared me for building large-scale AI systems in global technology companies, where cost optimization and performance are critical success factors.

        *Follow my blog for more insights about AI system architecture and optimization strategies.*",
                    Excerpt = "Technical deep dive into building a cost-optimized AI-powered WhatsApp health assistant with 60-70% cost reduction, real-time conversation management, and Indonesian language processing.",
                    Category = "AI & Automation",
                    Tags = "AI, WhatsApp Business, TypeScript, Node.js, Redis, Cost Optimization, Indonesian NLP, Conversational AI",
                    FeaturedImage = "/images/blog/whatsapp-health-assistant.jpg",
                    IsPublished = true,
                    PublishedDate = new DateTime(2025, 6, 18, 14, 30, 0, DateTimeKind.Utc),
                    CreatedDate = new DateTime(2025, 6, 18, 14, 30, 0, DateTimeKind.Utc),
                    UpdatedDate = null,
                    ReadTimeMinutes = 12,
                    ViewCount = 0,
                    MetaDescription = "Learn how to build cost-optimized AI-powered WhatsApp health assistants with 60-70% cost reduction through intelligent caching, compression, and real-time conversation management.",
                    MetaKeywords = "AI WhatsApp Bot, Cost Optimization, TypeScript, Node.js, Indonesian NLP, Conversational AI, Health Assistant, Claude AI",
                    Author = "Arif Tan",
                    AuthorEmail = "ariftan7788@gmail.com"
                }
            );
        }
    }
}