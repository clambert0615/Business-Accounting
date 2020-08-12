using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class CashController : Controller
    {
        private readonly AccountingAPIDbContext _context;
     
        public Cash c = new Cash();

        public CashController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult CashIndex()
        {
            decimal bal = 0;
            var cashList = _context.Cash.ToList();
            
            foreach(Cash c in cashList)
            {
                c.Balance = bal + (c.Deposit ?? 0) - (c.Withdrawl ?? 0);
                bal = (c.Balance ?? 0);
            }
           
            return View(cashList);
        }
        public IActionResult IndividualCash(int id)
        {
            Cash found = _context.Cash.Find(id);
            return View(found);
        }

        [HttpGet]
        public IActionResult AddCash()
        {
            return View();
        }
        public IActionResult AddCash(Cash cash)
        {
            if(ModelState.IsValid)
            {
                _context.Cash.Add(cash);
                _context.SaveChanges();
                return RedirectToAction("CashIndex");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
    }
}
