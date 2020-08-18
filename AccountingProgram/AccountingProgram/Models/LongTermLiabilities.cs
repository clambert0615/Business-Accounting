using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class LongTermLiabilities
    {
        public LongTermLiabilities()
        {
            Expenses = new HashSet<Expenses>();
            Payments = new HashSet<Payments>();
        }

        public int LtliabilitiesId { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? TotalNumberofPayments { get; set; }
        public decimal? Balance { get; set; }
        public int? PaymentId { get; set; }
        public DateTime? OriginDate { get; set; }

        public virtual Payments Payment { get; set; }
        public virtual ICollection<Expenses> Expenses { get; set; }
        public virtual ICollection<Payments> Payments { get; set; }
    }
}
