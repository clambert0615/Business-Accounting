using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public partial class Wages
    {
        public Wages()
        {
            PayrollPayable = new HashSet<PayrollPayable>();
            PayrollTaxesPayable = new HashSet<PayrollTaxesPayable>();
        }

        public int WageId { get; set; }
        public int? EmployeeId { get; set; }
        public int? CashId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime PayDate { get; set; }
        public decimal? GrossPay { get; set; }
        public decimal? Sstax { get; set; }
        public decimal? MedicareTax { get; set; }
        public decimal? FedIncTax { get; set; }
        public decimal? StateIncTax { get; set; }
        public decimal? LocalIncTax { get; set; }
        public decimal? InsuranceDed { get; set; }
        public decimal? SavingsDed { get; set; }
        public decimal? NetPay { get; set; }
        public int? PayrollPayableId { get; set; }

        public virtual Cash Cash { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual PayrollPayable PayrollPayableNavigation { get; set; }
        public virtual ICollection<PayrollPayable> PayrollPayable { get; set; }
        public virtual ICollection<PayrollTaxesPayable> PayrollTaxesPayable { get; set; }
    }
}
