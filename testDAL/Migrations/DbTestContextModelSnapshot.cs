﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using testDAL;

namespace testDAL.Migrations
{
    [DbContext(typeof(DbTestContext))]
    partial class DbTestContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("DTOs.Models.BookingAction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("BookingBegine")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("BookingFinish")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("KeyObjectId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("KeyObjectId");

                    b.HasIndex("UserId");

                    b.ToTable("BookingActions");
                });

            modelBuilder.Entity("DTOs.Models.KeyObject", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("AudNum")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsBooked")
                        .HasColumnType("bit");

                    b.Property<Guid?>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("KeyObjects");
                });

            modelBuilder.Entity("DTOs.Models.Permission", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid?>("KeyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("KeyId")
                        .IsUnique()
                        .HasFilter("[KeyId] IS NOT NULL");

                    b.ToTable("Permissions");
                });

            modelBuilder.Entity("DTOs.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MiddleName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("PermissionUser", b =>
                {
                    b.Property<Guid>("PermissionsId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("UsersWithPermissionsId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PermissionsId", "UsersWithPermissionsId");

                    b.HasIndex("UsersWithPermissionsId");

                    b.ToTable("PermissionUser");
                });

            modelBuilder.Entity("DTOs.Models.BookingAction", b =>
                {
                    b.HasOne("DTOs.Models.KeyObject", "KeyObject")
                        .WithMany()
                        .HasForeignKey("KeyObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DTOs.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("KeyObject");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DTOs.Models.KeyObject", b =>
                {
                    b.HasOne("DTOs.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DTOs.Models.Permission", b =>
                {
                    b.HasOne("DTOs.Models.KeyObject", "Key")
                        .WithOne("Permission")
                        .HasForeignKey("DTOs.Models.Permission", "KeyId");

                    b.Navigation("Key");
                });

            modelBuilder.Entity("PermissionUser", b =>
                {
                    b.HasOne("DTOs.Models.Permission", null)
                        .WithMany()
                        .HasForeignKey("PermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DTOs.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersWithPermissionsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("DTOs.Models.KeyObject", b =>
                {
                    b.Navigation("Permission");
                });
#pragma warning restore 612, 618
        }
    }
}
