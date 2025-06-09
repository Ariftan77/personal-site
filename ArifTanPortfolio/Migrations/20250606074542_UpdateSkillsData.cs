using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ArifTanPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSkillsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Programming Languages", "devicon-javascript-plain", "JavaScript", 8, 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "devicon-python-plain", "Python", 7, 3 });

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
                columns: new[] { "Category", "Icon", "Name", "SortOrder" },
                values: new object[] { "Web Frameworks", "devicon-dot-net-plain", "ASP.NET MVC", 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Web Frameworks", "devicon-dot-net-plain", "Razor Pages", 9, 3 });

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
                columns: new[] { "Category", "Description", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Web Frameworks", "Maintenance experience", "devicon-react-original", "React", 6, 5 });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Category", "Description", "Icon", "IsVisible", "Name", "Proficiency", "SortOrder" },
                values: new object[,]
                {
                    { 9, "Web Frameworks", null, "devicon-dot-net-plain", true, "SignalR", 8, 6 },
                    { 10, "Frontend", null, "devicon-jquery-plain", true, "jQuery", 8, 1 },
                    { 11, "Frontend", null, "devicon-javascript-plain", true, "AJAX", 8, 2 },
                    { 12, "Frontend", null, "devicon-bootstrap-plain", true, "Bootstrap", 8, 3 },
                    { 13, "Frontend", null, "devicon-html5-plain", true, "HTML/CSS", 8, 4 },
                    { 14, "Databases", null, "devicon-microsoftsqlserver-plain", true, "SQL Server", 8, 1 },
                    { 15, "Databases", null, "devicon-postgresql-plain", true, "PostgreSQL", 7, 2 },
                    { 16, "Databases", null, "devicon-dot-net-plain", true, "Entity Framework Core", 9, 3 },
                    { 17, "Databases", null, "devicon-sqlite-plain", true, "SQLite", 7, 4 },
                    { 18, "Cloud Platforms", null, "devicon-azure-plain", true, "Azure", 7, 1 },
                    { 19, "Cloud Platforms", null, "fas fa-bolt", true, "Power Automate", 7, 2 },
                    { 20, "DevOps", null, "devicon-docker-plain", true, "Docker", 7, 1 },
                    { 21, "DevOps", null, "devicon-git-plain", true, "Git", 8, 2 },
                    { 22, "DevOps", null, "devicon-azure-plain", true, "Azure DevOps", 7, 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Web Frameworks", "devicon-dot-net-plain", "ASP.NET Core", 9, 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "devicon-javascript-plain", "JavaScript", 8, 2 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Programming Languages", "devicon-python-plain", "Python", 7, 3 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Category", "Icon", "Name", "SortOrder" },
                values: new object[] { "Databases", "devicon-microsoftsqlserver-plain", "SQL Server", 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Cloud Platforms", "devicon-azure-plain", "Azure", 7, 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "Category", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "DevOps", "devicon-docker-plain", "Docker", 7, 1 });

            migrationBuilder.UpdateData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "Category", "Description", "Icon", "Name", "Proficiency", "SortOrder" },
                values: new object[] { "Version Control", null, "devicon-git-plain", "Git", 8, 1 });
        }
    }
}
