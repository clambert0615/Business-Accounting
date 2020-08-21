using System;
using System.Collections.Generic;

namespace AccountingProgram.Models
{
    public partial class PayrollPayable
    {
        public int PayrollId { get; set; }
        public decimal? FedIncTaxWithheld { get; set; }
        public decimal? StateIncTaxWithheld { get; set; }
        public decimal? Ficasstax { get; set; }
        public decimal? Ficamed { get; set; }
        public decimal? MedicalIns { get; set; }
        public decimal? SalariesPay { get; set; }
        public decimal? EmployerFicass { get; set; }
        public decimal? EmployerMed { get; set; }
        public decimal? Futataxes { get; set; }
        public decimal? Sutataxes { get; set; }
        public decimal? EmployerMedIns { get; set; }
    }
}
