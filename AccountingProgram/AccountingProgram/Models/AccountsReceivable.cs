using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public partial class AccountsReceivable
    {
        public AccountsReceivable()
        {
            Arreceipts = new HashSet<Arreceipts>();
            Sales = new HashSet<Sales>();
        }

        public int Id { get; set; }
        public string CustomerName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime DueDate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? CashAmount { get; set; }
        public decimal? AccRecAmount { get; set; }
        public int? InvoiceId { get; set; }
        public int? CustomerId { get; set; }
        public decimal? Balance { get; set; }

        public virtual Customers Customer { get; set; }
        public virtual Invoice Invoice { get; set; }
        public virtual ICollection<Arreceipts> Arreceipts { get; set; }
        public virtual ICollection<Sales> Sales { get; set; }
    }
}
