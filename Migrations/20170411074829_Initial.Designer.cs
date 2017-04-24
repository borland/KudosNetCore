using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using KudosNetCore.Model;

namespace KudosNetCore.Migrations
{
    [DbContext(typeof(Repository))]
    [Migration("20170411074829_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("KudosNetCore.Model.Kudo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Message");

                    b.Property<int>("TeamId");

                    b.Property<string>("UserName")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("Kudos");
                });

            modelBuilder.Entity("KudosNetCore.Model.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("KudosNetCore.Model.Kudo", b =>
                {
                    b.HasOne("KudosNetCore.Model.Team", "Team")
                        .WithMany("Kudos")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
