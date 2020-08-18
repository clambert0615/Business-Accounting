using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class AccumulatedDepreciationController : Controller
    {
        private readonly AccountingAPIDbContext _context;
        public AccumulatedDepreciationController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult AddAccumulatedDepreciation(AccumulatedDepreciation ad)
        {
            Expenses expense = new Expenses();
            expense.Description = "Depreciation";
            expense.Amount = ad.Amount;
            _context.Expenses.Add(expense);
            _context.SaveChanges();

            ad.ExpenseId = expense.ExpId;
            _context.AccumulatedDepreciation.Add(ad);
            _context.SaveChanges();

            LongTermAssets oldlta = _context.LongTermAssets.Find(ad.LongTermAssetId);
            oldlta.Balance -= ad.Amount;
            oldlta.LifeRemainig -= 1;
            _context.Entry(oldlta).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldlta);
            _context.SaveChanges();

            return RedirectToAction("LongTermAssetIndex", "LongTermAssets");
        }
    }
}
