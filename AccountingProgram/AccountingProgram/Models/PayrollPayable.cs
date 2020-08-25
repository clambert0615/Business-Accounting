using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public partial class PayrollPayable
    {
        public PayrollPayable()
        {
            Cash = new HashSet<Cash>();
            PayrollTaxesPayable = new HashSet<PayrollTaxesPayable>();
            Wages = new HashSet<Wages>();
        }

        public int PayrollId { get; set; }
        public decimal? MedicalIns { get; set; }
        public decimal? SalariesPay { get; set; }
        public decimal? EmployerMedIns { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime PayableDate { get; set; }
        public DateTime? PaymentDate { get; set; }
        public int? WageId { get; set; }
        public decimal? LocalIncTax { get; set; }
        public int? CashId { get; set; }
        public decimal? SalaryBalance { get; set; }
        public decimal? BenefitsTotal { get; set; }
        public decimal? BenefitsBalance { get; set; }
        public decimal? SavingsDed { get; set; }
        public decimal? SavingsDedBalance { get; set; }
        public decimal? BenefitsPayment { get; set; }
        public decimal? SavingsPayment { get; set; }
        public decimal? SalaryPayment { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime? BenefitPaymentDate { get; set; }

        public virtual Cash CashNavigation { get; set; }
        public virtual Wages Wage { get; set; }
        public virtual ICollection<Cash> Cash { get; set; }
        public virtual ICollection<PayrollTaxesPayable> PayrollTaxesPayable { get; set; }
        public virtual ICollection<Wages> Wages { get; set; }
    }
}
