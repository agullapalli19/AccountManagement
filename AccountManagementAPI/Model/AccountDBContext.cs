using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AccountManagementAPI.Model;

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

    public virtual DbSet<TblRefreshToken> TblRefreshTokens { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(e => e.AccountId).HasName("PK__Account__349DA5A63B7F05A8");

            entity.ToTable("Account");

            entity.Property(e => e.AccountNumber)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Balance).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");

            entity.HasOne(d => d.AccountHolder).WithMany(p => p.Accounts)
                .HasForeignKey(d => d.AccountHolderId)
                .HasConstraintName("FK__Account__Account__3B75D760");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.PersonId).HasName("PK__Person__AA2FFBE5069416BE");

            entity.ToTable("Person");

            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.FirstName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedOn).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<TblRefreshToken>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("Tbl_refreshToken");

            entity.Property(e => e.RefreshToken).HasMaxLength(1);
            entity.Property(e => e.TokenId).HasMaxLength(1);
            entity.Property(e => e.UserId).HasMaxLength(1);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Transact__3214EC07681D34AE");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1);

            entity.HasOne(d => d.Person).WithMany(p => p.Transactions)
                .HasForeignKey(d => d.PersonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Perso__440B1D61");

            entity.HasOne(d => d.SourceAccount).WithMany(p => p.TransactionSourceAccounts)
                .HasForeignKey(d => d.SourceAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Sourc__44FF419A");

            entity.HasOne(d => d.TargetAccount).WithMany(p => p.TransactionTargetAccounts)
                .HasForeignKey(d => d.TargetAccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Transacti__Targe__45F365D3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
