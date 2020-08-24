using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public partial class Expenses
    {
        public Expenses()
        {
            AccumulatedDepreciation = new HashSet<AccumulatedDepreciation>();
            Cash = new HashSet<Cash>();
            Payments = new HashSet<Payments>();
        }

        public int ExpId { get; set; }
        public string Description { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime PaymentDate { get; set; }
        public decimal? Amount { get; set; }
        public int? CashId { get; set; }
        public int? LongTermLiabId { get; set; }
        public int? AccDepId { get; set; }
        public int? PaymentId { get; set; }

        public virtual AccumulatedDepreciation AccDep { get; set; }
        public virtual Cash CashNavigation { get; set; }
        public virtual LongTermLiabilities LongTermLiab { get; set; }
        public virtual Payments Payment { get; set; }
        public virtual ICollection<AccumulatedDepreciation> AccumulatedDepreciation { get; set; }
        public virtual ICollection<Cash> Cash { get; set; }
        public virtual ICollection<Payments> Payments { get; set; }
    }
}
