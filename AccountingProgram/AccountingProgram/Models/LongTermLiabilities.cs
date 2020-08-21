using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        public string Ltlitem { get; set; }
        public string Ltldescription { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? TotalNumberofPayments { get; set; }
        public decimal? Ltlbalance { get; set; }
        public int? PaymentId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime OriginDate { get; set; }

        public virtual Payments Payment { get; set; }
        public virtual ICollection<Expenses> Expenses { get; set; }
        public virtual ICollection<Payments> Payments { get; set; }
    }
}
