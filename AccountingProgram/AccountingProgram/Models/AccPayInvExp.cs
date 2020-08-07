﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountingProgram.Models
{
    public class AccPayInvExp
    {
        public AccountsPayable Payable { get; set; }
        public List<AccountsPayable> PayableList { get; set; }
        public Inventory Item { get; set; }
        public List<Inventory> InventoryList { get; set; }
       public Expenses Expense { get; set; }
    }
}
