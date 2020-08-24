using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingProgram.Models
{
    public class IncomeStatement
    {
        public Sales Sales { get; set; }
        public decimal CostofGoodsSold { get; set; }
        public decimal Advertising { get; set; }
        public decimal Depreciation { get; set; }
        public decimal Insurance { get; set; }
        public decimal Interest { get; set; }
        public decimal EmployeeBenefits { get; set; }
        public decimal Meals { get; set; }
        public decimal Rent { get; set; }
        public decimal Supplies { get; set; }
        public decimal Travel { get; set; }
        public decimal Utilities { get; set; }
        public decimal Wages { get; set; }
        public decimal Vehicle { get; set; }
        public decimal Other { get; set; }
        public decimal TotalRevenue { get; set; }
        public decimal GrossProfit { get; set; }
        public decimal Totalexpenses { get; set; }
        public decimal IncomeTaxExpense {get; set;}
        public decimal IncomeBeforeTax { get; set; }
        public decimal NetIncome { get; set; }
        public decimal PayrollTax { get; set; }
        
    }
}
