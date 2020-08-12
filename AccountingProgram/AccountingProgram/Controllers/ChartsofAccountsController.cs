using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class ChartsofAccountsController : Controller
    {
        private readonly AccountingAPIDbContext _context;
        public BalanceSheet bs = new BalanceSheet(); 
        public ChartsofAccountsController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult ChartsofAccountsIndex()
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
            return View();
        }
    }
}
