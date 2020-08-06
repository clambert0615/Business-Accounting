using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingProgram.Models
{
    public class Cash
    {
        public int Id { get; set; }
        public DateTime? TransDate { get; set; }
        public decimal? Deposit { get; set; }
        public decimal? Withdrawl { get; set; }
    }
}
