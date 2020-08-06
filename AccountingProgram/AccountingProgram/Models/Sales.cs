using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingProgram.Models
{
    public class Sales
    {
        public int SalesId { get; set; }
        public DateTime? TransDate { get; set; }
        public decimal? Amount { get; set; }
    }
}
