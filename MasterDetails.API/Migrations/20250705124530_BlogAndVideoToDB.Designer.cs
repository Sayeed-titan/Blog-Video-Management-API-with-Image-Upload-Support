﻿// <auto-generated />
using System;
using MasterDetails.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MasterDetails.API.Migrations
{
    [DbContext(typeof(BlogDbContext))]
    [Migration("20250705124530_BlogAndVideoToDB")]
    partial class BlogAndVideoToDB
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.17")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MasterDetails.API.Entities.Blog", b =>
                {
                    b.Property<int>("BlogID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BlogID"));

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CoverImageUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("GETDATE()");

                    b.Property<bool>("IsPublished")
                        .HasColumnType("bit");

                    b.Property<string>("Tags")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("BlogID");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("MasterDetails.API.Entities.BlogVideo", b =>
                {
                    b.Property<int>("BlogVideoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasColumnName("BlogVideoID");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BlogVideoID"));

                    b.Property<int>("BlogID")
                        .HasColumnType("int")
                        .HasColumnName("BlogID");

                    b.Property<string>("Caption")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int?>("DisplayOrder")
                        .HasColumnType("int");

                    b.Property<string>("VideoUrl")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.HasKey("BlogVideoID");

                    b.HasIndex("BlogID");

                    b.ToTable("BlogVideos");
                });

            modelBuilder.Entity("MasterDetails.API.Entities.BlogVideo", b =>
                {
                    b.HasOne("MasterDetails.API.Entities.Blog", "Blog")
                        .WithMany("BlogVideos")
                        .HasForeignKey("BlogID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");
                });

            modelBuilder.Entity("MasterDetails.API.Entities.Blog", b =>
                {
                    b.Navigation("BlogVideos");
                });
#pragma warning restore 612, 618
        }
    }
}
