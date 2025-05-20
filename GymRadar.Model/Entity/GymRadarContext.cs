using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace GymRadar.Model.Entity;

public partial class GymRadarContext : DbContext
{
    public GymRadarContext()
    {
    }

    public GymRadarContext(DbContextOptions<GymRadarContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Gym> Gyms { get; set; }

    public virtual DbSet<GymCourse> GymCourses { get; set; }

    public virtual DbSet<Pt> Pts { get; set; }

    public virtual DbSet<Ptslot> Ptslots { get; set; }

    public virtual DbSet<Slot> Slots { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=14.225.220.28;Database=GymRadar;User Id=sa;Password=0363919179aN;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Account");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Role)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Admin>(entity =>
        {
            entity.ToTable("Admin");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Admins)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("FK_Admin_Account");
        });

        modelBuilder.Entity<Gym>(entity =>
        {
            entity.ToTable("Gym");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.GymName).HasMaxLength(50);
            entity.Property(e => e.Qrcode)
                .IsUnicode(false)
                .HasColumnName("QRCode");
            entity.Property(e => e.RepresentName).HasMaxLength(50);
            entity.Property(e => e.TaxCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Gyms)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Gym_Account");
        });

        modelBuilder.Entity<GymCourse>(entity =>
        {
            entity.ToTable("GymCourse");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.Type).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Gym).WithMany(p => p.GymCourses)
                .HasForeignKey(d => d.GymId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_GymCourse_Gym");
        });

        modelBuilder.Entity<Pt>(entity =>
        {
            entity.ToTable("PT");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.GoalTraining).HasMaxLength(50);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Pts)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PT_Account");

            entity.HasOne(d => d.Gym).WithMany(p => p.Pts)
                .HasForeignKey(d => d.GymId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PT_Gym_1");
        });

        modelBuilder.Entity<Ptslot>(entity =>
        {
            entity.ToTable("PTSlot");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.Ptid).HasColumnName("PTId");
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Pt).WithMany(p => p.Ptslots)
                .HasForeignKey(d => d.Ptid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PTSlot_PT_1");

            entity.HasOne(d => d.Slot).WithMany(p => p.Ptslots)
                .HasForeignKey(d => d.SlotId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_PTSlot_PTSlot");
        });

        modelBuilder.Entity<Slot>(entity =>
        {
            entity.ToTable("Slot");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.EndTime).HasPrecision(0);
            entity.Property(e => e.Name).HasMaxLength(50);
            entity.Property(e => e.StartTime).HasPrecision(0);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Gym).WithMany(p => p.Slots)
                .HasForeignKey(d => d.GymId)
                .HasConstraintName("FK_Slot_Gym");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("User");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreateAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UpdateAt).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Users)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Account");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
