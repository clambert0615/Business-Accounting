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
            oldlta.LifeRemaining -= 1;
            _context.Entry(oldlta).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldlta);
            _context.SaveChanges();

            return RedirectToAction("LongTermAssetIndex", "LongTermAssets");
        }
        [HttpGet]
        public IActionResult UpdateAD(int id)
        {
            AccumulatedDepreciation found = _context.AccumulatedDepreciation.Find(id);
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
        public IActionResult UpdateAD(AccumulatedDepreciation updatedAD)
        {
            AccumulatedDepreciation oldAD = _context.AccumulatedDepreciation.Find(updatedAD.AccDepId);
            if (ModelState.IsValid)
            {
                LongTermAssets oldlta = _context.LongTermAssets.Find(updatedAD.LongTermAssetId);
                oldlta.Balance = oldlta.Balance + oldAD.Amount - updatedAD.Amount;
           
                _context.Entry(oldlta).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(oldlta);
                _context.SaveChanges();

                oldAD.Description = updatedAD.Description;
                oldAD.Amount = updatedAD.Amount;
                _context.Entry(oldAD).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(oldAD);
                _context.SaveChanges();

                Expenses oldExp = _context.Expenses.First(x => x.AccDepId ==updatedAD.AccDepId);
                oldExp.Amount = updatedAD.Amount;
                _context.Entry(oldExp).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(oldExp);
                _context.SaveChanges();

                return RedirectToAction("LongTermAssetIndex", "LongTermAssets");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }

        }
    }
}
