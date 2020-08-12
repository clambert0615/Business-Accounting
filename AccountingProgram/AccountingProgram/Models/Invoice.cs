using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public partial class Invoice
    {
        public Invoice()
        {
            AccountsReceivable = new HashSet<AccountsReceivable>();
        }

        public int InvoiceId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime InvDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime DueDate { get; set; }
        public string CustomerName { get; set; }
        public string StreetAddress { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public int? Zip { get; set; }
        public decimal? AmountDue { get; set; }
        public decimal? SalesTax { get; set; }
        public decimal? Subtotal { get; set; }
        public int? InventoryId { get; set; }

        public virtual Inventory Inventory { get; set; }
        public virtual ICollection<AccountsReceivable> AccountsReceivable { get; set; }
    }
}
