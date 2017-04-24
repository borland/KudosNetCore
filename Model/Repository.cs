using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KudosNetCore.Model
{
    public class Repository : DbContext
    {
        public Repository(DbContextOptions<Repository> options) : base(options)
        {

        }

        public DbSet<Team> Teams { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Kudo> Kudos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserTeam>()
                .HasKey(u => new { u.UserId, u.TeamId }); // m2m join table
        }
    }
}
