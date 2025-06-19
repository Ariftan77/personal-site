using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArifTanPortfolio.Migrations
{
    /// <inheritdoc />
    public partial class alterBlogPostTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Summary",
                table: "BlogPosts");

            migrationBuilder.RenameColumn(
                name: "LastModified",
                table: "BlogPosts",
                newName: "CreatedDate");

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDate",
                table: "BlogPosts",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AuthorEmail",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Excerpt",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 160,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetaKeywords",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "BlogPosts",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "BlogPosts",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Author",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "AuthorEmail",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "Excerpt",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "MetaDescription",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "MetaKeywords",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "BlogPosts");

            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "BlogPosts");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "BlogPosts",
                newName: "LastModified");

            migrationBuilder.AlterColumn<string>(
                name: "Tags",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedDate",
                table: "BlogPosts",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Summary",
                table: "BlogPosts",
                type: "TEXT",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }
    }
}
