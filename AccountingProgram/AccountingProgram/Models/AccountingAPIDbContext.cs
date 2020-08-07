using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AccountingProgram.Models
{
    public partial class AccountingAPIDbContext : DbContext
    {
        public AccountingAPIDbContext()
        {
        }

        public AccountingAPIDbContext(DbContextOptions<AccountingAPIDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AccountsPayable> AccountsPayable { get; set; }
        public virtual DbSet<AccountsReceivable> AccountsReceivable { get; set; }
        public virtual DbSet<Cash> Cash { get; set; }
        public virtual DbSet<Expenses> Expenses { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Sales> Sales { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.\\SQLExpress;Database=AccountingAPIDb;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountsPayable>(entity =>
            {
                entity.HasKey(e => e.PayableId)
                    .HasName("PK__Accounts__97CCDB3A63125E4A");

                entity.Property(e => e.PayableId).HasColumnName("PayableID");

                entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DueDate).HasColumnType("date");
                entity.Property(e => e.AmountDue).HasColumnType("decimal(10,2");

                entity.Property(e => e.PaymentAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PaymentDate).HasColumnType("date");

                entity.Property(e => e.VendorName)
                    .HasColumnName("Vendor Name")
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<AccountsReceivable>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CustomerName).HasMaxLength(200);

                entity.Property(e => e.DueDate).HasColumnType("date");
            });

            modelBuilder.Entity<Cash>(entity =>
            {
                entity.Property(e => e.Deposit).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TransDate).HasColumnType("date");

                entity.Property(e => e.Withdrawl).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Expenses>(entity =>
            {
                entity.HasKey(e => e.ExpId)
                    .HasName("PK__Expenses__45B117E7904F8CF4");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.PaymentDate).HasColumnType("date");
            });

            modelBuilder.Entity<Inventory>(entity =>
            {
                entity.HasKey(e => e.InvId)
                    .HasName("PK__Inventor__9DC82C6A224575BC");

                entity.Property(e => e.Description).HasMaxLength(100);

                entity.Property(e => e.Item)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Message).HasMaxLength(250);
            });

            modelBuilder.Entity<Sales>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TransDate).HasColumnType("date");
            });

            modelBuilder.Entity<Vendor>(entity =>
            {
                entity.HasKey(e => e.VenId)
                    .HasName("PK__Vendor__2203EDFAA8CE3173");

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.Name).HasMaxLength(100);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
