using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class AccountsReceivable
    {
        public AccountsReceivable()
        {
            Sales = new HashSet<Sales>();
        }

        public int Id { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? CashAmount { get; set; }
        public decimal? AccRecAmount { get; set; }

        public virtual ICollection<Sales> Sales { get; set; }
    }
}
