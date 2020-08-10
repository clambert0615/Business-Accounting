using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace AccountingProgram.Models
{
    public partial class AccountingAPIDbContext : DbContext
    {
        public AccountingAPIDbContext()
        {
        }

        public AccountingAPIDbContext(DbContextOptions<AccountingAPIDbContext> options, IConfiguration configuration)
            : base(options)
        {
            Configuration = configuration;
        }

        public virtual DbSet<AccountsPayable> AccountsPayable { get; set; }
        public virtual DbSet<AccountsReceivable> AccountsReceivable { get; set; }
        public virtual DbSet<Assets> Assets { get; set; }
        public virtual DbSet<Cash> Cash { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Expenses> Expenses { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Sales> Sales { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<Wages> Wages { get; set; }
        public IConfiguration Configuration { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")); 
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountsPayable>(entity =>
            {
                entity.HasKey(e => e.PayableId)
                    .HasName("PK__Accounts__97CCDB3A63125E4A");

                entity.Property(e => e.PayableId).HasColumnName("PayableID");

                entity.Property(e => e.AmountDue).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.PaymentAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PaymentDate).HasColumnType("date");

                entity.Property(e => e.VendorName)
                    .HasColumnName("Vendor Name")
                    .HasMaxLength(100);

                entity.HasOne(d => d.Payments)
                    .WithMany(p => p.AccountsPayable)
                    .HasForeignKey(d => d.PaymentsId)
                    .HasConstraintName("FK__AccountsP__Payme__5FB337D6");

                entity.HasOne(d => d.Ven)
                    .WithMany(p => p.AccountsPayable)
                    .HasForeignKey(d => d.VenId)
                    .HasConstraintName("FK__AccountsP__VenId__5AEE82B9");
            });

            modelBuilder.Entity<AccountsReceivable>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CustomerName).HasMaxLength(200);

                entity.Property(e => e.DueDate).HasColumnType("date");
            });

            modelBuilder.Entity<Assets>(entity =>
            {
                entity.HasKey(e => e.AssetId)
                    .HasName("PK__Assets__43492352E2E69DB5");

                entity.Property(e => e.AccDepreciation).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Cost).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Type).HasMaxLength(100);

                entity.Property(e => e.UsefulLife).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Cash>(entity =>
            {
                entity.Property(e => e.Deposit).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TransDate).HasColumnType("date");

                entity.Property(e => e.Withdrawl).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.EmpId)
                    .HasName("PK__Employee__AF2DBB999CDD5FF0");

                entity.Property(e => e.Birthdate).HasColumnType("date");

                entity.Property(e => e.City).HasMaxLength(30);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.FirstName).HasMaxLength(30);

                entity.Property(e => e.LastName).HasMaxLength(30);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.Ssn).HasColumnName("SSN");

                entity.Property(e => e.State).HasMaxLength(10);

                entity.Property(e => e.StreetAddress).HasMaxLength(100);
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

                entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<Payments>(entity =>
            {
                entity.HasKey(e => e.PaymentId)
                    .HasName("PK__Payments__9B556A383B500A89");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PayDate).HasColumnType("date");

                entity.HasOne(d => d.Cash)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.CashId)
                    .HasConstraintName("FK__Payments__CashId__5EBF139D");

                entity.HasOne(d => d.Pay)
                    .WithMany(p => p.PaymentsNavigation)
                    .HasForeignKey(d => d.PayId)
                    .HasConstraintName("FK__Payments__PayId__5DCAEF64");
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

                entity.Property(e => e.City).HasMaxLength(30);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.State).HasMaxLength(30);
            });

            modelBuilder.Entity<Wages>(entity =>
            {
                entity.HasKey(e => e.WageId)
                    .HasName("PK__Wages__6CDF5E8A3FF44E49");

                entity.Property(e => e.FedIncTax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.GrossPay).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InsuranceDed).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.LocalIncTax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.MedicareTax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.NetPay).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PayDate).HasColumnType("date");

                entity.Property(e => e.SavingsDed).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Sstax)
                    .HasColumnName("SSTax")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.StateIncTax).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Cash)
                    .WithMany(p => p.Wages)
                    .HasForeignKey(d => d.CashId)
                    .HasConstraintName("FK__Wages__CashId__6754599E");

                entity.HasOne(d => d.Employee)
                    .WithMany(p => p.Wages)
                    .HasForeignKey(d => d.EmployeeId)
                    .HasConstraintName("FK__Wages__EmployeeI__66603565");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
