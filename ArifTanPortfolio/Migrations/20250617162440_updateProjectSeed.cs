using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArifTanPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class updateProjectSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Challenges", "Description", "EndDate", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Multi-tenant domain modeling with DDD bounded contexts, real-time location synchronization across warehouse networks, implementing Clean Architecture in complex business domain, CQRS pattern implementation for read/write separation, performance optimization for concurrent warehouse operations", "Comprehensive WMS with real-time location tracking, multi-client inventory management, and cross-warehouse operations", null, "Domain-Driven Design for complex business logic, Clean Architecture implementation in enterprise systems, CQRS patterns for scalable applications, multi-tenant bounded context design, advanced SignalR for real-time systems, AWS cloud architecture integration", "Led development of an enterprise-scale warehouse management system serving multiple warehouse locations with real-time inventory tracking and location-based operations. The system supports multi-client operations with separate inventory management per client while maintaining unified warehouse operations. Features real-time dashboard with SignalR for location grid updates, comprehensive reporting, and scalable architecture designed for warehouse network expansion.", "Multi-Warehouse Management System", "Implemented SignalR for real-time location grid dashboard, designed multi-tenant domain architecture with client isolation using DDD bounded contexts, applied Clean Architecture with separated domain/application/infrastructure layers, optimized database queries with strategic indexing, created robust API layer following CQRS patterns, implemented comprehensive logging and monitoring with AWS services", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core 8.0, PostgreSQL, SignalR, Entity Framework Core, Clean Architecture, Domain-Driven Design, Tailwind CSS, AWS (S3, EC2, RDS), REST APIs, JavaScript" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Challenges", "Description", "EndDate", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Enterprise Software", "Domain modeling for warehouse operations using DDD principles, implementing Clean Architecture in enterprise client environment, complex inventory and invoice domain logic, tight MVP delivery timeline with proper architectural foundation", "Enterprise warehouse operations management system for international manufacturing client with inventory and invoice modules", new DateTime(2024, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Clean Architecture implementation in enterprise projects, Domain-Driven Design tactical patterns, leading development teams using modern architectural approaches, client stakeholder management, importance of architectural decisions in project success", "Technical lead for developing a comprehensive warehouse operations management system for a global manufacturing client. The system handles warehouse job operations, packaging material inventory management, and invoice processing. Built using layered architecture principles with a team of 3 engineers, delivering significant operational efficiency improvements for a client transitioning from manual processes to digital warehouse management.", "Global Manufacturing Client Web Application", "Applied Clean Architecture principles with clear domain/application/infrastructure separation, designed domain models for warehouse operations using DDD tactical patterns, implemented repository and unit of work patterns, created comprehensive API layer with proper dependency inversion, deployed on AWS Lightsail with containerized approach", new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core 7.0, PostgreSQL, Entity Framework Core, Clean Architecture, Domain-Driven Design, Bootstrap, AWS (S3, EC2, RDS), REST APIs, JavaScript" });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Category", "Challenges", "Description", "EndDate", "FeaturedImage", "GitHubUrl", "ImageGallery", "IsFeatured", "LessonsLearned", "LiveUrl", "LongDescription", "Name", "Solutions", "SortOrder", "StartDate", "Technologies" },
                values: new object[,]
                {
                    { 3, "Integration Platform", "Multiple client EDI format variations, reliable SFTP monitoring and file processing, error handling and notification systems, automated workflow management, maintaining system reliability for critical logistics operations", "Automated EDI file processing system with SFTP monitoring, API integrations, and multi-client support for logistics operations", new DateTime(2023, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", true, "Node.js for enterprise file processing, SFTP protocol implementation, building reliable automated systems, multi-client platform architecture, importance of comprehensive error handling in logistics systems", null, "Developed a comprehensive EDI processing platform serving multiple logistics clients with automated file processing, SFTP monitoring, and API integrations. The system includes SFTP folder watcher programs, EDI file processing engines, XML generation APIs, and automated notification systems. Handled diverse client requirements with custom processing logic for each client while maintaining a unified platform architecture.", "Multi-Client EDI Processing & Integration Platform", "Built robust SFTP folder watcher with Node.js, implemented flexible EDI parsing engine supporting multiple formats, created comprehensive error handling with email and Teams notifications, designed modular architecture for easy client onboarding, automated file processing with proper logging and monitoring", 3, new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Node.js, SFTP protocols, XML processing, Email APIs, Microsoft Teams integration, Task Scheduling, File System monitoring" },
                    { 4, "Document Processing & Logistics", "Accurate OCR data extraction from varying document formats, automated file processing reliability, system deployment", "Python-based OCR system for manifest processing", new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", false, "Python OCR implementation for production systems, automated document processing workflows.", null, "Developed an intelligent document processing system using Python OCR to extract data from manifest files. The OCR system automatically processes uploaded documents via task scheduler, extracts relevant data, consolidates information, and saves to database.", "OCR Document Processing & Logistics Container System", "Implemented robust Python OCR processing with error handling, designed efficient data consolidation algorithms.", 4, new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Python, OCR libraries, Task Scheduler, SQL Server, File processing" },
                    { 5, "Manufacturing Systems", "Complex manufacturing workflow integration, high-volume transaction processing, ERP system integration, custom reporting requirements, user training and system adoption in manufacturing environment", "Comprehensive warehouse management system with desktop applications for electronics manufacturing operations", new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", false, "Desktop application development for manufacturing, database design for high-volume operations, ERP system integration patterns, importance of user training in system adoption, manufacturing industry requirements and workflows", null, "Contributed to developing a complete warehouse management system from scratch for electronics manufacturing operations. The system included desktop applications for warehouse operations, production tracking, finished goods management, and shipment processing. Handled high-volume manufacturing workflows with integration to existing ERP systems and custom label printing solutions.", "Electronics Manufacturing WMS & Desktop Applications", "Built robust desktop applications using C# Windows Forms, implemented efficient database design with Entity Framework, created seamless ERP integrations, developed comprehensive reporting system, provided extensive user training and support", 5, new DateTime(2018, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "C# Windows Forms, Entity Framework, SQL Server, Web Services, Crystal Reports, Manufacturing integrations" },
                    { 6, "Web Development", "Professional design and user experience, responsive layout across devices, SEO optimization, content management system design, performance optimization, modern web development practices", "Professional portfolio website showcasing software engineering expertise and career journey", new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", false, "Modern web development practices, responsive design principles, SEO optimization techniques, personal branding through technology, importance of professional online presence in career development", "https://ariftan.dev", "Designed and developed a comprehensive personal portfolio website to showcase software engineering expertise, project portfolio, and professional journey. The website features responsive design, modern UI/UX, project galleries, blog functionality, and contact forms. Built with focus on performance, SEO optimization, and professional presentation for career advancement opportunities.", "Personal Portfolio Website", "Implemented clean and modern design with Bootstrap 5, created responsive layouts for all device sizes, optimized for search engines with proper meta tags and structured data, built custom CMS functionality with Entity Framework, optimized loading performance and user experience", 6, new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core 8.0, Razor Pages, Bootstrap 5, Entity Framework Core, SQLite, HTML/CSS, JavaScript" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Challenges", "Description", "EndDate", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Real-time inventory synchronization across multiple locations, performance optimization with large datasets, complex business logic implementation, integration with legacy ERP systems, ensuring 99.9% uptime for critical operations", "Comprehensive WMS handling inventory management, order processing, and real-time analytics for logistics operations", new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Importance of scalable architecture design, performance optimization strategies for enterprise applications, effective team collaboration in complex projects, domain-driven design principles, real-time system challenges and solutions", "Led development of a full-scale warehouse management system that processes thousands of transactions daily across multiple warehouse locations. The system handles complex inventory tracking, order fulfillment workflows, and provides real-time analytics for operational decision-making. Built with scalable architecture to support business growth and integration with existing ERP systems.", "Enterprise Warehouse Management System", "Implemented SignalR for real-time updates, optimized database queries with strategic indexing, applied clean architecture with domain-driven design, created robust API layer for integrations, implemented comprehensive logging and monitoring", new DateTime(2025, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core, PostgreSQL, SignalR, Entity Framework Core, AWS, JavaScript, Tailwind" });

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Challenges", "Description", "EndDate", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Machine Learning", "", "OCR and document automation system using machine learning", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "Automated document processing system that uses OCR and ML to extract and process information from various document types.", "AI Document Processing", "", new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Python, TensorFlow, Azure Cognitive Services, ASP.NET Core API" });
        }
    }
}
