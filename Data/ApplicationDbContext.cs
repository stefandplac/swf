using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using swf.Models.DBObjects;

namespace swf.Data
{
    public partial class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Engineer> Engineers { get; set; } = null!;
        public virtual DbSet<Week> Weeks { get; set; } = null!;
        public virtual DbSet<WeeklySchedule> WeeklySchedules { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-585FI51K\\SQLEXPRESS;Database=SWF;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Engineer>(entity =>
            {
                entity.HasKey(e => e.IdEngineer)
                    .HasName("PK__tmp_ms_x__8920D67AF1FBDD67");

                entity.Property(e => e.IdEngineer).ValueGeneratedNever();

                entity.Property(e => e.FullName)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Week>(entity =>
            {
                entity.HasKey(e => e.IdWeek)
                    .HasName("PK__Weeks__24F85D29CABCF9B2");

                entity.Property(e => e.IdWeek).ValueGeneratedNever();
            });

            modelBuilder.Entity<WeeklySchedule>(entity =>
            {
                entity.HasKey(e => e.IdSchedule)
                    .HasName("PK__WeeklySc__D16D3B62B16541A7");

                entity.ToTable("WeeklySchedule");

                entity.Property(e => e.IdSchedule).ValueGeneratedNever();

                entity.HasOne(d => d.FirstHalfEngineer)
                    .WithMany(p => p.WeeklyScheduleFirstHalfEngineers)
                    .HasForeignKey(d => d.FirstHalfEngineerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SWF_Engineers_FirstHalfOfDay");

                entity.HasOne(d => d.SecondHalfEngineer)
                    .WithMany(p => p.WeeklyScheduleSecondHalfEngineers)
                    .HasForeignKey(d => d.SecondHalfEngineerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SWF_Engineers_SecondHalfOfDay");

                entity.HasOne(d => d.Week)
                    .WithMany(p => p.WeeklySchedules)
                    .HasForeignKey(d => d.WeekId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SWF_Weeks");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
