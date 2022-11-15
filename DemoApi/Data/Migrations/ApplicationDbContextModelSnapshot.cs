﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Stio.WorkflowManager.DemoApi.Data;

#nullable disable

namespace DemoApi.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Stio.WorkflowManager.DemoApi.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("aa9afdaf-2c5d-4ca6-81a1-64d98cc56878")
                        });
                });

            modelBuilder.Entity("Stio.WorkflowManager.DemoApi.Data.Entities.Workflow", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Workflows");
                });

            modelBuilder.Entity("Stio.WorkflowManager.DemoApi.Data.Entities.WorkflowStep", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Data")
                        .HasColumnType("text");

                    b.Property<bool>("IsSoftDelete")
                        .HasColumnType("boolean");

                    b.Property<string>("Payload")
                        .HasColumnType("text");

                    b.Property<string>("PreviousStepKey")
                        .HasColumnType("text");

                    b.Property<string>("StepKey")
                        .HasColumnType("text");

                    b.Property<Guid>("WorkflowId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("WorkflowId");

                    b.ToTable("WorkflowSteps");
                });

            modelBuilder.Entity("Stio.WorkflowManager.DemoApi.Data.Entities.Workflow", b =>
                {
                    b.HasOne("Stio.WorkflowManager.DemoApi.Data.Entities.User", "User")
                        .WithMany("Workflows")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Stio.WorkflowManager.DemoApi.Data.Entities.WorkflowStep", b =>
                {
                    b.HasOne("Stio.WorkflowManager.DemoApi.Data.Entities.Workflow", "Workflow")
                        .WithMany("WorkflowSteps")
                        .HasForeignKey("WorkflowId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workflow");
                });

            modelBuilder.Entity("Stio.WorkflowManager.DemoApi.Data.Entities.User", b =>
                {
                    b.Navigation("Workflows");
                });

            modelBuilder.Entity("Stio.WorkflowManager.DemoApi.Data.Entities.Workflow", b =>
                {
                    b.Navigation("WorkflowSteps");
                });
#pragma warning restore 612, 618
        }
    }
}
