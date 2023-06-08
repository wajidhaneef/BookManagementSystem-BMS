﻿// <auto-generated />
using System;
using BookManagementSystem_BMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookManagementSystem_BMS.Migrations
{
    [DbContext(typeof(BMSContext))]
    [Migration("20230608134650_RemoveCoverPageIdBookTBL")]
    partial class RemoveCoverPageIdBookTBL
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BookManagementSystem_BMS.Models.Book", b =>
                {
                    b.Property<int>("BookID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BookID"), 1L, 1);

                    b.Property<string>("BookName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.HasKey("BookID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Book", "BMS");
                });

            modelBuilder.Entity("BookManagementSystem_BMS.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("CategoryID"), 1L, 1);

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryID");

                    b.ToTable("Category", "BMS");
                });

            modelBuilder.Entity("BookManagementSystem_BMS.Models.Chapter", b =>
                {
                    b.Property<int>("ChapterID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ChapterID"), 1L, 1);

                    b.Property<int>("BookID")
                        .HasColumnType("int");

                    b.Property<string>("ChapterName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ChapterID");

                    b.HasIndex("BookID");

                    b.ToTable("Chapter", "BMS");
                });

            modelBuilder.Entity("BookManagementSystem_BMS.Models.CoverPage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<byte[]>("ImageData")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("ImageMimeType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BookId")
                        .IsUnique();

                    b.ToTable("CoverPages");
                });

            modelBuilder.Entity("BookManagementSystem_BMS.Models.Role", b =>
                {
                    b.Property<int>("RoleID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("RoleID"), 1L, 1);

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RoleID");

                    b.ToTable("Role", "BMS");
                });

            modelBuilder.Entity("BookManagementSystem_BMS.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserID"), 1L, 1);

                    b.Property<string>("EmailAddress")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RoleID")
                        .HasColumnType("int");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.HasAlternateKey("EmailAddress")
                        .HasName("EmailAddress");

                    b.HasIndex("RoleID");

                    b.ToTable("User", "BMS");
                });

            modelBuilder.Entity("CategoryRole", b =>
                {
                    b.Property<int>("CategoriesCategoryID")
                        .HasColumnType("int");

                    b.Property<int>("RolesRoleID")
                        .HasColumnType("int");

                    b.HasKey("CategoriesCategoryID", "RolesRoleID");

                    b.HasIndex("RolesRoleID");

                    b.ToTable("Roles_Categories", "BMS");
                });

            modelBuilder.Entity("BookManagementSystem_BMS.Models.Book", b =>
                {
                    b.HasOne("BookManagementSystem_BMS.Models.Category", "Category")
                        .WithMany("Books")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BookManagementSystem_BMS.Models.Chapter", b =>
                {
                    b.HasOne("BookManagementSystem_BMS.Models.Book", "Book")
                        .WithMany("Chapters")
                        .HasForeignKey("BookID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("BookManagementSystem_BMS.Models.CoverPage", b =>
                {
                    b.HasOne("BookManagementSystem_BMS.Models.Book", "Book")
                        .WithOne()
                        .HasForeignKey("BookManagementSystem_BMS.Models.CoverPage", "BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("BookManagementSystem_BMS.Models.User", b =>
                {
                    b.HasOne("BookManagementSystem_BMS.Models.Role", "Role")
                        .WithMany("Users")
                        .HasForeignKey("RoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("CategoryRole", b =>
                {
                    b.HasOne("BookManagementSystem_BMS.Models.Category", null)
                        .WithMany()
                        .HasForeignKey("CategoriesCategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BookManagementSystem_BMS.Models.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesRoleID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("BookManagementSystem_BMS.Models.Book", b =>
                {
                    b.Navigation("Chapters");
                });

            modelBuilder.Entity("BookManagementSystem_BMS.Models.Category", b =>
                {
                    b.Navigation("Books");
                });

            modelBuilder.Entity("BookManagementSystem_BMS.Models.Role", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
