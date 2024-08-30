using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Model;

public partial class AccountDBContext : DbContext
{
    public AccountDBContext()
    {
    }

    public AccountDBContext(DbContextOptions<AccountDBContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Account> Accounts { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5A6893D4D2A");

            entity.ToTable("Account");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.AccountHolder).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.AccountHolderId)
                .HasConstraintName("FK__Account__Account__267ABA7A");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__Person__AA2FFBE541EF69D8");

            entity.ToTable("Person");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC077513E6B4");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Person).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Perso__29572725");

            entity.HasOne(d => d.SourceAccount).WithMany(p => p.TransactionSourceAccounts)
                .HasForeignKey(d => d.SourceAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Sourc__2A4B4B5E");

            entity.HasOne(d => d.TargetAccount).WithMany(p => p.TransactionTargetAccounts)
                .HasForeignKey(d => d.TargetAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Targe__2B3F6F97");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
