using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public partial class AccountsPayable
    {
        public AccountsPayable()
        {
            PayableInventory = new HashSet<PayableInventory>();
            PaymentsNavigation = new HashSet<Payments>();
        }

        public int PayableId { get; set; }
        public string VendorName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime DueDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime PaymentDate { get; set; }
        public decimal? PaymentAmount { get; set; } = 0;
        public decimal? Balance { get; set; }
        public decimal? AmountDue { get; set; }
        public int? VenId { get; set; }
        public int? PaymentsId { get; set; }
        public int? InvoiceId { get; set; }
        public int? SalesId { get; set; }

        public virtual Invoice Invoice { get; set; }
        public virtual Payments Payments { get; set; }
        public virtual Sales Sales { get; set; }
        public virtual Vendor Ven { get; set; }
        public virtual ICollection<PayableInventory> PayableInventory { get; set; }
        public virtual ICollection<Payments> PaymentsNavigation { get; set; }
    }
}
