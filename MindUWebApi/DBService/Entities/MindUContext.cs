﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DBService.Entities
{
    public partial class MindUContext : DbContext
    {
        public MindUContext()
        {
        }

        public MindUContext(DbContextOptions<MindUContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApiLogs> ApiLogs { get; set; }
        public virtual DbSet<Collaborators> Collaborators { get; set; }
        public virtual DbSet<CollaboratorsTechnologies> CollaboratorsTechnologies { get; set; }
        public virtual DbSet<Levels> Levels { get; set; }
        public virtual DbSet<Roles> Roles { get; set; }
        public virtual DbSet<Technologies> Technologies { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                throw new Exception("DbContext is not configured");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<ApiLogs>(entity =>
            {
                entity.HasKey(e => e.LogId);

                entity.Property(e => e.CreateDate).HasColumnType("datetime");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.LogLevel)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(500);
            });

            modelBuilder.Entity<Collaborators>(entity =>
            {
                entity.Property(e => e.FullName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasColumnName("isActive")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.TimeZone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Level)
                    .WithMany(p => p.Collaborators)
                    .HasForeignKey(d => d.Levelid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Collaborators_Levels");
            });

            modelBuilder.Entity<CollaboratorsTechnologies>(entity =>
            {
                entity.Property(e => e.Certificates).HasColumnType("xml");

                entity.Property(e => e.ProyectsUrl).HasColumnType("xml");

                entity.HasOne(d => d.Collaborator)
                    .WithMany(p => p.CollaboratorsTechnologies)
                    .HasForeignKey(d => d.Collaboratorid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollaboratorsTechnologies_Collaborators");

                entity.HasOne(d => d.Technology)
                    .WithMany(p => p.CollaboratorsTechnologies)
                    .HasForeignKey(d => d.TechnologyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CollaboratorsTechnologies_Technologies");
            });

            modelBuilder.Entity<Levels>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Technologies>(entity =>
            {
                entity.Property(e => e.TechnologyId).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.IsActive)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Salt)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Users_Roles");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
