using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArifTanPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class update_skills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                column: "Technologies",
                value: "ASP.NET Core, PostgreSQL, SignalR, Entity Framework Core, AWS, JavaScript, Tailwind");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Programming Languages", "devicon-dart-plain", "Dart", 6, 4 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "Frameworks", true, "ASP.NET Core", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Frameworks", "devicon-nodejs-plain", true, "Node.js", 8, 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Frameworks", "devicon-flutter-plain", "Flutter", 6, 3 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Description", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Frameworks", null, "devicon-bootstrap-plain", true, "Bootstrap", 8, 4 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Frameworks", true, "ASP.NET MVC", 9, 5 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "Frameworks", "devicon-dot-net-plain", true, "Razor Pages", 6 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Category", "Description", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Frameworks", "Maintenance experience", "devicon-react-original", true, "React", 6, 7 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "Frameworks", "devicon-dot-net-plain", true, "SignalR", 8 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "Frameworks", "devicon-jquery-plain", true, "jQuery", 9 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Frameworks", "devicon-html5-plain", "HTML/CSS", 8, 10 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Frameworks", "devicon-javascript-plain", "AJAX", 8, 11 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Cloud & DevOps", "devicon-amazonwebservices-plain", true, "AWS", 8, 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "Cloud & DevOps", "devicon-git-plain", true, "Git", 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Cloud & DevOps", "devicon-docker-plain", true, "Docker", 7, 3 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Category", "IsVisible", "Proficiency", "SortOrder" },
                values: new object[] { "Cloud & DevOps", true, 8, 4 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Cloud & DevOps", "devicon-amazonwebservices-plain", true, "AWS S3", 8, 5 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Cloud & DevOps", "devicon-amazonwebservices-plain", "AWS EC2", 7, 6 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "Cloud & DevOps", "devicon-amazonwebservices-plain", true, "AWS Lightsail", 7 });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Category", "Description", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[,]
                {
                    { 23, "Cloud & DevOps", null, "devicon-azure-plain", true, "Azure", 7, 8 },
                    { 24, "Cloud & DevOps", null, "devicon-azure-plain", true, "Azure DevOps", 7, 9 },
                    { 25, "Databases", null, "devicon-microsoftsqlserver-plain", true, "SQL Server", 9, 1 },
                    { 26, "Databases", null, "devicon-postgresql-plain", true, "PostgreSQL", 9, 2 },
                    { 27, "Databases", null, "devicon-dot-net-plain", true, "Entity Framework Core", 9, 3 },
                    { 28, "Databases", null, "devicon-sqlite-plain", true, "SQLite", 9, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                column: "Technologies",
                value: "ASP.NET Core, Entity Framework Core, SQL Server, SignalR, Azure, JavaScript, Bootstrap");

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Web Frameworks", "devicon-dot-net-plain", "ASP.NET Core", 9, 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "Web Frameworks", false, "ASP.NET MVC", 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Web Frameworks", "devicon-dot-net-plain", false, "Razor Pages", 9, 3 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Web Frameworks", "devicon-nodejs-plain", "Node.js", 8, 4 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Description", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Web Frameworks", "Maintenance experience", "devicon-react-original", false, "React", 6, 5 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 9,
                columns: new[] { "Category", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Web Frameworks", false, "SignalR", 8, 6 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "Frontend", "devicon-jquery-plain", false, "jQuery", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11,
                columns: new[] { "Category", "Description", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Frontend", null, "devicon-javascript-plain", false, "AJAX", 8, 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "Frontend", "devicon-bootstrap-plain", false, "Bootstrap", 3 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 13,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "Frontend", "devicon-html5-plain", false, "HTML/CSS", 4 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 14,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Databases", "devicon-microsoftsqlserver-plain", "SQL Server", 9, 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 15,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Databases", "devicon-postgresql-plain", "PostgreSQL", 9, 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 16,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Databases", "devicon-dot-net-plain", false, "Entity Framework Core", 9, 3 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 17,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "Databases", "devicon-sqlite-plain", false, "SQLite", 4 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 18,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Cloud Platforms", "devicon-azure-plain", false, "Azure", 9, 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 19,
                columns: new[] { "Category", "IsVisible", "Proficiency", "SortOrder" },
                values: new object[] { "Cloud Platforms", false, 9, 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 20,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "DevOps", "devicon-docker-plain", false, "Docker", 7, 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 21,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "DevOps", "devicon-git-plain", "Git", 9, 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 22,
                columns: new[] { "Category", "Icon", "IsVisible", "Name", "SortOrder" },
                values: new object[] { "DevOps", "devicon-azure-plain", false, "Azure DevOps", 3 });
        }
    }
}
