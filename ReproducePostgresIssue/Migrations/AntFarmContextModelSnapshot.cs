﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ReproducePostgresIssue;

namespace ReproducePostgresIssue.Migrations
{
    [DbContext(typeof(AntFarmContext))]
    partial class AntFarmContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("ReproducePostgresIssue.Entities.Ant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AgeInDays");

                    b.Property<string>("FavouriteAntGame");

                    b.Property<int?>("HiveId");

                    b.Property<bool>("IsLoyal");

                    b.Property<string>("Job");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("HiveId");

                    b.ToTable("Ants");
                });

            modelBuilder.Entity("ReproducePostgresIssue.Entities.Hive", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<decimal>("Latitude");

                    b.Property<decimal>("Longitude");

                    b.Property<string>("Name");

                    b.Property<int>("QueenId");

                    b.HasKey("Id");

                    b.ToTable("Hives");
                });

            modelBuilder.Entity("ReproducePostgresIssue.Entities.Queen", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AgeInDays");

                    b.Property<bool>("HasLifeInsurance");

                    b.Property<int?>("HiveId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("HiveId")
                        .IsUnique();

                    b.ToTable("Queens");
                });

            modelBuilder.Entity("ReproducePostgresIssue.Entities.Ant", b =>
                {
                    b.HasOne("ReproducePostgresIssue.Entities.Hive", "Hive")
                        .WithMany("Ants")
                        .HasForeignKey("HiveId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ReproducePostgresIssue.Entities.Queen", b =>
                {
                    b.HasOne("ReproducePostgresIssue.Entities.Hive", "Hive")
                        .WithOne("Queen")
                        .HasForeignKey("ReproducePostgresIssue.Entities.Queen", "HiveId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
