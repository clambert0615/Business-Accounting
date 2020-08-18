using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly AccountingAPIDbContext _context;
        public AccPayInvExp apie = new AccPayInvExp();
        public ExpenseController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult ExpenseIndex()
        {
            List<Expenses> expenseList = _context.Expenses.ToList();
            apie.ExpenseList = expenseList;
            return View(apie);
        }
        public IActionResult IndividualExpense(int id)
        {
            Expenses found = _context.Expenses.Find(id);
            return View(found);
        }
       
        //categories for expenses: Utilities, Advertising, Vehicle, Employee Benefits, Meals/Entertainment, Supplies, Depreciation, Insurance, Wages, Travel, Rent/Lease, Other
        [HttpGet]
        public IActionResult AddExpense()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddExpense(Expenses expense)
        {
            if(ModelState.IsValid)
            {
                _context.Expenses.Add(expense);
                _context.SaveChanges();
                Cash c = new Cash();
                c.Withdrawl = expense.Amount;
                c.TransDate = expense.PaymentDate;
                c.ExpenseId = expense.ExpId;
                _context.Cash.Add(c);
                _context.SaveChanges();
            }
            return RedirectToAction("ExpenseIndex");
        }
        [HttpGet]
        public IActionResult UpdateExpense(int id)
        {
            Expenses found = _context.Expenses.Find(id);
            if (found == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                return View(found);
            }
        }
        [HttpPost]
        public IActionResult UpdateExpense(Expenses updatedExpense)
        {
            Expenses old = _context.Expenses.Find(updatedExpense.ExpId);
            old.Description = updatedExpense.Description;
            old.PaymentDate = updatedExpense.PaymentDate;
            old.Amount = updatedExpense.Amount;
            _context.Entry(old).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(old);
            _context.SaveChanges();
            return RedirectToAction("ExpenseIndex");

        }
        

    }
}
