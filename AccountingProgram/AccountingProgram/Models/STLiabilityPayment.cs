using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingProgram.Models
{
    public class STLiabilityPayment
    {
        public Stliabilities STLiability { get; set; }
        public Payments Payment { get; set; }
       public List<Payments> PaymentList { get; set; }
    }
}
