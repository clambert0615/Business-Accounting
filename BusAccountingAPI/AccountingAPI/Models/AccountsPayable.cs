using System;
using System.Collections.Generic;

namespace AccountingAPI.Models
{
    public partial class AccountsPayable
    {
        public int PayableId { get; set; }
        public string Payee { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? PaymentAmount { get; set; }
        public decimal? Balance { get; set; }
    }
}
