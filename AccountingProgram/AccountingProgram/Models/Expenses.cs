using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingProgram.Models
{
    public class Expenses
    {
        public int ExpId { get; set; }
        public string Description { get; set; }
        public DateTime? PaymentDate { get; set; }
        public decimal? Amount { get; set; }
    }
}
