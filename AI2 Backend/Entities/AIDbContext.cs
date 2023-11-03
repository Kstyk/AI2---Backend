﻿using Microsoft.EntityFrameworkCore;

namespace AI2_Backend.Entities
{
    public class AIDbContext : DbContext
    {
        public AIDbContext(DbContextOptions<AIDbContext> options) : base(options)
        {


        }

        public DbSet<Role> Roles { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(255);
            modelBuilder.Entity<User>()
                .Property(e => e.RoleId)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(e => e.Password)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(e => e.FirstName)
                .HasMaxLength(255)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(e => e.LastName)
                .HasMaxLength(255)
                .IsRequired();
            modelBuilder.Entity<User>()
                .Property(e => e.AboutMe)
                .HasColumnType("text")
                .HasMaxLength(10000);
            modelBuilder.Entity<User>()
                .Property(e => e.Education)
                .HasColumnType("text")
                .HasMaxLength(10000);

        }


    }
}