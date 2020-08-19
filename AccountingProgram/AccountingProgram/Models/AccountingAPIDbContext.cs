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

        public IConfiguration Configuration { get; private set; }


        public virtual DbSet<AccountsPayable> AccountsPayable { get; set; }
        public virtual DbSet<AccountsReceivable> AccountsReceivable { get; set; }
        public virtual DbSet<AccumulatedDepreciation> AccumulatedDepreciation { get; set; }
        public virtual DbSet<Arreceipts> Arreceipts { get; set; }
        public virtual DbSet<Assets> Assets { get; set; }
        public virtual DbSet<Cash> Cash { get; set; }
        public virtual DbSet<Customers> Customers { get; set; }
        public virtual DbSet<Employee> Employee { get; set; }
        public virtual DbSet<Expenses> Expenses { get; set; }
        public virtual DbSet<Inventory> Inventory { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceInventory> InvoiceInventory { get; set; }
        public virtual DbSet<LongTermAssets> LongTermAssets { get; set; }
        public virtual DbSet<LongTermLiabilities> LongTermLiabilities { get; set; }
        public virtual DbSet<OwnersEquity> OwnersEquity { get; set; }
        public virtual DbSet<PayableInventory> PayableInventory { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Sales> Sales { get; set; }
        public virtual DbSet<SalesInventory> SalesInventory { get; set; }
        public virtual DbSet<Vendor> Vendor { get; set; }
        public virtual DbSet<Wages> Wages { get; set; }

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

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.AccountsPayable)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK__AccountsP__Invoi__4D5F7D71");

                entity.HasOne(d => d.Payments)
                    .WithMany(p => p.AccountsPayable)
                    .HasForeignKey(d => d.PaymentsId)
                    .HasConstraintName("FK__AccountsP__Payme__5FB337D6");

                entity.HasOne(d => d.Sales)
                    .WithMany(p => p.AccountsPayable)
                    .HasForeignKey(d => d.SalesId)
                    .HasConstraintName("FK__AccountsP__Sales__607251E5");

                entity.HasOne(d => d.Ven)
                    .WithMany(p => p.AccountsPayable)
                    .HasForeignKey(d => d.VenId)
                    .HasConstraintName("FK__AccountsP__VenId__5AEE82B9");
            });

            modelBuilder.Entity<AccountsReceivable>(entity =>
            {
                entity.Property(e => e.AccRecAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CashAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CustomerName).HasMaxLength(200);

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.AccountsReceivable)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK__AccountsR__Custo__19DFD96B");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.AccountsReceivable)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK__AccountsR__Invoi__151B244E");
            });

            modelBuilder.Entity<AccumulatedDepreciation>(entity =>
            {
                entity.HasKey(e => e.AccDepId)
                    .HasName("PK__Accumula__557ECC4D331CA9AD");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.LongTermAssetId).HasColumnName("LongTermAssetID");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Expense)
                    .WithMany(p => p.AccumulatedDepreciation)
                    .HasForeignKey(d => d.ExpenseId)
                    .HasConstraintName("FK__Accumulat__Expen__2057CCD0");

                entity.HasOne(d => d.LongTermAsset)
                    .WithMany(p => p.AccumulatedDepreciation)
                    .HasForeignKey(d => d.LongTermAssetId)
                    .HasConstraintName("FK__Accumulat__LongT__1F63A897");
            });

            modelBuilder.Entity<Arreceipts>(entity =>
            {
                entity.HasKey(e => e.ArreciptsId)
                    .HasName("PK__ARReceip__DA86A3E632B7ECD9");

                entity.ToTable("ARReceipts");

                entity.Property(e => e.ArreciptsId).HasColumnName("ARReciptsId");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.ReceiptDate).HasColumnType("date");

                entity.HasOne(d => d.AccountsRec)
                    .WithMany(p => p.Arreceipts)
                    .HasForeignKey(d => d.AccountsRecId)
                    .HasConstraintName("FK__ARReceipt__Accou__73852659");

                entity.HasOne(d => d.Cash)
                    .WithMany(p => p.Arreceipts)
                    .HasForeignKey(d => d.CashId)
                    .HasConstraintName("FK__ARReceipt__CashI__6442E2C9");

                entity.HasOne(d => d.Sales)
                    .WithMany(p => p.Arreceipts)
                    .HasForeignKey(d => d.SalesId)
                    .HasConstraintName("FK__ARReceipt__Sales__634EBE90");
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
                entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.BeginAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Deposit).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TransDate).HasColumnType("date");

                entity.Property(e => e.Withdrawl).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Expense)
                    .WithMany(p => p.Cash)
                    .HasForeignKey(d => d.ExpenseId)
                    .HasConstraintName("FK__Cash__ExpenseId__19AACF41");

                entity.HasOne(d => d.Sales)
                    .WithMany(p => p.Cash)
                    .HasForeignKey(d => d.SalesId)
                    .HasConstraintName("FK__Cash__SalesId__70DDC3D8");
            });

            modelBuilder.Entity<Customers>(entity =>
            {
                entity.HasKey(e => e.CustId)
                    .HasName("PK__Customer__049E3AA926A629F8");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(100);

                entity.Property(e => e.Phone).HasMaxLength(15);

                entity.Property(e => e.State).HasMaxLength(10);

                entity.Property(e => e.StreetAdd).HasMaxLength(100);
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

                entity.HasOne(d => d.AccDep)
                    .WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.AccDepId)
                    .HasConstraintName("FK__Expenses__AccDep__2CBDA3B5");

                entity.HasOne(d => d.CashNavigation)
                    .WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.CashId)
                    .HasConstraintName("FK__Expenses__CashId__1A9EF37A");

                entity.HasOne(d => d.LongTermLiab)
                    .WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.LongTermLiabId)
                    .HasConstraintName("FK__Expenses__LongTe__251C81ED");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Expenses)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__Expenses__Paymen__3FD07829");
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

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.AmountDue).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.City).HasMaxLength(50);

                entity.Property(e => e.CustomerName).HasMaxLength(200);

                entity.Property(e => e.DueDate).HasColumnType("date");

                entity.Property(e => e.InvDate).HasColumnType("date");

                entity.Property(e => e.SalesTax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.State).HasMaxLength(5);

                entity.Property(e => e.StreetAddress).HasMaxLength(200);

                entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.InventoryId)
                    .HasConstraintName("FK__Invoice__Invento__02FC7413");
            });

            modelBuilder.Entity<InvoiceInventory>(entity =>
            {
                entity.HasKey(e => e.InvInvId)
                    .HasName("PK__InvoiceI__B775B945BF94B12D");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.InvoiceInventory)
                    .HasForeignKey(d => d.InventoryId)
                    .HasConstraintName("FK__InvoiceIn__Inven__3C34F16F");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceInventory)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK__InvoiceIn__Invoi__3D2915A8");
            });

            modelBuilder.Entity<LongTermAssets>(entity =>
            {
                entity.HasKey(e => e.LtassetId)
                    .HasName("PK__LongTerm__778B6714E1318721");

                entity.Property(e => e.LtassetId).HasColumnName("LTAssetId");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Balance).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Description).HasMaxLength(200);

                entity.Property(e => e.Item).HasMaxLength(50);

                entity.Property(e => e.LifeRemaining).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PurchaseDate).HasColumnType("date");

                entity.Property(e => e.UsefulLife).HasColumnType("decimal(10, 2)");
            });

            modelBuilder.Entity<LongTermLiabilities>(entity =>
            {
                entity.HasKey(e => e.LtliabilitiesId)
                    .HasName("PK__LongTerm__A5948EF54D4D0F50");

                entity.Property(e => e.LtliabilitiesId).HasColumnName("LTLiabilitiesId");

                entity.Property(e => e.Ltlbalance)
                    .HasColumnName("LTLBalance")
                    .HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Ltldescription)
                    .HasColumnName("LTLDescription")
                    .HasMaxLength(200);

                entity.Property(e => e.Ltlitem)
                    .HasColumnName("LTLItem")
                    .HasMaxLength(50);

                entity.Property(e => e.OriginDate).HasColumnType("date");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.LongTermLiabilities)
                    .HasForeignKey(d => d.PaymentId)
                    .HasConstraintName("FK__LongTermL__Payme__2334397B");
            });

            modelBuilder.Entity<OwnersEquity>(entity =>
            {
                entity.HasKey(e => e.OwnEquId)
                    .HasName("PK__OwnersEq__8599DC9AF30546C9");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Date).HasColumnType("date");
            });

            modelBuilder.Entity<PayableInventory>(entity =>
            {
                entity.HasKey(e => e.PayInvId)
                    .HasName("PK__PayableI__6DE6E75FCEFAADAA");

                entity.Property(e => e.InvPrice).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.PayableInventory)
                    .HasForeignKey(d => d.InventoryId)
                    .HasConstraintName("FK__PayableIn__Inven__09746778");

                entity.HasOne(d => d.Payable)
                    .WithMany(p => p.PayableInventory)
                    .HasForeignKey(d => d.PayableId)
                    .HasConstraintName("FK__PayableIn__Payab__0880433F");
            });

            modelBuilder.Entity<Payments>(entity =>
            {
                entity.HasKey(e => e.PaymentId)
                    .HasName("PK__Payments__9B556A383B500A89");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.InterestExpense).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.PayDate).HasColumnType("date");

                entity.Property(e => e.TotalAmount).HasColumnType("decimal(10, 2)");

                entity.HasOne(d => d.Cash)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.CashId)
                    .HasConstraintName("FK__Payments__CashId__5EBF139D");

                entity.HasOne(d => d.Expense)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.ExpenseId)
                    .HasConstraintName("FK__Payments__Expens__40C49C62");

                entity.HasOne(d => d.LongTermLiab)
                    .WithMany(p => p.Payments)
                    .HasForeignKey(d => d.LongTermLiabId)
                    .HasConstraintName("FK__Payments__LongTe__24285DB4");

                entity.HasOne(d => d.Pay)
                    .WithMany(p => p.PaymentsNavigation)
                    .HasForeignKey(d => d.PayId)
                    .HasConstraintName("FK__Payments__PayId__5DCAEF64");
            });

            modelBuilder.Entity<Sales>(entity =>
            {
                entity.Property(e => e.AccRecAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Amount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.CashAmount).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.SalesTax).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.Subtotal).HasColumnType("decimal(10, 2)");

                entity.Property(e => e.TransDate).HasColumnType("date");

                entity.HasOne(d => d.AccRec)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.AccRecId)
                    .HasConstraintName("FK__Sales__AccRecId__6EF57B66");

                entity.HasOne(d => d.CashNavigation)
                    .WithMany(p => p.SalesNavigation)
                    .HasForeignKey(d => d.CashId)
                    .HasConstraintName("FK__Sales__CashId__6FE99F9F");

                entity.HasOne(d => d.Inv)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.InvId)
                    .HasConstraintName("FK__Sales__InvId__6E01572D");

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.Sales)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK__Sales__InvoiceId__2739D489");
            });

            modelBuilder.Entity<SalesInventory>(entity =>
            {
                entity.HasOne(d => d.Inventory)
                    .WithMany(p => p.SalesInventory)
                    .HasForeignKey(d => d.InventoryId)
                    .HasConstraintName("FK__SalesInve__Inven__40F9A68C");

                entity.HasOne(d => d.Sales)
                    .WithMany(p => p.SalesInventory)
                    .HasForeignKey(d => d.SalesId)
                    .HasConstraintName("FK__SalesInve__Sales__40058253");
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
