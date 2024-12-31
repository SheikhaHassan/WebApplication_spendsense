using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace budgetManagement.Models;

public partial class BudgetContext : DbContext
{
    public BudgetContext()
    {
    }

    public BudgetContext(DbContextOptions<BudgetContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<DashboardSummary> DashboardSummaries { get; set; }

    public virtual DbSet<Expense> Expenses { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCategorySpending> UserCategorySpendings { get; set; }

    public virtual DbSet<UserExpenseReport> UserExpenseReports { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer(WebApplication.CreateBuilder().Configuration.GetConnectionString("DB1"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.BudgetId).HasName("PK__Budget__3A655C1402074B8F");

            entity.ToTable("Budget");

            entity.Property(e => e.BudgetId).HasColumnName("budget_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.Needs)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("needs");
            entity.Property(e => e.Savings)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("savings");
            entity.Property(e => e.TotalIncome)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("total_income");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.Wants)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("wants");

            entity.HasOne(d => d.User).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fK_UserBudget");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Categori__D54EE9B425968A1D");

            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category_name");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ParentCategoryId).HasColumnName("parent_category_id");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("fK_ParentCategory");
        });

        modelBuilder.Entity<DashboardSummary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("DashboardSummary");

            entity.Property(e => e.RemainingBudget).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.TotalExpense).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.TotalIncome).HasColumnType("decimal(10, 2)");
        });

        modelBuilder.Entity<Expense>(entity =>
        {
            entity.HasKey(e => e.ExpenseId).HasName("PK__Expense__404B6A6BC966B176");

            entity.ToTable("Expense");

            entity.Property(e => e.ExpenseId).HasColumnName("expense_id");
            entity.Property(e => e.Amount)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("amount");
            entity.Property(e => e.CategoryId).HasColumnName("category_id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime")
                .HasColumnName("created_at");
            entity.Property(e => e.ExpenseDate).HasColumnName("expense_date");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Category).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fK_CategoryExpense");

            entity.HasOne(d => d.User).WithMany(p => p.Expenses)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fK_UserExpense");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__B9BE370F7652B367");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__AB6E616455725CE1").IsUnique();

            entity.Property(e => e.UserId)
                .ValueGeneratedNever()
                .HasColumnName("user_id");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("email");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false)
                .HasColumnName("password");
            entity.Property(e => e.Role)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValue("user")
                .HasColumnName("role");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("username");
        });

        modelBuilder.Entity<UserCategorySpending>(entity =>
        {
            entity.HasKey(e => e.RecordId).HasName("PK__UserCate__BFCFB4DDAF4A208D");

            entity.ToTable("UserCategorySpending");

            entity.Property(e => e.RecordId).HasColumnName("record_id");
            entity.Property(e => e.AmountSpent)
                .HasDefaultValue(0m)
                .HasColumnType("decimal(18, 0)")
                .HasColumnName("amount_spent");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("category_name");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.UserCategorySpendings)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_UserSpending");
        });

        modelBuilder.Entity<UserExpenseReport>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("UserExpenseReport");

            entity.Property(e => e.Category)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ExpenseAmount).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Uname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UName");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
