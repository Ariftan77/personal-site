using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArifTanPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BlogPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "TEXT", nullable: false),
                    Summary = table.Column<string>(type: "TEXT", maxLength: 300, nullable: false),
                    Slug = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    PublishedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsPublished = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tags = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    FeaturedImage = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    ReadTimeMinutes = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogPosts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactMessages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Subject = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 2000, nullable: false),
                    DateSent = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    Company = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    LongDescription = table.Column<string>(type: "TEXT", nullable: false),
                    Technologies = table.Column<string>(type: "TEXT", nullable: false),
                    LiveUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    GitHubUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    FeaturedImage = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    ImageGallery = table.Column<string>(type: "TEXT", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsFeatured = table.Column<bool>(type: "INTEGER", nullable: false),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Challenges = table.Column<string>(type: "TEXT", nullable: false),
                    Solutions = table.Column<string>(type: "TEXT", nullable: false),
                    LessonsLearned = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Proficiency = table.Column<int>(type: "INTEGER", nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Icon = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    SortOrder = table.Column<int>(type: "INTEGER", nullable: false),
                    IsVisible = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "Id", "Category", "Challenges", "Description", "EndDate", "FeaturedImage", "GitHubUrl", "ImageGallery", "IsFeatured", "LessonsLearned", "LiveUrl", "LongDescription", "Name", "Solutions", "SortOrder", "StartDate", "Technologies" },
                values: new object[,]
                {
                    { 1, "Enterprise Software", "", "Comprehensive warehouse management system with real-time tracking and analytics", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", true, "", null, "A full-featured WMS built with ASP.NET Core, handling inventory management, order processing, and real-time analytics for logistics operations.", "Advanced Logistics Platform", "", 1, new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core, Entity Framework, SQL Server, SignalR, Azure" },
                    { 2, "Machine Learning", "", "OCR and document automation system using machine learning", new DateTime(2023, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, null, "", true, "", null, "Automated document processing system that uses OCR and ML to extract and process information from various document types.", "AI Document Processing", "", 2, new DateTime(2023, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Python, TensorFlow, Azure Cognitive Services, ASP.NET Core API" }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Category", "Description", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[,]
                {
                    { 1, "Programming Languages", null, "devicon-csharp-plain", true, "C#", 9, 1 },
                    { 2, "Web Frameworks", null, "devicon-dot-net-plain", true, "ASP.NET Core", 9, 1 },
                    { 3, "Programming Languages", null, "devicon-javascript-plain", true, "JavaScript", 8, 2 },
                    { 4, "Programming Languages", null, "devicon-python-plain", true, "Python", 7, 3 },
                    { 5, "Databases", null, "devicon-microsoftsqlserver-plain", true, "SQL Server", 8, 1 },
                    { 6, "Cloud Platforms", null, "devicon-azure-plain", true, "Azure", 7, 1 },
                    { 7, "DevOps", null, "devicon-docker-plain", true, "Docker", 7, 1 },
                    { 8, "Version Control", null, "devicon-git-plain", true, "Git", 8, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_PublishedDate",
                table: "BlogPosts",
                column: "PublishedDate");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPosts_Slug",
                table: "BlogPosts",
                column: "Slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessages_DateSent",
                table: "ContactMessages",
                column: "DateSent");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Category",
                table: "Projects",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SortOrder",
                table: "Projects",
                column: "SortOrder");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_Category",
                table: "Skills",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_SortOrder",
                table: "Skills",
                column: "SortOrder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogPosts");

            migrationBuilder.DropTable(
                name: "ContactMessages");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Skills");
        }
    }
}
