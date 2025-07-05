using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MasterDetails.API.Migrations
{
    /// <inheritdoc />
    public partial class BlogAndVideoToDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    BlogID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CoverImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Tags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.BlogID);
                });

            migrationBuilder.CreateTable(
                name: "BlogVideos",
                columns: table => new
                {
                    BlogVideoID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BlogID = table.Column<int>(type: "int", nullable: false),
                    VideoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    DisplayOrder = table.Column<int>(type: "int", nullable: true),
                    Caption = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogVideos", x => x.BlogVideoID);
                    table.ForeignKey(
                        name: "FK_BlogVideos_Blogs_BlogID",
                        column: x => x.BlogID,
                        principalTable: "Blogs",
                        principalColumn: "BlogID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BlogVideos_BlogID",
                table: "BlogVideos",
                column: "BlogID");


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogVideos");

            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
