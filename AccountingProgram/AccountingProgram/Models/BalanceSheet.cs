﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace AccountingProgram.Models
{
    public class BalanceSheet
    {
        public AccountsPayable Payable { get; set; }
        public AccountsReceivable Receivable { get; set; }
        public Cash Cash { get; set; }
        public Expenses Expense { get; set; }
        public Inventory Inventory { get; set; }
        public LongTermAssets LTAssets { get; set; }
        public LongTermLiabilities LTLiabilities { get; set; }
        public PayrollPayable PayrollPay { get; set; }
        public PayrollTaxesPayable PayTaxesPayable { get; set; }
        public Sales Sales { get; set; }
        public OwnersEquity Equity { get; set; }
        

        public decimal? Buildings { get; set; }
        public decimal? Equipment { get; set; }
        public decimal? Land { get; set; }
        public decimal TotalCurrentAssets { get; set; }
        public decimal TotalPPE { get; set; }
        public decimal TotalAssets { get; set; }
        public decimal? LoanBalance { get; set; }
        public decimal TotalLiabilities { get; set; }
        public decimal TotalLiabilitiesEquity { get; set; }
        public decimal CurrentLiabilities { get; set; }
        public decimal MarketableSecurities { get; set; }
        public decimal PrepaidInsurance { get; set; }
        public decimal PrepaidRent { get; set; }
        public decimal OtherPrepaidExpense { get; set; }
        public decimal OtherCurrentAsset { get; set; }
        public decimal AccruedExpenses { get; set; }
        public decimal UnearnedRevenue { get; set; }
        public decimal ShortTermDebt { get; set; }
        public decimal TaxesPayable { get; set; }
        public decimal CurrentLTDebt { get; set; }
        public decimal OtherCurrentLiabiltiy { get; set; }
    }
}
