﻿// <auto-generated />
using System;
using AI2_Backend.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AI2_Backend.Migrations
{
    [DbContext(typeof(AIDbContext))]
    [Migration("20231109201109_AddStatsAndSavedProfilesModels")]
    partial class AddStatsAndSavedProfilesModels
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.12")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AI2_Backend.Entities.Qualification", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Qualifications");
                });

            modelBuilder.Entity("AI2_Backend.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("AI2_Backend.Entities.SavedProfile", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.Property<int>("RecruiterId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.HasIndex("RecruiterId");

                    b.ToTable("SavedProfiles");
                });

            modelBuilder.Entity("AI2_Backend.Entities.Stats", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("CounterDaily")
                        .HasColumnType("int");

                    b.Property<int>("CounterMonthly")
                        .HasColumnType("int");

                    b.Property<int>("EmployeeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EmployeeId");

                    b.ToTable("Stats");
                });

            modelBuilder.Entity("AI2_Backend.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AboutMe")
                        .HasMaxLength(10000)
                        .HasColumnType("text");

                    b.Property<string>("Education")
                        .HasMaxLength(10000)
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("FirstName")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("LastName")
                        .HasMaxLength(255)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<decimal?>("RequiredPayment")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int?>("Voivodeship")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("AI2_Backend.Entities.UserQualification", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("QualificationId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "QualificationId");

                    b.HasIndex("QualificationId");

                    b.ToTable("UserQualifications");
                });

            modelBuilder.Entity("AI2_Backend.Entities.SavedProfile", b =>
                {
                    b.HasOne("AI2_Backend.Entities.User", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AI2_Backend.Entities.User", "Recruiter")
                        .WithMany()
                        .HasForeignKey("RecruiterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");

                    b.Navigation("Recruiter");
                });

            modelBuilder.Entity("AI2_Backend.Entities.Stats", b =>
                {
                    b.HasOne("AI2_Backend.Entities.User", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Employee");
                });

            modelBuilder.Entity("AI2_Backend.Entities.User", b =>
                {
                    b.HasOne("AI2_Backend.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("AI2_Backend.Entities.UserQualification", b =>
                {
                    b.HasOne("AI2_Backend.Entities.Qualification", "Qualification")
                        .WithMany("UserQualifications")
                        .HasForeignKey("QualificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AI2_Backend.Entities.User", "User")
                        .WithMany("UserQualifications")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Qualification");

                    b.Navigation("User");
                });

            modelBuilder.Entity("AI2_Backend.Entities.Qualification", b =>
                {
                    b.Navigation("UserQualifications");
                });

            modelBuilder.Entity("AI2_Backend.Entities.User", b =>
                {
                    b.Navigation("UserQualifications");
                });
#pragma warning restore 612, 618
        }
    }
}
