using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AccountingProgram.Models
{
    public partial class PayrollTaxesPayable
    {
        public int PayTaxesId { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime PayrollDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]

        public DateTime? PaymentDate { get; set; }
        public decimal? Amount { get; set; }
        public decimal? PaymentAmount { get; set; }
        public decimal? Balance { get; set; }
        public int? PayrollPayId { get; set; }
        public int? CashId { get; set; }
        public decimal? FedInTaxWithheld { get; set; }
        public decimal? StateIncTaxWithheld { get; set; }
        public decimal? LocalIncomeTaxWithheld { get; set; }
        public decimal? Ficasstax { get; set; }
        public decimal? Ficamed { get; set; }
        public decimal? EmployerFicass { get; set; }
        public decimal? EmployerFicamed { get; set; }
        public decimal? Futataxes { get; set; }
        public decimal? Sutataxes { get; set; }
        public decimal? EmployerMedIns { get; set; }
        public int? WageId { get; set; }

        public virtual Cash Cash { get; set; }
        public virtual PayrollPayable PayrollPay { get; set; }
        public virtual Wages Wage { get; set; }
    }
}
