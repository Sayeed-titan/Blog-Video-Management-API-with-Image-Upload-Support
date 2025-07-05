using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MasterDetails.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Blogs",
                columns: new[] { "BlogID", "Author", "Content", "CoverImageUrl", "CreatedAt", "IsPublished", "Tags", "Title" },
                values: new object[,]
                {
                    { 1, "John Doe", "This blog post covers the new features in C# 10...", "https://example.com/images/csharp10.png", new DateTime(2025, 7, 5, 18, 49, 10, 901, DateTimeKind.Local).AddTicks(156), true, "C#, .NET, Programming", "Introduction to C# 10" },
                    { 2, "Jane Smith", "Learn how to build web applications using ASP.NET Core...", "https://example.com/images/aspnetcore.png", new DateTime(2025, 7, 5, 18, 49, 10, 901, DateTimeKind.Local).AddTicks(171), true, "ASP.NET Core, Web Development", "Getting Started with ASP.NET Core" }
                });

            migrationBuilder.InsertData(
                table: "BlogVideos",
                columns: new[] { "BlogVideoID", "BlogID", "Caption", "DisplayOrder", "VideoUrl" },
                values: new object[,]
                {
                    { 1, 1, "C# 10 New Features Overview", 1, "https://www.youtube.com/embed/dQw4w9WgXcQ" },
                    { 2, 1, "Advanced C# 10 Tips", 2, "https://www.youtube.com/embed/oHg5SJYRHA0" },
                    { 3, 2, "ASP.NET Core Beginner Tutorial", 1, "https://www.youtube.com/embed/abcdefg1234" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BlogVideos",
                keyColumn: "BlogVideoID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BlogVideos",
                keyColumn: "BlogVideoID",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BlogVideos",
                keyColumn: "BlogVideoID",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "BlogID",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Blogs",
                keyColumn: "BlogID",
                keyValue: 2);
        }
    }
}
