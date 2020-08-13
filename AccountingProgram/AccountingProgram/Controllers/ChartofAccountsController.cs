using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class ChartofAccountsController : Controller
    {
        private readonly AccountingAPIDbContext _context;
        public BalanceSheet bs = new BalanceSheet { Cash = new Cash(), Payable = new AccountsPayable(), 
        Receivable = new AccountsReceivable(), Expense = new Expenses(), Inventory = new Inventory(), Sales = new Sales() };
        
       
        public ChartofAccountsController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult ChartofAccountsIndex()
        {
            return View();
        }
        public IActionResult BalanceSheet()
        {
            List<Cash> cashList = _context.Cash.ToList();
            decimal bal = 0;
            foreach (Cash c in cashList)
            {
                c.Balance = bal + (c.Deposit ?? 0) - (c.Withdrawl ?? 0);
                bal = (c.Balance ?? 0);
            }
            bs.Cash.Balance = bal;
            List<AccountsPayable> payableList = _context.AccountsPayable.ToList();
            decimal apbal = 0;
            foreach(AccountsPayable ap in payableList)
            {
                apbal += (ap.Balance ?? 0);
            }
            bs.Payable.Balance = apbal;
            List<AccountsReceivable> arList = _context.AccountsReceivable.ToList();
            decimal arbal = 0;
            foreach(AccountsReceivable ar in arList)
            {
                arbal += (ar.Balance ?? 0);
            }
            bs.Receivable.Balance = arbal;
            List<Expenses> expList = _context.Expenses.ToList();
            decimal expbal = 0;
            foreach(Expenses e in expList)
            {
                expbal += (e.Amount ?? 0);
            }
            bs.Expense.Amount = expbal;
            List<Inventory> invList = _context.Inventory.ToList();
            decimal invbal = 0;
            foreach(Inventory i in invList)
            {
                invbal += (i.Price) * (i.Quantity ?? 0);
            }
            bs.Inventory.Price = invbal;

            return View(bs);
        }
    }
}
