using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class AccountsReceivable
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? Amount { get; set; }
    }
}
