using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class Expenses
    {
        public int ExpId { get; set; }
        public string Description { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? Amount { get; set; }
    }
}
