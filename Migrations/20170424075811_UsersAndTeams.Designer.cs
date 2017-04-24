using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using KudosNetCore.Model;

namespace KudosNetCore.Migrations
{
    [DbContext(typeof(Repository))]
    [Migration("20170424075811_UsersAndTeams")]
    partial class UsersAndTeams
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

                    b.Property<string>("CreatedBy")
                        .HasMaxLength(100);

                    b.Property<string>("ForWho")
                        .HasMaxLength(100);

                    b.Property<string>("Message");

                    b.Property<int>("TeamId");

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

            modelBuilder.Entity("KudosNetCore.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Flags");

                    b.Property<string>("FullName")
                        .HasMaxLength(100);

                    b.Property<byte[]>("PasswordHash");

                    b.Property<byte[]>("PasswordSalt");

                    b.Property<string>("Username")
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("KudosNetCore.Model.UserTeam", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("TeamId");

                    b.HasKey("UserId", "TeamId");

                    b.HasIndex("TeamId");

                    b.ToTable("UserTeam");
                });

            modelBuilder.Entity("KudosNetCore.Model.Kudo", b =>
                {
                    b.HasOne("KudosNetCore.Model.Team", "Team")
                        .WithMany("Kudos")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("KudosNetCore.Model.UserTeam", b =>
                {
                    b.HasOne("KudosNetCore.Model.Team", "Team")
                        .WithMany("UserTeams")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("KudosNetCore.Model.User", "User")
                        .WithMany("UserTeams")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
