using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class SalesController : Controller
    {
        private readonly AccountingAPIDbContext _context;

        public SalesController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult SalesIndex()
        {
            return View();
        }
        [HttpGet]
        public IActionResult AddSale()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCashSale(Sales sale, List<InvGrid> list)
        {
            if (ModelState.IsValid)
            {
                _context.Sales.Add(sale);
                _context.SaveChanges();
                Cash c = new Cash();
                c.Deposit = sale.CashAmount;
                c.TransDate = sale.TransDate;
                c.SalesId = sale.SalesId;
                _context.Cash.Add(c);
                _context.SaveChanges();

                
                    
            }
            foreach (var item in list)
            {
                Inventory found = _context.Inventory.Find(item.InvId);
                found.Quantity -= item.Quantity;

                _context.Entry(found).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(found);
                _context.SaveChanges();

                decimal cogsexpense = item.Quantity * found.Price;
                Expenses cogs = new Expenses();
                cogs.Description = item.Description;
                cogs.Amount = cogsexpense;
                cogs.PaymentDate = sale.TransDate;
                _context.Expenses.Add(cogs);
                _context.SaveChanges();
            }
            return View();
        }
    }
}
