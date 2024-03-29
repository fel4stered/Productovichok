﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ProductovichokBot.Data.Models;

namespace ProductovichokBot.Data;

public partial class ProductovichokContext : DbContext
{
    public ProductovichokContext()
    {
    }

    public ProductovichokContext(DbContextOptions<ProductovichokContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Address> Addresses { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Check> Checks { get; set; }

    public virtual DbSet<Code> Codes { get; set; }

    public virtual DbSet<House> Houses { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Street> Streets { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserAddress> UserAddresses { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
       => optionsBuilder.UseMySql("server=rc1b-kspwzb8gf9wxum7u.mdb.yandexcloud.net;user=kek;password=productovichok2116;database=productovichok", ServerVersion.Parse("8.0.25-mysql"));


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Address>(entity =>
        {
            entity.HasKey(e => e.AddressId).HasName("PRIMARY");

            entity.ToTable("address");

            entity.HasIndex(e => e.HouseId, "HouseID");

            entity.HasIndex(e => e.StreetId, "StreetID");

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.HouseId).HasColumnName("HouseID");
            entity.Property(e => e.StreetId).HasColumnName("StreetID");

            entity.HasOne(d => d.House).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.HouseId)
                .HasConstraintName("address_ibfk_2");

            entity.HasOne(d => d.Street).WithMany(p => p.Addresses)
                .HasForeignKey(d => d.StreetId)
                .HasConstraintName("address_ibfk_1");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(100);
        });

        modelBuilder.Entity<Check>(entity =>
        {
            entity.HasKey(e => e.ChecksId).HasName("PRIMARY");

            entity.HasIndex(e => e.OrderId, "OrderId_idx");

            entity.HasIndex(e => e.UserId, "UserId_idx");

            entity.HasOne(d => d.Order).WithMany(p => p.Checks)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("OrderId1");

            entity.HasOne(d => d.User).WithMany(p => p.Checks)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("UserId1");
        });

        modelBuilder.Entity<Code>(entity =>
        {
            entity.HasKey(e => e.CodeId).HasName("PRIMARY");

            entity.ToTable("codes");

            entity.HasIndex(e => e.UserId, "UserID");

            entity.Property(e => e.CodeId)
                .ValueGeneratedNever()
                .HasColumnName("CodeID");
            entity.Property(e => e.TimeAdd)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp")
                .HasColumnName("time_add");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Codes)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("codes_ibfk_1");
        });

        modelBuilder.Entity<House>(entity =>
        {
            entity.HasKey(e => e.HouseId).HasName("PRIMARY");

            entity.ToTable("house");

            entity.Property(e => e.HouseId).HasColumnName("HouseID");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PRIMARY");

            entity.ToTable("orders");

            entity.HasIndex(e => e.AddressId, "AddressID");

            entity.HasIndex(e => e.ClientId, "ClientID");

            entity.HasIndex(e => e.CourierId, "CourierID");

            entity.HasIndex(e => e.PickerId, "PickerID");

            entity.HasIndex(e => e.StatusId, "StatusID");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.ClientId).HasColumnName("ClientID");
            entity.Property(e => e.CourierId).HasColumnName("CourierID");
            entity.Property(e => e.DeliveryDateTime).HasColumnType("datetime");
            entity.Property(e => e.OrderComment).HasMaxLength(200);
            entity.Property(e => e.OrderDateTime).HasColumnType("datetime");
            entity.Property(e => e.PickerId).HasColumnName("PickerID");
            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.TotalPrice).HasPrecision(10, 2);

            entity.HasOne(d => d.Address).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AddressId)
                .HasConstraintName("orders_ibfk_5");

            entity.HasOne(d => d.Client).WithMany(p => p.OrderClients)
                .HasForeignKey(d => d.ClientId)
                .HasConstraintName("orders_ibfk_1");

            entity.HasOne(d => d.Courier).WithMany(p => p.OrderCouriers)
                .HasForeignKey(d => d.CourierId)
                .HasConstraintName("orders_ibfk_2");

            entity.HasOne(d => d.Picker).WithMany(p => p.OrderPickers)
                .HasForeignKey(d => d.PickerId)
                .HasConstraintName("orders_ibfk_3");

            entity.HasOne(d => d.Status).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusId)
                .HasConstraintName("orders_ibfk_4");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PRIMARY");

            entity.ToTable("orderdetails");

            entity.HasIndex(e => e.OrderId, "OrderID");

            entity.HasIndex(e => e.ProductId, "ProductID");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.ProductId).HasColumnName("ProductID");

            entity.HasOne(d => d.Order).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderdetails_ibfk_1");

            entity.HasOne(d => d.Product).WithMany(p => p.Orderdetails)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("orderdetails_ibfk_2");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.ToTable("products");

            entity.HasIndex(e => e.CategoryId, "CategoryID");

            entity.HasIndex(e => e.UnitId, "UnitID");

            entity.Property(e => e.ProductId).HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(250)
                .HasColumnName("ImageURL");
            entity.Property(e => e.ProductName).HasMaxLength(100);
            entity.Property(e => e.UnitId).HasColumnName("UnitID");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_ibfk_1");

            entity.HasOne(d => d.Unit).WithMany(p => p.Products)
                .HasForeignKey(d => d.UnitId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("products_ibfk_2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.RoleId).HasName("PRIMARY");

            entity.ToTable("roles");

            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.RoleName).HasMaxLength(50);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.StatusId).HasName("PRIMARY");

            entity.ToTable("statuses");

            entity.Property(e => e.StatusId).HasColumnName("StatusID");
            entity.Property(e => e.StatusName).HasMaxLength(50);
        });

        modelBuilder.Entity<Street>(entity =>
        {
            entity.HasKey(e => e.StreetId).HasName("PRIMARY");

            entity.ToTable("street");

            entity.Property(e => e.StreetId).HasColumnName("StreetID");
            entity.Property(e => e.StreetName).HasMaxLength(100);
        });

        modelBuilder.Entity<Unit>(entity =>
        {
            entity.HasKey(e => e.UnitId).HasName("PRIMARY");

            entity.ToTable("units");

            entity.Property(e => e.UnitId).HasColumnName("UnitID");
            entity.Property(e => e.Title).HasMaxLength(5);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.RoleId, "RoleID");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");
            entity.Property(e => e.Surname).HasMaxLength(100);
            entity.Property(e => e.TelegramUserNickname).HasMaxLength(20);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("users_ibfk_1");
        });

        modelBuilder.Entity<UserAddress>(entity =>
        {
            entity.HasKey(e => new { e.AddressId, e.UserId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("UserAddress");

            entity.HasIndex(e => e.UserId, "UserID_idx");

            entity.Property(e => e.AddressId).HasColumnName("AddressID");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.Address).WithMany(p => p.UserAddresses)
                .HasForeignKey(d => d.AddressId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("AddressID");

            entity.HasOne(d => d.User).WithMany(p => p.UserAddresses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("UserID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
