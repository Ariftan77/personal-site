using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArifTanPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class updateProjectList : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Challenges", "Description", "EndDate", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "Real-time inventory synchronization across multiple locations, performance optimization with large datasets, complex business logic implementation, integration with legacy ERP systems, ensuring 99.9% uptime for critical operations", "Comprehensive WMS handling inventory management, order processing, and real-time analytics for logistics operations", new DateTime(2025, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Importance of scalable architecture design, performance optimization strategies for enterprise applications, effective team collaboration in complex projects, domain-driven design principles, real-time system challenges and solutions", "Led development of a full-scale warehouse management system that processes thousands of transactions daily across multiple warehouse locations. The system handles complex inventory tracking, order fulfillment workflows, and provides real-time analytics for operational decision-making. Built with scalable architecture to support business growth and integration with existing ERP systems.", "Enterprise Warehouse Management System", "Implemented SignalR for real-time updates, optimized database queries with strategic indexing, applied clean architecture with domain-driven design, created robust API layer for integrations, implemented comprehensive logging and monitoring", new DateTime(2025, 5, 13, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core, Entity Framework Core, SQL Server, SignalR, Azure, JavaScript, Bootstrap" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Projects",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Challenges", "Description", "EndDate", "LessonsLearned", "LongDescription", "Name", "Solutions", "StartDate", "Technologies" },
                values: new object[] { "", "Comprehensive warehouse management system with real-time tracking and analytics", new DateTime(2023, 8, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "", "A full-featured WMS built with ASP.NET Core, handling inventory management, order processing, and real-time analytics for logistics operations.", "Advanced Logistics Platform", "", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ASP.NET Core, Entity Framework, SQL Server, SignalR, Azure" });
        }
    }
}
