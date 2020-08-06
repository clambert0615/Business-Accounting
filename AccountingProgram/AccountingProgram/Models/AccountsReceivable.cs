using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingProgram.Models
{
    public class AccountsReceivable
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? Amount { get; set; }
    }
}
