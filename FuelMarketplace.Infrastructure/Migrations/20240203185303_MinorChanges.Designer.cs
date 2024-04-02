﻿// <auto-generated />
using System;
using FuelMarketplace.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FuelMarketplace.Infrastructure.Migrations
{
    [DbContext(typeof(MarketplaceContext))]
    [Migration("20240203185303_MinorChanges")]
    partial class MinorChanges
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.13")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FuelMarketplace.Domain.Models.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Latitude")
                        .HasColumnType("float");

                    b.Property<double>("Longitude")
                        .HasColumnType("float");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("Voivodeship")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Address");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<Guid>("FileGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int?>("OfferId")
                        .HasColumnType("int");

                    b.Property<int?>("PostCommentId")
                        .HasColumnType("int");

                    b.Property<int?>("PostId")
                        .HasColumnType("int");

                    b.Property<int?>("SalesPointId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OfferId");

                    b.HasIndex("PostCommentId");

                    b.HasIndex("PostId");

                    b.HasIndex("SalesPointId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.Offer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FuelType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10, 2)");

                    b.Property<int?>("SalesPointId")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SalesPointId");

                    b.HasIndex("UserId");

                    b.ToTable("Offers");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.OfferComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("OfferId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OfferId");

                    b.HasIndex("UserId");

                    b.ToTable("OfferComments");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FuelType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.PostComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("Created")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(10000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Updated")
                        .HasColumnType("datetime2");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("PostId");

                    b.HasIndex("UserId");

                    b.ToTable("PostComments");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.SalesPoint", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("nvarchar(1000)");

                    b.Property<int?>("ProfileImageId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProfileImageId");

                    b.HasIndex("UserId");

                    b.ToTable("SalesPoints");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("varchar(20)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(2000)
                        .HasColumnType("nvarchar(2000)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("varchar(320)");

                    b.Property<bool>("IsBanned")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("varbinary(64)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("varbinary(128)");

                    b.Property<int?>("ProfileImageId")
                        .HasColumnType("int");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ProfileImageId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.Image", b =>
                {
                    b.HasOne("FuelMarketplace.Domain.Models.Offer", null)
                        .WithMany("Images")
                        .HasForeignKey("OfferId");

                    b.HasOne("FuelMarketplace.Domain.Models.PostComment", null)
                        .WithMany("Images")
                        .HasForeignKey("PostCommentId");

                    b.HasOne("FuelMarketplace.Domain.Models.Post", null)
                        .WithMany("Images")
                        .HasForeignKey("PostId");

                    b.HasOne("FuelMarketplace.Domain.Models.SalesPoint", null)
                        .WithMany("Images")
                        .HasForeignKey("SalesPointId");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.Offer", b =>
                {
                    b.HasOne("FuelMarketplace.Domain.Models.SalesPoint", "SalesPoint")
                        .WithMany("Offers")
                        .HasForeignKey("SalesPointId")
                        .OnDelete(DeleteBehavior.NoAction);

                    b.HasOne("FuelMarketplace.Domain.Models.User", "User")
                        .WithMany("Offers")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SalesPoint");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.OfferComment", b =>
                {
                    b.HasOne("FuelMarketplace.Domain.Models.Offer", "Offer")
                        .WithMany("Comments")
                        .HasForeignKey("OfferId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("FuelMarketplace.Domain.Models.User", "User")
                        .WithMany("OfferComments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Offer");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.Post", b =>
                {
                    b.HasOne("FuelMarketplace.Domain.Models.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.PostComment", b =>
                {
                    b.HasOne("FuelMarketplace.Domain.Models.Post", "Post")
                        .WithMany("Comments")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("FuelMarketplace.Domain.Models.User", "User")
                        .WithMany("PostComments")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("Post");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.SalesPoint", b =>
                {
                    b.HasOne("FuelMarketplace.Domain.Models.Image", "ProfileImage")
                        .WithMany()
                        .HasForeignKey("ProfileImageId");

                    b.HasOne("FuelMarketplace.Domain.Models.User", "User")
                        .WithMany("SalesPoints")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ProfileImage");

                    b.Navigation("User");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.User", b =>
                {
                    b.HasOne("FuelMarketplace.Domain.Models.Image", "ProfileImage")
                        .WithMany()
                        .HasForeignKey("ProfileImageId");

                    b.Navigation("ProfileImage");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.Offer", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.Post", b =>
                {
                    b.Navigation("Comments");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.PostComment", b =>
                {
                    b.Navigation("Images");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.SalesPoint", b =>
                {
                    b.Navigation("Images");

                    b.Navigation("Offers");
                });

            modelBuilder.Entity("FuelMarketplace.Domain.Models.User", b =>
                {
                    b.Navigation("OfferComments");

                    b.Navigation("Offers");

                    b.Navigation("PostComments");

                    b.Navigation("Posts");

                    b.Navigation("SalesPoints");
                });
#pragma warning restore 612, 618
        }
    }
}
