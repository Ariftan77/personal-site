﻿// <auto-generated />
using System;
using ArifTanPortfolio.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ArifTanPortfolio.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20250617162440_updateProjectSeed")]
    partial class updateProjectSeed
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.5");

            modelBuilder.Entity("ArifTanPortfolio.Models.BlogPost", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("FeaturedImage")
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("LastModified")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("PublishedDate")
                        .HasColumnType("TEXT");

                    b.Property<int>("ReadTimeMinutes")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Summary")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("TEXT");

                    b.Property<string>("Tags")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PublishedDate");

                    b.HasIndex("Slug")
                        .IsUnique();

                    b.ToTable("BlogPosts");
                });

            modelBuilder.Entity("ArifTanPortfolio.Models.ContactMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Company")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateSent")
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsRead")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Subject")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("DateSent");

                    b.ToTable("ContactMessages");
                });

            modelBuilder.Entity("ArifTanPortfolio.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Challenges")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("FeaturedImage")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("GitHubUrl")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("ImageGallery")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsFeatured")
                        .HasColumnType("INTEGER");

                    b.Property<string>("LessonsLearned")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("LiveUrl")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("LongDescription")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<string>("Solutions")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SortOrder")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Technologies")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Category");

                    b.HasIndex("SortOrder");

                    b.ToTable("Projects");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Category = "Enterprise Software",
                            Challenges = "Multi-tenant domain modeling with DDD bounded contexts, real-time location synchronization across warehouse networks, implementing Clean Architecture in complex business domain, CQRS pattern implementation for read/write separation, performance optimization for concurrent warehouse operations",
                            Description = "Comprehensive WMS with real-time location tracking, multi-client inventory management, and cross-warehouse operations",
                            ImageGallery = "",
                            IsFeatured = true,
                            LessonsLearned = "Domain-Driven Design for complex business logic, Clean Architecture implementation in enterprise systems, CQRS patterns for scalable applications, multi-tenant bounded context design, advanced SignalR for real-time systems, AWS cloud architecture integration",
                            LongDescription = "Led development of an enterprise-scale warehouse management system serving multiple warehouse locations with real-time inventory tracking and location-based operations. The system supports multi-client operations with separate inventory management per client while maintaining unified warehouse operations. Features real-time dashboard with SignalR for location grid updates, comprehensive reporting, and scalable architecture designed for warehouse network expansion.",
                            Name = "Multi-Warehouse Management System",
                            Solutions = "Implemented SignalR for real-time location grid dashboard, designed multi-tenant domain architecture with client isolation using DDD bounded contexts, applied Clean Architecture with separated domain/application/infrastructure layers, optimized database queries with strategic indexing, created robust API layer following CQRS patterns, implemented comprehensive logging and monitoring with AWS services",
                            SortOrder = 1,
                            StartDate = new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Technologies = "ASP.NET Core 8.0, PostgreSQL, SignalR, Entity Framework Core, Clean Architecture, Domain-Driven Design, Tailwind CSS, AWS (S3, EC2, RDS), REST APIs, JavaScript"
                        },
                        new
                        {
                            Id = 2,
                            Category = "Enterprise Software",
                            Challenges = "Domain modeling for warehouse operations using DDD principles, implementing Clean Architecture in enterprise client environment, complex inventory and invoice domain logic, tight MVP delivery timeline with proper architectural foundation",
                            Description = "Enterprise warehouse operations management system for international manufacturing client with inventory and invoice modules",
                            EndDate = new DateTime(2024, 7, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageGallery = "",
                            IsFeatured = true,
                            LessonsLearned = "Clean Architecture implementation in enterprise projects, Domain-Driven Design tactical patterns, leading development teams using modern architectural approaches, client stakeholder management, importance of architectural decisions in project success",
                            LongDescription = "Technical lead for developing a comprehensive warehouse operations management system for a global manufacturing client. The system handles warehouse job operations, packaging material inventory management, and invoice processing. Built using layered architecture principles with a team of 3 engineers, delivering significant operational efficiency improvements for a client transitioning from manual processes to digital warehouse management.",
                            Name = "Global Manufacturing Client Web Application",
                            Solutions = "Applied Clean Architecture principles with clear domain/application/infrastructure separation, designed domain models for warehouse operations using DDD tactical patterns, implemented repository and unit of work patterns, created comprehensive API layer with proper dependency inversion, deployed on AWS Lightsail with containerized approach",
                            SortOrder = 2,
                            StartDate = new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Technologies = "ASP.NET Core 7.0, PostgreSQL, Entity Framework Core, Clean Architecture, Domain-Driven Design, Bootstrap, AWS (S3, EC2, RDS), REST APIs, JavaScript"
                        },
                        new
                        {
                            Id = 3,
                            Category = "Integration Platform",
                            Challenges = "Multiple client EDI format variations, reliable SFTP monitoring and file processing, error handling and notification systems, automated workflow management, maintaining system reliability for critical logistics operations",
                            Description = "Automated EDI file processing system with SFTP monitoring, API integrations, and multi-client support for logistics operations",
                            EndDate = new DateTime(2023, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageGallery = "",
                            IsFeatured = true,
                            LessonsLearned = "Node.js for enterprise file processing, SFTP protocol implementation, building reliable automated systems, multi-client platform architecture, importance of comprehensive error handling in logistics systems",
                            LongDescription = "Developed a comprehensive EDI processing platform serving multiple logistics clients with automated file processing, SFTP monitoring, and API integrations. The system includes SFTP folder watcher programs, EDI file processing engines, XML generation APIs, and automated notification systems. Handled diverse client requirements with custom processing logic for each client while maintaining a unified platform architecture.",
                            Name = "Multi-Client EDI Processing & Integration Platform",
                            Solutions = "Built robust SFTP folder watcher with Node.js, implemented flexible EDI parsing engine supporting multiple formats, created comprehensive error handling with email and Teams notifications, designed modular architecture for easy client onboarding, automated file processing with proper logging and monitoring",
                            SortOrder = 3,
                            StartDate = new DateTime(2023, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Technologies = "Node.js, SFTP protocols, XML processing, Email APIs, Microsoft Teams integration, Task Scheduling, File System monitoring"
                        },
                        new
                        {
                            Id = 4,
                            Category = "Document Processing & Logistics",
                            Challenges = "Accurate OCR data extraction from varying document formats, automated file processing reliability, system deployment",
                            Description = "Python-based OCR system for manifest processing",
                            EndDate = new DateTime(2024, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageGallery = "",
                            IsFeatured = false,
                            LessonsLearned = "Python OCR implementation for production systems, automated document processing workflows.",
                            LongDescription = "Developed an intelligent document processing system using Python OCR to extract data from manifest files. The OCR system automatically processes uploaded documents via task scheduler, extracts relevant data, consolidates information, and saves to database.",
                            Name = "OCR Document Processing & Logistics Container System",
                            Solutions = "Implemented robust Python OCR processing with error handling, designed efficient data consolidation algorithms.",
                            SortOrder = 4,
                            StartDate = new DateTime(2023, 10, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Technologies = "Python, OCR libraries, Task Scheduler, SQL Server, File processing"
                        },
                        new
                        {
                            Id = 5,
                            Category = "Manufacturing Systems",
                            Challenges = "Complex manufacturing workflow integration, high-volume transaction processing, ERP system integration, custom reporting requirements, user training and system adoption in manufacturing environment",
                            Description = "Comprehensive warehouse management system with desktop applications for electronics manufacturing operations",
                            EndDate = new DateTime(2020, 9, 30, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageGallery = "",
                            IsFeatured = false,
                            LessonsLearned = "Desktop application development for manufacturing, database design for high-volume operations, ERP system integration patterns, importance of user training in system adoption, manufacturing industry requirements and workflows",
                            LongDescription = "Contributed to developing a complete warehouse management system from scratch for electronics manufacturing operations. The system included desktop applications for warehouse operations, production tracking, finished goods management, and shipment processing. Handled high-volume manufacturing workflows with integration to existing ERP systems and custom label printing solutions.",
                            Name = "Electronics Manufacturing WMS & Desktop Applications",
                            Solutions = "Built robust desktop applications using C# Windows Forms, implemented efficient database design with Entity Framework, created seamless ERP integrations, developed comprehensive reporting system, provided extensive user training and support",
                            SortOrder = 5,
                            StartDate = new DateTime(2018, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Technologies = "C# Windows Forms, Entity Framework, SQL Server, Web Services, Crystal Reports, Manufacturing integrations"
                        },
                        new
                        {
                            Id = 6,
                            Category = "Web Development",
                            Challenges = "Professional design and user experience, responsive layout across devices, SEO optimization, content management system design, performance optimization, modern web development practices",
                            Description = "Professional portfolio website showcasing software engineering expertise and career journey",
                            EndDate = new DateTime(2024, 12, 31, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            ImageGallery = "",
                            IsFeatured = false,
                            LessonsLearned = "Modern web development practices, responsive design principles, SEO optimization techniques, personal branding through technology, importance of professional online presence in career development",
                            LiveUrl = "https://ariftan.dev",
                            LongDescription = "Designed and developed a comprehensive personal portfolio website to showcase software engineering expertise, project portfolio, and professional journey. The website features responsive design, modern UI/UX, project galleries, blog functionality, and contact forms. Built with focus on performance, SEO optimization, and professional presentation for career advancement opportunities.",
                            Name = "Personal Portfolio Website",
                            Solutions = "Implemented clean and modern design with Bootstrap 5, created responsive layouts for all device sizes, optimized for search engines with proper meta tags and structured data, built custom CMS functionality with Entity Framework, optimized loading performance and user experience",
                            SortOrder = 6,
                            StartDate = new DateTime(2024, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            Technologies = "ASP.NET Core 8.0, Razor Pages, Bootstrap 5, Entity Framework Core, SQLite, HTML/CSS, JavaScript"
                        });
                });

            modelBuilder.Entity("ArifTanPortfolio.Models.Skill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Icon")
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("Proficiency")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SortOrder")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Category");

                    b.HasIndex("SortOrder");

                    b.ToTable("Skills");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Category = "Programming Languages",
                            Icon = "devicon-csharp-plain",
                            IsVisible = true,
                            Name = "C#",
                            Proficiency = 9,
                            SortOrder = 1
                        },
                        new
                        {
                            Id = 2,
                            Category = "Programming Languages",
                            Icon = "devicon-javascript-plain",
                            IsVisible = true,
                            Name = "JavaScript",
                            Proficiency = 8,
                            SortOrder = 2
                        },
                        new
                        {
                            Id = 3,
                            Category = "Programming Languages",
                            Icon = "devicon-python-plain",
                            IsVisible = true,
                            Name = "Python",
                            Proficiency = 8,
                            SortOrder = 3
                        },
                        new
                        {
                            Id = 4,
                            Category = "Programming Languages",
                            Icon = "devicon-dart-plain",
                            IsVisible = true,
                            Name = "Dart",
                            Proficiency = 6,
                            SortOrder = 4
                        },
                        new
                        {
                            Id = 5,
                            Category = "Frameworks",
                            Icon = "devicon-dot-net-plain",
                            IsVisible = true,
                            Name = "ASP.NET Core",
                            Proficiency = 9,
                            SortOrder = 1
                        },
                        new
                        {
                            Id = 6,
                            Category = "Frameworks",
                            Icon = "devicon-nodejs-plain",
                            IsVisible = true,
                            Name = "Node.js",
                            Proficiency = 8,
                            SortOrder = 2
                        },
                        new
                        {
                            Id = 7,
                            Category = "Frameworks",
                            Icon = "devicon-flutter-plain",
                            IsVisible = true,
                            Name = "Flutter",
                            Proficiency = 6,
                            SortOrder = 3
                        },
                        new
                        {
                            Id = 8,
                            Category = "Frameworks",
                            Icon = "devicon-bootstrap-plain",
                            IsVisible = true,
                            Name = "Bootstrap",
                            Proficiency = 8,
                            SortOrder = 4
                        },
                        new
                        {
                            Id = 9,
                            Category = "Frameworks",
                            Icon = "devicon-dot-net-plain",
                            IsVisible = true,
                            Name = "ASP.NET MVC",
                            Proficiency = 9,
                            SortOrder = 5
                        },
                        new
                        {
                            Id = 10,
                            Category = "Frameworks",
                            Icon = "devicon-dot-net-plain",
                            IsVisible = true,
                            Name = "Razor Pages",
                            Proficiency = 8,
                            SortOrder = 6
                        },
                        new
                        {
                            Id = 11,
                            Category = "Frameworks",
                            Description = "Maintenance experience",
                            Icon = "devicon-react-original",
                            IsVisible = true,
                            Name = "React",
                            Proficiency = 6,
                            SortOrder = 7
                        },
                        new
                        {
                            Id = 12,
                            Category = "Frameworks",
                            Icon = "devicon-dot-net-plain",
                            IsVisible = true,
                            Name = "SignalR",
                            Proficiency = 8,
                            SortOrder = 8
                        },
                        new
                        {
                            Id = 13,
                            Category = "Frameworks",
                            Icon = "devicon-jquery-plain",
                            IsVisible = true,
                            Name = "jQuery",
                            Proficiency = 8,
                            SortOrder = 9
                        },
                        new
                        {
                            Id = 14,
                            Category = "Frameworks",
                            Icon = "devicon-html5-plain",
                            IsVisible = true,
                            Name = "HTML/CSS",
                            Proficiency = 8,
                            SortOrder = 10
                        },
                        new
                        {
                            Id = 15,
                            Category = "Frameworks",
                            Icon = "devicon-javascript-plain",
                            IsVisible = true,
                            Name = "AJAX",
                            Proficiency = 8,
                            SortOrder = 11
                        },
                        new
                        {
                            Id = 16,
                            Category = "Cloud & DevOps",
                            Icon = "devicon-amazonwebservices-plain",
                            IsVisible = true,
                            Name = "AWS",
                            Proficiency = 8,
                            SortOrder = 1
                        },
                        new
                        {
                            Id = 17,
                            Category = "Cloud & DevOps",
                            Icon = "devicon-git-plain",
                            IsVisible = true,
                            Name = "Git",
                            Proficiency = 9,
                            SortOrder = 2
                        },
                        new
                        {
                            Id = 18,
                            Category = "Cloud & DevOps",
                            Icon = "devicon-docker-plain",
                            IsVisible = true,
                            Name = "Docker",
                            Proficiency = 7,
                            SortOrder = 3
                        },
                        new
                        {
                            Id = 19,
                            Category = "Cloud & DevOps",
                            Icon = "fas fa-bolt",
                            IsVisible = true,
                            Name = "Power Automate",
                            Proficiency = 8,
                            SortOrder = 4
                        },
                        new
                        {
                            Id = 20,
                            Category = "Cloud & DevOps",
                            Icon = "devicon-amazonwebservices-plain",
                            IsVisible = true,
                            Name = "AWS S3",
                            Proficiency = 8,
                            SortOrder = 5
                        },
                        new
                        {
                            Id = 21,
                            Category = "Cloud & DevOps",
                            Icon = "devicon-amazonwebservices-plain",
                            IsVisible = true,
                            Name = "AWS EC2",
                            Proficiency = 7,
                            SortOrder = 6
                        },
                        new
                        {
                            Id = 22,
                            Category = "Cloud & DevOps",
                            Icon = "devicon-amazonwebservices-plain",
                            IsVisible = true,
                            Name = "AWS Lightsail",
                            Proficiency = 7,
                            SortOrder = 7
                        },
                        new
                        {
                            Id = 23,
                            Category = "Cloud & DevOps",
                            Icon = "devicon-azure-plain",
                            IsVisible = true,
                            Name = "Azure",
                            Proficiency = 7,
                            SortOrder = 8
                        },
                        new
                        {
                            Id = 24,
                            Category = "Cloud & DevOps",
                            Icon = "devicon-azure-plain",
                            IsVisible = true,
                            Name = "Azure DevOps",
                            Proficiency = 7,
                            SortOrder = 9
                        },
                        new
                        {
                            Id = 25,
                            Category = "Databases",
                            Icon = "devicon-microsoftsqlserver-plain",
                            IsVisible = true,
                            Name = "SQL Server",
                            Proficiency = 9,
                            SortOrder = 1
                        },
                        new
                        {
                            Id = 26,
                            Category = "Databases",
                            Icon = "devicon-postgresql-plain",
                            IsVisible = true,
                            Name = "PostgreSQL",
                            Proficiency = 9,
                            SortOrder = 2
                        },
                        new
                        {
                            Id = 27,
                            Category = "Databases",
                            Icon = "devicon-dot-net-plain",
                            IsVisible = true,
                            Name = "Entity Framework Core",
                            Proficiency = 9,
                            SortOrder = 3
                        },
                        new
                        {
                            Id = 28,
                            Category = "Databases",
                            Icon = "devicon-sqlite-plain",
                            IsVisible = true,
                            Name = "SQLite",
                            Proficiency = 9,
                            SortOrder = 4
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
