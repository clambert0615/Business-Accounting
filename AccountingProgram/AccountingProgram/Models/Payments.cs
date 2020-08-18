using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public partial class Payments
    {
        public Payments()
        {
            AccountsPayable = new HashSet<AccountsPayable>();
            LongTermLiabilities = new HashSet<LongTermLiabilities>();
        }

        public int PaymentId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime PayDate { get; set; }
        public decimal? Amount { get; set; }
        public int? PayId { get; set; }
        public int? CashId { get; set; }
        public int? LongTermLiabId { get; set; }
        public decimal? InterestExpense { get; set; }

        public virtual Cash Cash { get; set; }
        public virtual LongTermLiabilities LongTermLiab { get; set; }
        public virtual AccountsPayable Pay { get; set; }
        public virtual ICollection<AccountsPayable> AccountsPayable { get; set; }
        public virtual ICollection<LongTermLiabilities> LongTermLiabilities { get; set; }
    }
}
