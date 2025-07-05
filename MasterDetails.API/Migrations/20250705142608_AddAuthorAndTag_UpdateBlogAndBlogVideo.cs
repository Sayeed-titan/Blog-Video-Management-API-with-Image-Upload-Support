using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MasterDetails.API.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorAndTag_UpdateBlogAndBlogVideo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "Author",
                table: "Blogs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Blogs",
                type: "datetime",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AddColumn<int>(
                name: "AuthorID",
                table: "Blogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Authors",
                columns: table => new
                {
                    AuthorID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Bio = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Authors", x => x.AuthorID);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "BlogTags",
                columns: table => new
                {
                    BlogID = table.Column<int>(type: "int", nullable: false),
                    TagID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogTags", x => new { x.BlogID, x.TagID });
                    table.ForeignKey(
                        name: "FK_BlogTags_Blogs_BlogID",
                        column: x => x.BlogID,
                        principalTable: "Blogs",
                        principalColumn: "BlogID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BlogTags_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_AuthorID",
                table: "Blogs",
                column: "AuthorID");

            migrationBuilder.CreateIndex(
                name: "IX_BlogTags_TagID",
                table: "BlogTags",
                column: "TagID");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_Authors_AuthorID",
                table: "Blogs",
                column: "AuthorID",
                principalTable: "Authors",
                principalColumn: "AuthorID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_Authors_AuthorID",
                table: "Blogs");

            migrationBuilder.DropTable(
                name: "Authors");

            migrationBuilder.DropTable(
                name: "BlogTags");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_AuthorID",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "AuthorID",
                table: "Blogs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Blogs",
                type: "datetime",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime");

            migrationBuilder.AddColumn<string>(
                name: "Author",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

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
    }
}
