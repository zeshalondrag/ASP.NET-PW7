using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace SORAPC.Models;

public partial class SorapcContext : DbContext
{
    public SorapcContext()
    {
    }

    public SorapcContext(DbContextOptions<SorapcContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cart> Carts { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderPosition> OrderPositions { get; set; }

    public virtual DbSet<OrderStatus> OrderStatuses { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductCategory> ProductCategories { get; set; }

    public virtual DbSet<ProductStatus> ProductStatuses { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=ZESHALONDRAG\\SQLEXPRESS;Initial Catalog=sorapc;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cart>(entity =>
        {
            entity.HasKey(e => e.IdCart).HasName("PK__Cart__72140ECF68C4C47E");

            entity.ToTable("Cart");

            entity.Property(e => e.IdCart).HasColumnName("ID_Cart");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.UsersId).HasColumnName("Users_ID");

            entity.HasOne(d => d.Product).WithMany(p => p.Carts)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Cart__Product_ID__6A30C649");

            entity.HasOne(d => d.Users).WithMany(p => p.Carts)
                .HasForeignKey(d => d.UsersId)
                .HasConstraintName("FK__Cart__Users_ID__693CA210");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK__Orders__EC9FA955E24F1CCA");

            entity.HasIndex(e => e.OrderNumber, "UQ__Orders__67C7B3CBA19F38CE").IsUnique();

            entity.Property(e => e.IdOrder).HasColumnName("ID_Order");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("Order_Date");
            entity.Property(e => e.OrderNumber)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("Order_Number");
            entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatus_ID");
            entity.Property(e => e.TotalAmount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Total_Amount");
            entity.Property(e => e.UsersId).HasColumnName("Users_ID");

            entity.HasOne(d => d.OrderStatus).WithMany(p => p.Orders)
                .HasForeignKey(d => d.OrderStatusId)
                .HasConstraintName("FK__Orders__OrderSta__628FA481");

            entity.HasOne(d => d.Users).WithMany(p => p.Orders)
                .HasForeignKey(d => d.UsersId)
                .HasConstraintName("FK__Orders__Users_ID__60A75C0F");
        });

        modelBuilder.Entity<OrderPosition>(entity =>
        {
            entity.HasKey(e => e.IdOrderPosition).HasName("PK__OrderPos__FD3AA9D6AE8476AB");

            entity.ToTable("OrderPosition");

            entity.Property(e => e.IdOrderPosition).HasColumnName("ID_OrderPosition");
            entity.Property(e => e.OrderId).HasColumnName("Order_ID");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderPositions)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("FK__OrderPosi__Order__656C112C");

            entity.HasOne(d => d.Product).WithMany(p => p.OrderPositions)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__OrderPosi__Produ__66603565");
        });

        modelBuilder.Entity<OrderStatus>(entity =>
        {
            entity.HasKey(e => e.IdOrderStatus).HasName("PK__OrderSta__36EC88CF6E69E807");

            entity.ToTable("OrderStatus");

            entity.HasIndex(e => e.Title, "UQ__OrderSta__2CB664DCA591EFED").IsUnique();

            entity.Property(e => e.IdOrderStatus).HasColumnName("ID_OrderStatus");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.IdProduct).HasName("PK__Products__522DE49672A5E0CD");

            entity.HasIndex(e => e.Names, "UQ__Products__44C034865331BDD4").IsUnique();

            entity.Property(e => e.IdProduct).HasColumnName("ID_Product");
            entity.Property(e => e.Descriptions).IsUnicode(false);
            entity.Property(e => e.Img).IsUnicode(false);
            entity.Property(e => e.Names)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ProductCategoryId).HasColumnName("ProductCategory_ID");
            entity.Property(e => e.ProductStatusId).HasColumnName("ProductStatus_ID");

            entity.HasOne(d => d.ProductCategory).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductCategoryId)
                .HasConstraintName("FK__Products__Produc__59FA5E80");

            entity.HasOne(d => d.ProductStatus).WithMany(p => p.Products)
                .HasForeignKey(d => d.ProductStatusId)
                .HasConstraintName("FK__Products__Produc__59063A47");
        });

        modelBuilder.Entity<ProductCategory>(entity =>
        {
            entity.HasKey(e => e.IdProductCategory).HasName("PK__ProductC__8FAD631E86039AC9");

            entity.ToTable("ProductCategory");

            entity.HasIndex(e => e.Title, "UQ__ProductC__2CB664DC4DBFC197").IsUnique();

            entity.Property(e => e.IdProductCategory).HasColumnName("ID_ProductCategory");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProductStatus>(entity =>
        {
            entity.HasKey(e => e.IdProductStatus).HasName("PK__ProductS__92EB9BF6390180AA");

            entity.ToTable("ProductStatus");

            entity.HasIndex(e => e.Title, "UQ__ProductS__2CB664DC4367AF7E").IsUnique();

            entity.Property(e => e.IdProductStatus).HasColumnName("ID_ProductStatus");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.IdReview).HasName("PK__Reviews__E39E964743BCAF49");

            entity.Property(e => e.IdReview).HasColumnName("ID_Review");
            entity.Property(e => e.Comment).IsUnicode(false);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.ProductId).HasColumnName("Product_ID");
            entity.Property(e => e.UsersId).HasColumnName("Users_ID");

            entity.HasOne(d => d.Product).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Reviews__Product__6D0D32F4");

            entity.HasOne(d => d.Users).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UsersId)
                .HasConstraintName("FK__Reviews__Users_I__6E01572D");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.IdRole).HasName("PK__Roles__43DCD32DBDBCA27B");

            entity.HasIndex(e => e.Title, "UQ__Roles__2CB664DC3DCEBBE2").IsUnique();

            entity.Property(e => e.IdRole).HasColumnName("ID_Role");
            entity.Property(e => e.Title)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUsers).HasName("PK__Users__B97FFDA1E031020F");

            entity.HasIndex(e => e.Phone, "UQ__Users__5C7E359ED159147A").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105346143221C").IsUnique();

            entity.HasIndex(e => e.Logins, "UQ__Users__D00D0632978A6AB0").IsUnique();

            entity.Property(e => e.IdUsers).HasColumnName("ID_Users");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Logins)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Passwords).IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(18)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("Role_ID");
            entity.Property(e => e.UserMiddlename)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserSurname)
                .HasMaxLength(50)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("FK__Users__Role_ID__4F7CD00D");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
