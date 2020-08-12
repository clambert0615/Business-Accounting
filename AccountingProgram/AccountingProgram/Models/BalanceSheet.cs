using System;
using System.Collections.Generic;
using System.Linq;
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
        public Sales Sales { get; set; }

    }
}
