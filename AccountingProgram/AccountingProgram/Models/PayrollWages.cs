using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingProgram.Models
{
    public class PayrollWages
    {
        PayrollPayable PayrollPayable { get; set; }
        List <PayrollPayable>  PPList { get; set; }
        Wages Wage { get; set; }
    }
}
