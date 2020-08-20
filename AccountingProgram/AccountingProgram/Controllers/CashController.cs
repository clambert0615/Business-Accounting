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
        [HttpGet]
        public IActionResult UpdateCash(int id)
        {
            Cash found = _context.Cash.Find(id);
            if(found != null)
            {
                return View(found);
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        [HttpPost]
        public IActionResult UpdateCash(Cash updatedCash)
        {
            Cash oldCash = _context.Cash.Find(updatedCash.Id);
            oldCash.TransDate = updatedCash.TransDate;
            oldCash.Withdrawl = updatedCash.Withdrawl;
            oldCash.Deposit = updatedCash.Deposit;
            _context.Entry(oldCash).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldCash);
            _context.SaveChanges();

            return RedirectToAction("CashIndex");
        }
    }
}
