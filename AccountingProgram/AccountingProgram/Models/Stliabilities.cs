using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class Stliabilities
    {
        public int StliabilityId { get; set; }
        public DateTime? OriginDate { get; set; }
        public int? PaymentId { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Item { get; set; }
        public string Description { get; set; }
        public decimal? Amount { get; set; }
        public decimal? Balance { get; set; }

        public virtual Payments Payment { get; set; }
    }
}
