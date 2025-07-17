using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArifTanPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Content",
                value: "# Building Enterprise WMS with Clean Architecture: Real-World Implementation\n\n        Leading the development of a multi-warehouse management system has been one of the most challenging and rewarding projects of my career. This post shares the practical lessons learned while implementing Clean Architecture and Domain-Driven Design in a real enterprise environment.\n\n        ## The Challenge\n\n        Our client needed a comprehensive warehouse management system that could:\n        - Handle multiple warehouse locations\n        - Support multi-client operations with data isolation  \n        - Provide real-time inventory tracking\n        - Scale for future warehouse network expansion\n\n        ## Clean Architecture in Practice\n\n        ### Domain Layer Design\n\n        The heart of our system lies in properly modeled domain entities:\n\n        ```csharp\n        public class WarehouseLocation : Entity\n        {\n            public LocationId Id { get; private set; }\n            public string LocationCode { get; private set; }\n            public ClientId ClientId { get; private set; }\n            public WarehouseId WarehouseId { get; private set; }\n            \n            private readonly List<InventoryItem> _inventoryItems = new();\n            public IReadOnlyList<InventoryItem> InventoryItems => _inventoryItems.AsReadOnly();\n            \n            public Result<InventoryItem> AddInventory(Product product, Quantity quantity, UserId userId)\n            {\n                if (quantity.IsZero())\n                    return Result.Failure<InventoryItem>(\"Quantity must be greater than zero\");\n                    \n                var inventoryItem = InventoryItem.Create(product, quantity, Id, userId);\n                _inventoryItems.Add(inventoryItem);\n                \n                AddDomainEvent(new InventoryAddedEvent(Id, product.Id, quantity));\n                \n                return Result.Success(inventoryItem);\n            }\n        }\n        ```\n\n        ### Application Layer Use Cases\n\n        ```csharp\n        public class TransferInventoryUseCase : ITransferInventoryUseCase\n        {\n            private readonly IWarehouseLocationRepository _locationRepository;\n            private readonly IInventoryTransferService _transferService;\n            private readonly IUnitOfWork _unitOfWork;\n            \n            public async Task<r> ExecuteAsync(TransferInventoryCommand command)\n            {\n                var sourceLocation = await _locationRepository.GetByIdAsync(command.SourceLocationId);\n                var targetLocation = await _locationRepository.GetByIdAsync(command.TargetLocationId);\n                \n                var transferResult = _transferService.TransferInventory(\n                    sourceLocation, \n                    targetLocation, \n                    command.ProductId, \n                    command.Quantity,\n                    command.UserId);\n                    \n                if (transferResult.IsFailure)\n                    return transferResult;\n                    \n                await _unitOfWork.SaveChangesAsync();\n                return Result.Success();\n            }\n        }\n        ```\n\n        ## Real-Time Updates with SignalR\n\n        One of the key requirements was real-time location grid updates for warehouse operators:\n\n        ```csharp\n        [Authorize]\n        public class WarehouseHub : Hub\n        {\n            public async Task JoinWarehouseGroup(string warehouseId)\n            {\n                await Groups.AddToGroupAsync(Context.ConnectionId, $\"warehouse_{warehouseId}\");\n            }\n            \n            public async Task NotifyLocationUpdate(string warehouseId, LocationUpdateDto update)\n            {\n                await Clients.Group($\"warehouse_{warehouseId}\")\n                    .SendAsync(\"LocationUpdated\", update);\n            }\n        }\n        ```\n\n        ## Multi-Tenant Architecture\n\n        Ensuring client data isolation was critical:\n\n        ```csharp\n        public class WarehouseLocationRepository : IWarehouseLocationRepository\n        {\n            private readonly ApplicationDbContext _context;\n            private readonly ICurrentUserService _currentUser;\n            \n            public async Task<List<WarehouseLocation>> GetByWarehouseAsync(WarehouseId warehouseId)\n            {\n                var clientId = _currentUser.GetClientId();\n                \n                return await _context.WarehouseLocations\n                    .Where(l => l.WarehouseId == warehouseId && l.ClientId == clientId)\n                    .ToListAsync();\n            }\n        }\n        ```\n\n        ## Performance Optimization\n\n        Strategic database indexing proved crucial for performance:\n\n        ```sql\n        CREATE INDEX IX_WarehouseLocation_ClientId_WarehouseId \n        ON WarehouseLocations (ClientId, WarehouseId);\n\n        CREATE INDEX IX_InventoryItem_LocationId_ProductId \n        ON InventoryItems (LocationId, ProductId) \n        INCLUDE (Quantity, LastUpdated);\n        ```\n\n        ## Key Lessons Learned\n\n        ### 1. Domain Modeling is Critical\n        Spending time upfront to understand the domain and model it correctly pays massive dividends. The domain model becomes the single source of truth for business rules.\n\n        ### 2. Clean Architecture Enables Flexibility\n        When requirements changed, our Clean Architecture made it relatively easy to adapt. Business logic stayed in the domain, and we could modify infrastructure without affecting core functionality.\n\n        ### 3. Real-time Features Need Careful Design\n        SignalR is powerful, but managing connections and state in a multi-tenant environment requires careful consideration of scalability and security.\n\n        ### 4. Performance Matters from Day One\n        Implementing performance monitoring and optimization early saves significant refactoring time later.\n\n        ## Technical Stack\n\n        - **Backend**: ASP.NET Core 8.0, Entity Framework Core\n        - **Architecture**: Clean Architecture, Domain-Driven Design\n        - **Real-time**: SignalR for live updates\n        - **Database**: PostgreSQL with strategic indexing\n        - **Cloud**: AWS (EC2, RDS, S3)\n        - **Frontend**: Tailwind CSS, JavaScript\n\n        ## Results\n\n        The system now successfully handles:\n        - Multiple warehouse locations with unified operations\n        - Real-time inventory tracking across the network\n        - Multi-client data isolation and security\n        - Scalable architecture ready for expansion\n\n        ## Conclusion\n\n        This project reinforced my belief that good architecture is not just about following patterns—it's about solving real business problems while building systems that can evolve with changing requirements.\n\n        The experience has prepared me well for tackling similar challenges at global technology companies, where scalable architecture and clean code are essential for success.\n\n        *Follow my blog for more insights about enterprise software development and my journey toward global technology companies.*");

            migrationBuilder.InsertData(
                table: "BlogPosts",
                columns: new[] { "Id", "Author", "AuthorEmail", "Category", "Content", "CreatedDate", "Excerpt", "FeaturedImage", "IsPublished", "MetaDescription", "MetaKeywords", "PublishedDate", "ReadTimeMinutes", "Slug", "Tags", "Title", "UpdatedDate", "ViewCount" },
                values: new object[] { 2, "Arif Tan", "ariftan7788@gmail.com", "AI & Automation", "# Building an AI-Powered WhatsApp Health Assistant: Technical Deep Dive\n\n        Creating an intelligent health assistant for WhatsApp Business posed unique challenges: handling Indonesian language nuances, managing API costs, and ensuring real-time responsiveness for health consultations. This post details the technical architecture and optimization strategies that achieved 60-70% cost reduction while maintaining excellent user experience.\n\n        ## The Business Challenge\n\n        Indonesian health product businesses face several obstacles:\n        - Limited availability of health consultants\n        - High operational costs for 24/7 support\n        - Language barriers with international health AI solutions\n        - Complex local payment methods (COD, e-wallets)\n        - Need for immediate health guidance\n\n        ## Architecture Overview\n\n        The system architecture follows a microservices approach optimized for conversational AI:\n\n        ```typescript\n        // Core conversation handler\n        export class ConversationManager {\n            private redis: RedisClient;\n            private aiService: ClaudeAIService;\n            private sessionStore: SessionStore;\n            \n            constructor(\n                private config: ConversationConfig,\n                private logger: Logger\n            ) {\n                this.redis = new RedisClient(config.redis);\n                this.aiService = new ClaudeAIService(config.claude);\n                this.sessionStore = new SessionStore(this.redis);\n            }\n            \n            async handleMessage(message: WhatsAppMessage): Promise<AIResponse> {\n                const session = await this.sessionStore.getSession(message.from);\n                const optimizedPrompt = await this.optimizePrompt(message, session);\n                \n                return await this.aiService.generateResponse(optimizedPrompt, {\n                    maxTokens: this.calculateOptimalTokens(session),\n                    temperature: 0.7,\n                    stopSequences: [\"END_CONVERSATION\"]\n                });\n            }\n        }\n        ```\n\n        ## Token Optimization: Achieving 60-70% Cost Reduction\n\n        The most critical aspect was optimizing AI API costs without compromising response quality:\n\n        ### 1. Dynamic Context Compression\n\n        ```typescript\n        export class ContextCompressor {\n            private compressionRatio: number = 0.4;\n            \n            async compressContext(conversation: ConversationHistory): Promise<string> {\n                const importantMessages = this.extractKeyMessages(conversation);\n                const compressedContext = await this.summarizeContext(importantMessages);\n                \n                return this.formatOptimalPrompt(compressedContext);\n            }\n            \n            private extractKeyMessages(conversation: ConversationHistory): Message[] {\n                return conversation.messages.filter(msg => \n                    msg.type === 'health_symptom' || \n                    msg.type === 'product_inquiry' ||\n                    msg.importance > 0.7\n                );\n            }\n        }\n        ```\n\n        ### 2. Intelligent Caching Strategy\n\n        ```typescript\n        export class ResponseCache {\n            private cache: Map<string, CachedResponse> = new Map();\n            \n            async getCachedResponse(messageHash: string): Promise<CachedResponse | null> {\n                const cached = this.cache.get(messageHash);\n                \n                if (cached && !this.isExpired(cached)) {\n                    return cached;\n                }\n                \n                return null;\n            }\n            \n            async cacheResponse(messageHash: string, response: AIResponse): Promise<void> {\n                const compressedResponse = await this.compressResponse(response);\n                \n                this.cache.set(messageHash, {\n                    response: compressedResponse,\n                    timestamp: Date.now(),\n                    hitCount: 0\n                });\n            }\n        }\n        ```\n\n        ## Real-Time Conversation State Management\n\n        Managing 1000+ concurrent conversations required sophisticated state handling:\n\n        ```typescript\n        export class SessionStore {\n            private redis: RedisClient;\n            private readonly SESSION_TTL = 3600; // 1 hour\n            \n            async getSession(userId: string): Promise<ConversationSession> {\n                const sessionKey = `session:${userId}`;\n                const sessionData = await this.redis.get(sessionKey);\n                \n                if (!sessionData) {\n                    return await this.createNewSession(userId);\n                }\n                \n                return JSON.parse(sessionData);\n            }\n            \n            async updateSession(userId: string, update: Partial<ConversationSession>): Promise<void> {\n                const sessionKey = `session:${userId}`;\n                const currentSession = await this.getSession(userId);\n                \n                const updatedSession = { ...currentSession, ...update };\n                \n                await this.redis.setex(\n                    sessionKey, \n                    this.SESSION_TTL, \n                    JSON.stringify(updatedSession)\n                );\n            }\n        }\n        ```\n\n        ## Indonesian Language Processing\n\n        Health consultations require precise language understanding:\n\n        ```typescript\n        export class IndonesianHealthProcessor {\n            private healthKeywords: Map<string, HealthCategory> = new Map([\n                ['sakit kepala', HealthCategory.HEADACHE],\n                ['demam', HealthCategory.FEVER],\n                ['batuk', HealthCategory.COUGH],\n                ['flu', HealthCategory.COLD],\n                ['vitamin', HealthCategory.SUPPLEMENTS]\n            ]);\n            \n            async processHealthQuery(message: string): Promise<HealthAnalysis> {\n                const normalizedMessage = this.normalizeIndonesian(message);\n                const detectedSymptoms = this.extractSymptoms(normalizedMessage);\n                const severity = this.assessSeverity(detectedSymptoms);\n                \n                return {\n                    symptoms: detectedSymptoms,\n                    severity,\n                    recommendedProducts: await this.getRecommendations(detectedSymptoms),\n                    requiresHumanConsultation: severity > 0.8\n                };\n            }\n        }\n        ```\n\n        ## WhatsApp Business API Integration\n\n        Robust integration with proper error handling and rate limiting:\n\n        ```typescript\n        export class WhatsAppService {\n            private client: WhatsAppBusinessClient;\n            private rateLimiter: RateLimiter;\n            \n            async sendMessage(to: string, message: WhatsAppMessage): Promise<SendResult> {\n                await this.rateLimiter.waitForToken();\n                \n                try {\n                    const result = await this.client.sendMessage({\n                        to,\n                        type: message.type,\n                        text: message.text,\n                        template: message.template\n                    });\n                    \n                    return { success: true, messageId: result.id };\n                } catch (error) {\n                    this.logger.error('Failed to send WhatsApp message', { error, to });\n                    return { success: false, error: error.message };\n                }\n            }\n        }\n        ```\n\n        ## Analytics and Business Intelligence\n\n        Comprehensive tracking for business insights:\n\n        ```typescript\n        export class AnalyticsService {\n            private db: SQLiteDatabase;\n            \n            async trackConversation(event: ConversationEvent): Promise<void> {\n                await this.db.run(`\n                    INSERT INTO conversation_analytics \n                    (user_id, event_type, timestamp, metadata)\n                    VALUES (?, ?, ?, ?)\n                `, [\n                    event.userId,\n                    event.type,\n                    event.timestamp,\n                    JSON.stringify(event.metadata)\n                ]);\n            }\n            \n            async generateBusinessReport(dateRange: DateRange): Promise<BusinessReport> {\n                const metrics = await this.db.all(`\n                    SELECT \n                        COUNT(*) as total_conversations,\n                        AVG(response_time) as avg_response_time,\n                        SUM(cost_saved) as total_cost_saved\n                    FROM conversation_analytics\n                    WHERE timestamp BETWEEN ? AND ?\n                `, [dateRange.start, dateRange.end]);\n                \n                return new BusinessReport(metrics);\n            }\n        }\n        ```\n\n        ## Performance Optimization Results\n\n        The optimization strategies delivered significant improvements:\n\n        ### Cost Reduction Metrics:\n        - **60-70% API cost reduction** through intelligent caching and compression\n        - **40% reduction in token usage** via context optimization\n        - **85% cache hit rate** for common health queries\n\n        ### Performance Metrics:\n        - **Sub-second response times** for 95% of queries\n        - **1000+ concurrent users** with stable performance\n        - **99.9% uptime** with proper error handling\n\n        ## Deployment Architecture\n\n        ```dockerfile\n        # Multi-stage Docker build for production\n        FROM node:18-alpine AS builder\n        WORKDIR /app\n        COPY package*.json ./\n        RUN npm ci --only=production\n        \n        FROM node:18-alpine AS production\n        WORKDIR /app\n        COPY --from=builder /app/node_modules ./node_modules\n        COPY . .\n        \n        EXPOSE 3000\n        CMD [\"node\", \"dist/index.js\"]\n        ```\n\n        ## Key Technical Lessons\n\n        ### 1. AI Cost Optimization is Critical\n        Without proper token management, AI costs can spiral quickly. Intelligent caching and compression are essential for commercial viability.\n\n        ### 2. Real-time State Management at Scale\n        Redis-based session management with proper TTL and cleanup strategies enables handling thousands of concurrent conversations.\n\n        ### 3. Indonesian Language Processing\n        Generic international AI models need significant customization for Indonesian health domain terminology and cultural context.\n\n        ### 4. WhatsApp Business API Reliability\n        Proper error handling, rate limiting, and retry mechanisms are crucial for maintaining service quality.\n\n        ## Technical Stack Summary\n\n        - **Runtime**: Node.js 18 with TypeScript\n        - **AI Integration**: Claude AI API with custom optimization\n        - **State Management**: Redis for session storage\n        - **Database**: SQLite for analytics and logging\n        - **Messaging**: WhatsApp Business API\n        - **Deployment**: Docker with multi-stage builds\n        - **Monitoring**: Custom analytics dashboard\n\n        ## Future Enhancements\n\n        - Integration with Indonesian health databases\n        - Voice message processing capabilities\n        - Advanced symptom analysis algorithms\n        - Multi-language support expansion\n        - Telemedicine integration\n\n        ## Conclusion\n\n        This project demonstrated that with proper architecture and optimization, AI-powered conversational systems can deliver both excellent user experience and commercial viability. The 60-70% cost reduction while maintaining quality responses proves that technical excellence directly translates to business value.\n\n        The experience has prepared me for building large-scale AI systems in global technology companies, where cost optimization and performance are critical success factors.\n\n        *Follow my blog for more insights about AI system architecture and optimization strategies.*", new DateTime(2025, 6, 18, 14, 30, 0, 0, DateTimeKind.Utc), "Technical deep dive into building a cost-optimized AI-powered WhatsApp health assistant with 60-70% cost reduction, real-time conversation management, and Indonesian language processing.", "/images/blog/whatsapp-health-assistant.jpg", true, "Learn how to build cost-optimized AI-powered WhatsApp health assistants with 60-70% cost reduction through intelligent caching, compression, and real-time conversation management.", "AI WhatsApp Bot, Cost Optimization, TypeScript, Node.js, Indonesian NLP, Conversational AI, Health Assistant, Claude AI", new DateTime(2025, 6, 18, 14, 30, 0, 0, DateTimeKind.Utc), 12, "building-ai-powered-whatsapp-health-assistant-technical-deep-dive", "AI, WhatsApp Business, TypeScript, Node.js, Redis, Cost Optimization, Indonesian NLP, Conversational AI", "Building an AI-Powered WhatsApp Health Assistant: Technical Deep Dive", null, 0 });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Challenges", "Description", "EndDate", "GitHubUrl", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "AI/Automation Platform", "Token optimization for cost efficiency, real-time conversation state management at scale, Indonesian language processing for health domain, WhatsApp Business API integration complexities, concurrent user session handling, local payment method ", "Conversational AI system for Indonesian health product business with advanced token optimization and real-time conversation management", new DateTime(2025, 7, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://github.com/Ariftan77/whatsapp-health-assistant", "AI API cost optimization strategies, real-time conversation management architecture, Indonesian market localization requirements, WhatsApp Business API best practices, scalable chat system design patterns, importance of analytics in conversational AI systems", "Architected and developed an enterprise-grade conversational AI system using TypeScript/Node.js for WhatsApp Business automation targeting Indonesian health product business. The system features advanced token optimization algorithms achieving 60-70% API cost reduction, real-time conversation state management supporting 1000+ concurrent users with sub-second response times. Specialized for Indonesian market with Bahasa Indonesia natural language processing, local payment integration (COD, e-wallets), and comprehensive analytics dashboard for business insights and customer journey tracking.", "AI-Powered WhatsApp Health Assistant Bot", "Implemented advanced token optimization algorithms with caching and compression achieving 60-70% cost reduction, built Redis-based conversation state management for 1000+ concurrent users, developed specialized Bahasa Indonesia NLP for health consultations, created robust WhatsApp Business API integration with proper error handling.", new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "TypeScript, Node.js, Express.js, Redis, Claude AI API, WhatsApp Business API, SQLite, Docker" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Challenges", "Description", "EndDate", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Enterprise Software", "Domain modeling for warehouse operations using DDD principles, implementing Clean Architecture in enterprise client environment, complex inventory and invoice domain logic, tight MVP delivery timeline with proper architectural foundation", "Enterprise warehouse operations management system for international client with inventory and invoice modules", new DateTime(2024, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Clean Architecture implementation in enterprise projects, Domain-Driven Design tactical patterns, leading development teams using modern architectural approaches, client stakeholder management, importance of architectural decisions in project success", "Technical lead for developing a comprehensive warehouse operations management system for a global client. The system handles warehouse job operations, packaging material inventory management, and invoice processing. Built using layered architecture principles with a team of 3 engineers, delivering significant operational efficiency improvements for a client transitioning from manual processes to digital warehouse management.", "Warehouse Job Management Web Application", "Applied Clean Architecture principles with clear domain/application/infrastructure separation, designed domain models for warehouse operations using DDD tactical patterns, implemented repository and unit of work patterns, created comprehensive API layer with proper dependency inversion, deployed on AWS EC2", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core 7.0, PostgreSQL, Entity Framework Core, Clean Architecture, Domain-Driven Design, Bootstrap, AWS (S3, EC2, RDS), REST APIs, JavaScript" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Challenges", "Description", "EndDate", "IsFeatured", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Integration Platform", "Multiple client EDI format variations, reliable SFTP monitoring and file processing, error handling and notification systems, automated workflow management, maintaining system reliability for critical logistics operations", "Automated EDI file processing system with SFTP monitoring, API integrations, and multi-client support for logistics operations", new DateTime(2023, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), true, "Node.js for enterprise file processing, SFTP protocol implementation, building reliable automated systems, multi-client platform architecture, importance of comprehensive error handling in logistics systems", "Developed a comprehensive EDI processing platform serving multiple logistics clients with automated file processing, SFTP monitoring, and API integrations. The system includes SFTP folder watcher programs, EDI file processing engines, XML generation APIs, and automated notification systems. Handled diverse client requirements with custom processing logic for each client while maintaining a unified platform architecture.", "Multi-Client EDI Processing & Integration Platform", "Built robust SFTP folder watcher with Node.js, implemented flexible EDI parsing engine supporting multiple formats, created comprehensive error handling with email and Teams notifications, designed modular architecture for easy client onboarding, automated file processing with proper logging and monitoring", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Node.js, SFTP protocols, XML processing, Email APIs, Microsoft Teams integration, Task Scheduling, File System monitoring" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Challenges", "Description", "EndDate", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Document Processing & Logistics", "Accurate OCR data extraction from varying document formats, automated file processing reliability, system deployment", "Python-based OCR system for manifest processing", new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Python OCR implementation for production systems, automated document processing workflows.", "Developed an intelligent document processing system using Python OCR to extract data from manifest files. The OCR system automatically processes uploaded documents via task scheduler, extracts relevant data, consolidates information, and saves to database.", "OCR Document Processing & Logistics Container System", "Implemented robust Python OCR processing with error handling, designed efficient data consolidation algorithms.", new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Python, OCR libraries, Task Scheduler, SQL Server, File processing" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Challenges", "Description", "EndDate", "LessonsLearned", "LiveUrl", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Manufacturing Systems", "Complex manufacturing workflow integration, high-volume transaction processing, ERP system integration, custom reporting requirements, user training and system adoption in manufacturing environment", "Comprehensive warehouse management system with desktop applications for electronics manufacturing operations", new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Desktop application development for manufacturing, database design for high-volume operations, ERP system integration patterns, importance of user training in system adoption, manufacturing industry requirements and workflows", null, "Contributed to developing a complete warehouse management system from scratch for electronics manufacturing operations. The system included desktop applications for warehouse operations, production tracking, finished goods management, and shipment processing. Handled high-volume manufacturing workflows with integration to existing ERP systems and custom label printing solutions.", "Electronics Manufacturing WMS & Desktop Applications", "Built robust desktop applications using C# Windows Forms, implemented efficient database design with Entity Framework, created seamless ERP integrations, developed comprehensive reporting system, provided extensive user training and support", new DateTime(2018, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C# Windows Forms, Entity Framework, SQL Server, Web Services, Crystal Reports, Manufacturing integrations" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Category", "Challenges", "Description", "EndDate", "FeaturedImage", "GitHubUrl", "ImageGallery", "IsFeatured", "LessonsLearned", "LiveUrl", "LongDescription", "Name", "Solutions", "SortOrder", "StartDate", "Technologies" },
                values: new object[] { 7, "Web Development", "Professional design and user experience, responsive layout across devices, SEO optimization, content management system design, performance optimization, modern web development practices", "Professional portfolio website showcasing software engineering expertise and career journey", new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "https://github.com/Ariftan77/personal-site", "", false, "Modern web development practices, responsive design principles, SEO optimization techniques, personal branding through technology, importance of professional online presence in career development", "https://ariftan.com", "Designed and developed a comprehensive personal portfolio website to showcase software engineering expertise, project portfolio, and professional journey. The website features responsive design, modern UI/UX, project galleries, blog functionality, and contact forms. Built with focus on performance, SEO optimization, and professional presentation for career advancement opportunities.", "Personal Portfolio Website", "Implemented clean and modern design with Bootstrap 5, created responsive layouts for all device sizes, optimized for search engines with proper meta tags and structured data, built custom CMS functionality with Entity Framework, optimized loading performance and user experience", 7, new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core 8.0, Razor Pages, Bootstrap 5, Entity Framework Core, SQLite, HTML/CSS, JavaScript" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsVisible",
                value: false);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsVisible",
                value: false);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10,
                column: "IsVisible",
                value: false);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11,
                column: "IsVisible",
                value: false);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12,
                column: "IsVisible",
                value: false);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 13,
                column: "IsVisible",
                value: false);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 14,
                column: "IsVisible",
                value: false);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "devicon-git-plain", "Git", 9, 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "devicon-amazonwebservices-plain", "AWS (S3, RDS)", 8, 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "devicon-amazonwebservices-plain", "AWS EC2 & Lightsail", 8, 5 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "devicon-docker-plain", "Docker", 7, 3 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "fas fa-bolt", false, "Power Automate", 4 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "devicon-azure-plain", false, "Azure", 8 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "devicon-azure-plain", false, "Azure DevOps", 9 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Databases", "devicon-microsoftsqlserver-plain", "SQL Server", 9, 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Databases", "devicon-postgresql-plain", "PostgreSQL", 9, 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Icon", "Name", "SortOrder" },
                values: new object[] { "devicon-dot-net-plain", "Entity Framework Core", 3 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Icon", "Name", "SortOrder" },
                values: new object[] { "devicon-sqlite-plain", "SQLite", 4 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1,
                column: "Content",
                value: "# Building Enterprise WMS with Clean Architecture: Real-World Implementation\n\n        Leading the development of a multi-warehouse management system has been one of the most challenging and rewarding projects of my career. This post shares the practical lessons learned while implementing Clean Architecture and Domain-Driven Design in a real enterprise environment.\n\n        ## The Challenge\n\n        Our client needed a comprehensive warehouse management system that could:\n        - Handle multiple warehouse locations\n        - Support multi-client operations with data isolation  \n        - Provide real-time inventory tracking\n        - Scale for future warehouse network expansion\n        - Integrate with existing ERP systems\n\n        ## Clean Architecture in Practice\n\n        ### Domain Layer Design\n\n        The heart of our system lies in properly modeled domain entities:\n\n        ```csharp\n        public class WarehouseLocation : Entity\n        {\n            public LocationId Id { get; private set; }\n            public string LocationCode { get; private set; }\n            public ClientId ClientId { get; private set; }\n            public WarehouseId WarehouseId { get; private set; }\n            \n            private readonly List<InventoryItem> _inventoryItems = new();\n            public IReadOnlyList<InventoryItem> InventoryItems => _inventoryItems.AsReadOnly();\n            \n            public Result<InventoryItem> AddInventory(Product product, Quantity quantity, UserId userId)\n            {\n                if (quantity.IsZero())\n                    return Result.Failure<InventoryItem>(\"Quantity must be greater than zero\");\n                    \n                var inventoryItem = InventoryItem.Create(product, quantity, Id, userId);\n                _inventoryItems.Add(inventoryItem);\n                \n                AddDomainEvent(new InventoryAddedEvent(Id, product.Id, quantity));\n                \n                return Result.Success(inventoryItem);\n            }\n        }\n        ```\n\n        ### Application Layer Use Cases\n\n        ```csharp\n        public class TransferInventoryUseCase : ITransferInventoryUseCase\n        {\n            private readonly IWarehouseLocationRepository _locationRepository;\n            private readonly IInventoryTransferService _transferService;\n            private readonly IUnitOfWork _unitOfWork;\n            \n            public async Task<r> ExecuteAsync(TransferInventoryCommand command)\n            {\n                var sourceLocation = await _locationRepository.GetByIdAsync(command.SourceLocationId);\n                var targetLocation = await _locationRepository.GetByIdAsync(command.TargetLocationId);\n                \n                var transferResult = _transferService.TransferInventory(\n                    sourceLocation, \n                    targetLocation, \n                    command.ProductId, \n                    command.Quantity,\n                    command.UserId);\n                    \n                if (transferResult.IsFailure)\n                    return transferResult;\n                    \n                await _unitOfWork.SaveChangesAsync();\n                return Result.Success();\n            }\n        }\n        ```\n\n        ## Real-Time Updates with SignalR\n\n        One of the key requirements was real-time location grid updates for warehouse operators:\n\n        ```csharp\n        [Authorize]\n        public class WarehouseHub : Hub\n        {\n            public async Task JoinWarehouseGroup(string warehouseId)\n            {\n                await Groups.AddToGroupAsync(Context.ConnectionId, $\"warehouse_{warehouseId}\");\n            }\n            \n            public async Task NotifyLocationUpdate(string warehouseId, LocationUpdateDto update)\n            {\n                await Clients.Group($\"warehouse_{warehouseId}\")\n                    .SendAsync(\"LocationUpdated\", update);\n            }\n        }\n        ```\n\n        ## Multi-Tenant Architecture\n\n        Ensuring client data isolation was critical:\n\n        ```csharp\n        public class WarehouseLocationRepository : IWarehouseLocationRepository\n        {\n            private readonly ApplicationDbContext _context;\n            private readonly ICurrentUserService _currentUser;\n            \n            public async Task<List<WarehouseLocation>> GetByWarehouseAsync(WarehouseId warehouseId)\n            {\n                var clientId = _currentUser.GetClientId();\n                \n                return await _context.WarehouseLocations\n                    .Where(l => l.WarehouseId == warehouseId && l.ClientId == clientId)\n                    .ToListAsync();\n            }\n        }\n        ```\n\n        ## Performance Optimization\n\n        Strategic database indexing proved crucial for performance:\n\n        ```sql\n        CREATE INDEX IX_WarehouseLocation_ClientId_WarehouseId \n        ON WarehouseLocations (ClientId, WarehouseId);\n\n        CREATE INDEX IX_InventoryItem_LocationId_ProductId \n        ON InventoryItems (LocationId, ProductId) \n        INCLUDE (Quantity, LastUpdated);\n        ```\n\n        ## Key Lessons Learned\n\n        ### 1. Domain Modeling is Critical\n        Spending time upfront to understand the domain and model it correctly pays massive dividends. The domain model becomes the single source of truth for business rules.\n\n        ### 2. Clean Architecture Enables Flexibility\n        When requirements changed, our Clean Architecture made it relatively easy to adapt. Business logic stayed in the domain, and we could modify infrastructure without affecting core functionality.\n\n        ### 3. Real-time Features Need Careful Design\n        SignalR is powerful, but managing connections and state in a multi-tenant environment requires careful consideration of scalability and security.\n\n        ### 4. Performance Matters from Day One\n        Implementing performance monitoring and optimization early saves significant refactoring time later.\n\n        ## Technical Stack\n\n        - **Backend**: ASP.NET Core 8.0, Entity Framework Core\n        - **Architecture**: Clean Architecture, Domain-Driven Design\n        - **Real-time**: SignalR for live updates\n        - **Database**: PostgreSQL with strategic indexing\n        - **Cloud**: AWS (EC2, RDS, S3)\n        - **Frontend**: Tailwind CSS, JavaScript\n\n        ## Results\n\n        The system now successfully handles:\n        - Multiple warehouse locations with unified operations\n        - Real-time inventory tracking across the network\n        - Multi-client data isolation and security\n        - Scalable architecture ready for expansion\n\n        ## Conclusion\n\n        This project reinforced my belief that good architecture is not just about following patterns—it's about solving real business problems while building systems that can evolve with changing requirements.\n\n        The experience has prepared me well for tackling similar challenges at global technology companies, where scalable architecture and clean code are essential for success.\n\n        *Follow my blog for more insights about enterprise software development and my journey toward global technology companies.*");

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Challenges", "Description", "EndDate", "GitHubUrl", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Enterprise Software", "Domain modeling for warehouse operations using DDD principles, implementing Clean Architecture in enterprise client environment, complex inventory and invoice domain logic, tight MVP delivery timeline with proper architectural foundation", "Enterprise warehouse operations management system for international manufacturing client with inventory and invoice modules", new DateTime(2024, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Clean Architecture implementation in enterprise projects, Domain-Driven Design tactical patterns, leading development teams using modern architectural approaches, client stakeholder management, importance of architectural decisions in project success", "Technical lead for developing a comprehensive warehouse operations management system for a global manufacturing client. The system handles warehouse job operations, packaging material inventory management, and invoice processing. Built using layered architecture principles with a team of 3 engineers, delivering significant operational efficiency improvements for a client transitioning from manual processes to digital warehouse management.", "Global Manufacturing Client Web Application", "Applied Clean Architecture principles with clear domain/application/infrastructure separation, designed domain models for warehouse operations using DDD tactical patterns, implemented repository and unit of work patterns, created comprehensive API layer with proper dependency inversion, deployed on AWS Lightsail with containerized approach", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core 7.0, PostgreSQL, Entity Framework Core, Clean Architecture, Domain-Driven Design, Bootstrap, AWS (S3, EC2, RDS), REST APIs, JavaScript" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Category", "Challenges", "Description", "EndDate", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Integration Platform", "Multiple client EDI format variations, reliable SFTP monitoring and file processing, error handling and notification systems, automated workflow management, maintaining system reliability for critical logistics operations", "Automated EDI file processing system with SFTP monitoring, API integrations, and multi-client support for logistics operations", new DateTime(2023, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Node.js for enterprise file processing, SFTP protocol implementation, building reliable automated systems, multi-client platform architecture, importance of comprehensive error handling in logistics systems", "Developed a comprehensive EDI processing platform serving multiple logistics clients with automated file processing, SFTP monitoring, and API integrations. The system includes SFTP folder watcher programs, EDI file processing engines, XML generation APIs, and automated notification systems. Handled diverse client requirements with custom processing logic for each client while maintaining a unified platform architecture.", "Multi-Client EDI Processing & Integration Platform", "Built robust SFTP folder watcher with Node.js, implemented flexible EDI parsing engine supporting multiple formats, created comprehensive error handling with email and Teams notifications, designed modular architecture for easy client onboarding, automated file processing with proper logging and monitoring", new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Node.js, SFTP protocols, XML processing, Email APIs, Microsoft Teams integration, Task Scheduling, File System monitoring" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Challenges", "Description", "EndDate", "IsFeatured", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Document Processing & Logistics", "Accurate OCR data extraction from varying document formats, automated file processing reliability, system deployment", "Python-based OCR system for manifest processing", new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), false, "Python OCR implementation for production systems, automated document processing workflows.", "Developed an intelligent document processing system using Python OCR to extract data from manifest files. The OCR system automatically processes uploaded documents via task scheduler, extracts relevant data, consolidates information, and saves to database.", "OCR Document Processing & Logistics Container System", "Implemented robust Python OCR processing with error handling, designed efficient data consolidation algorithms.", new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Python, OCR libraries, Task Scheduler, SQL Server, File processing" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Challenges", "Description", "EndDate", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Manufacturing Systems", "Complex manufacturing workflow integration, high-volume transaction processing, ERP system integration, custom reporting requirements, user training and system adoption in manufacturing environment", "Comprehensive warehouse management system with desktop applications for electronics manufacturing operations", new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Desktop application development for manufacturing, database design for high-volume operations, ERP system integration patterns, importance of user training in system adoption, manufacturing industry requirements and workflows", "Contributed to developing a complete warehouse management system from scratch for electronics manufacturing operations. The system included desktop applications for warehouse operations, production tracking, finished goods management, and shipment processing. Handled high-volume manufacturing workflows with integration to existing ERP systems and custom label printing solutions.", "Electronics Manufacturing WMS & Desktop Applications", "Built robust desktop applications using C# Windows Forms, implemented efficient database design with Entity Framework, created seamless ERP integrations, developed comprehensive reporting system, provided extensive user training and support", new DateTime(2018, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C# Windows Forms, Entity Framework, SQL Server, Web Services, Crystal Reports, Manufacturing integrations" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Challenges", "Description", "EndDate", "LessonsLearned", "LiveUrl", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Web Development", "Professional design and user experience, responsive layout across devices, SEO optimization, content management system design, performance optimization, modern web development practices", "Professional portfolio website showcasing software engineering expertise and career journey", new DateTime(2025, 6, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Modern web development practices, responsive design principles, SEO optimization techniques, personal branding through technology, importance of professional online presence in career development", "https://ariftan.dev", "Designed and developed a comprehensive personal portfolio website to showcase software engineering expertise, project portfolio, and professional journey. The website features responsive design, modern UI/UX, project galleries, blog functionality, and contact forms. Built with focus on performance, SEO optimization, and professional presentation for career advancement opportunities.", "Personal Portfolio Website", "Implemented clean and modern design with Bootstrap 5, created responsive layouts for all device sizes, optimized for search engines with proper meta tags and structured data, built custom CMS functionality with Entity Framework, optimized loading performance and user experience", new DateTime(2025, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core 8.0, Razor Pages, Bootstrap 5, Entity Framework Core, SQLite, HTML/CSS, JavaScript" });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7,
                column: "IsVisible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8,
                column: "IsVisible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10,
                column: "IsVisible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11,
                column: "IsVisible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12,
                column: "IsVisible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 13,
                column: "IsVisible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 14,
                column: "IsVisible",
                value: true);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "devicon-amazonwebservices-plain", "AWS", 8, 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "devicon-git-plain", "Git", 9, 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "devicon-docker-plain", "Docker", 7, 3 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "fas fa-bolt", "Power Automate", 8, 4 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "devicon-amazonwebservices-plain", true, "AWS S3", 5 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "devicon-amazonwebservices-plain", true, "AWS EC2", 6 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "devicon-amazonwebservices-plain", true, "AWS Lightsail", 7 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 23,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Cloud & DevOps", "devicon-azure-plain", "Azure", 7, 8 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 24,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Cloud & DevOps", "devicon-azure-plain", "Azure DevOps", 7, 9 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 25,
                columns: new[] { "Icon", "Name", "SortOrder" },
                values: new object[] { "devicon-microsoftsqlserver-plain", "SQL Server", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 26,
                columns: new[] { "Icon", "Name", "SortOrder" },
                values: new object[] { "devicon-postgresql-plain", "PostgreSQL", 2 });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Category", "Description", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[,]
                {
                    { 27, "Databases", null, "devicon-dot-net-plain", true, "Entity Framework Core", 9, 3 },
                    { 28, "Databases", null, "devicon-sqlite-plain", true, "SQLite", 9, 4 }
                });
        }
    }
}
