using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class Cash
    {
        public int Id { get; set; }
        public DateTime? TransDate { get; set; }
        public decimal? Deposit { get; set; }
        public decimal? Withdrawl { get; set; }
    }
}
