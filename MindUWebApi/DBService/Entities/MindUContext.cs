using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

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
//#warning To protect potentially sensitive information in your connection string, you should move it out of source code.
//See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=DESKTOP-UO6JH86\\SQLEXPRESS; Database=MindU; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Collaborators>(entity =>
            {
                entity.HasKey(e => e.CollaboratorId);

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
                entity.HasKey(e => e.LevelId);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Roles>(entity =>
            {
                entity.HasKey(e => e.RoleId);

                entity.Property(e => e.RoleName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Technologies>(entity =>
            {
                entity.HasKey(e => e.TechnologyId);

                entity.Property(e => e.TechnologyId).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasKey(e => e.UserId);

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
