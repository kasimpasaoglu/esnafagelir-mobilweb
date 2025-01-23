using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using esnafagelir_mobilweb.DMO;

namespace esnafagelir_mobilweb.DataAccessLayer;

public partial class DataBaseContext : DbContext
{
    public DataBaseContext()
    {
    }

    public DataBaseContext(DbContextOptions<DataBaseContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<BusinessType> BusinessTypes { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<ContactRequest> ContactRequests { get; set; }

    public virtual DbSet<District> Districts { get; set; }

    public virtual DbSet<ExpertCategory> ExpertCategories { get; set; }

    public virtual DbSet<ExpertRequest> ExpertRequests { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=DefaultConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Business>(entity =>
        {
            entity.HasKey(e => e.BusinessId).HasName("PK__Business__F1EAA36E4308AF09");

            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.BusinessName).HasMaxLength(100);

            entity.HasOne(d => d.BusinessType).WithMany(p => p.Businesses)
                .HasForeignKey(d => d.BusinessTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Businesse__Busin__3E52440B");

            entity.HasOne(d => d.District).WithMany(p => p.Businesses)
                .HasForeignKey(d => d.DistrictId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Businesse__Distr__3F466844");
        });

        modelBuilder.Entity<BusinessType>(entity =>
        {
            entity.HasKey(e => e.BusinessTypeId).HasName("PK__Business__1D43DEC0C3EC73E5");

            entity.Property(e => e.BusinessTypeName).HasMaxLength(100);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.CityId).HasName("PK__Cities__F2D21B7611F75891");

            entity.Property(e => e.CityName).HasMaxLength(100);
        });

        modelBuilder.Entity<ContactRequest>(entity =>
        {
            entity.HasKey(e => e.ContactRequestId).HasName("PK__ContactR__96BC305F239BE450");

            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.RecordDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.ContactRequests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ContactRe__UserI__48CFD27E");
        });

        modelBuilder.Entity<District>(entity =>
        {
            entity.HasKey(e => e.DistrictId).HasName("PK__District__85FDA4C668F1555A");

            entity.Property(e => e.DistrictName).HasMaxLength(100);

            entity.HasOne(d => d.City).WithMany(p => p.Districts)
                .HasForeignKey(d => d.CityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Districts__CityI__398D8EEE");
        });

        modelBuilder.Entity<ExpertCategory>(entity =>
        {
            entity.HasKey(e => e.ExpertCategoryId).HasName("PK__ExpertCa__62AF8CBE8FA28C30");

            entity.Property(e => e.CategoryDescription).HasMaxLength(255);
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<ExpertRequest>(entity =>
        {
            entity.HasKey(e => e.ExpertRequestId).HasName("PK__ExpertRe__938A414B5BDCDD9A");

            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.RecordDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Business).WithMany(p => p.ExpertRequests)
                .HasForeignKey(d => d.BusinessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExpertReq__Busin__4F7CD00D");

            entity.HasOne(d => d.ExpertCategory).WithMany(p => p.ExpertRequests)
                .HasForeignKey(d => d.ExpertCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExpertReq__Exper__5070F446");

            entity.HasOne(d => d.User).WithMany(p => p.ExpertRequests)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ExpertReq__UserI__4E88ABD4");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PK__Roles__8AFACE1AA2BCA379");

            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Users__1788CC4CB599E800");

            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(15);
            entity.Property(e => e.RegisterDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Surname).HasMaxLength(100);

            entity.HasOne(d => d.Business).WithMany(p => p.Users)
                .HasForeignKey(d => d.BusinessId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__BusinessI__52593CB8");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleId__440B1D61");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
