using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class OwnersEquity
    {
        public int OwnEquId { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Amount { get; set; }
    }
}
