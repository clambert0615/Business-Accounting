using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class AccountsReceivableController : Controller
    {
        private AccountingAPIDbContext _context;
        public AccRecReceiptsView arv = new AccRecReceiptsView();

       //AccountsReceivable can be updated in the Sales controller / Invoice methods
       //Receipts can be updated here in the UpdateReceipt method

        public AccountsReceivableController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult ARIndex()
        {
            List<AccountsReceivable> arList = _context.AccountsReceivable.ToList();            
            return View(arList);
        }
       public IActionResult IndividualReceivable(int id)
       {
           AccountsReceivable ar = _context.AccountsReceivable.Find(id);
           arv.AR = ar;
            arv.Receipts = _context.Arreceipts.Where(x => x.AccountsRecId == ar.Id).ToList();
            return View(arv);
      }
        public IActionResult GetReceipt(int id, decimal amount)
        {
            AccountsReceivable ar = _context.AccountsReceivable.Find(id);
            ar.CashAmount = amount;
            ar.Balance -= amount;
            _context.Entry(ar).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(ar);
            _context.SaveChanges();
            Cash c = new Cash();
            c.TransDate = DateTime.Today;
            c.Deposit = amount;
            _context.Cash.Add(c);
            _context.SaveChanges();
            Arreceipts a = new Arreceipts();
            a.AccountsRecId = ar.Id;
            a.ReceiptDate = DateTime.Today;
            a.Amount = amount;
            a.CashId = c.Id;
            _context.Arreceipts.Add(a);
            _context.SaveChanges();

            return RedirectToAction("ARIndex");
        }
        [HttpGet]
        public IActionResult UpdateReceipt(int id)
        {
            Arreceipts found = _context.Arreceipts.Find(id);
            return View(found);
        }

        [HttpPost]
        public IActionResult UpdateReceipt(Arreceipts updatedReceipt)
        {
            Arreceipts oldReceipt = _context.Arreceipts.First(x => x.ArreciptsId == updatedReceipt.ArreciptsId);
            AccountsReceivable oldar = _context.AccountsReceivable.First(x => x.Id == oldReceipt.AccountsRecId);
            Cash oldCash = _context.Cash.First(x => x.Id == oldReceipt.CashId);
            oldReceipt.ReceiptDate = updatedReceipt.ReceiptDate;
            oldReceipt.Amount = updatedReceipt.Amount;
            _context.Entry(oldReceipt).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldReceipt);
            _context.SaveChanges();
            oldar.Balance += oldar.CashAmount;
            oldar.CashAmount = updatedReceipt.Amount;
            oldar.Balance -= updatedReceipt.Amount;
            _context.Entry(oldar).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldar);
            _context.SaveChanges();
            oldCash.TransDate = updatedReceipt.ReceiptDate;
            oldCash.Deposit = updatedReceipt.Amount;
            _context.Entry(oldCash).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldCash);
            _context.SaveChanges();

            return RedirectToAction("ARIndex");
            
        }
        public IActionResult DeleteReceipt(int id)
        {
            Arreceipts found = _context.Arreceipts.Find(id);
            AccountsReceivable foundAR = _context.AccountsReceivable.First(x => x.Id == found.AccountsRecId);
            Cash foundCash = _context.Cash.First(x => x.Id == found.CashId);
            if (found != null)
            {
                foundAR.Balance += foundAR.CashAmount;
                foundAR.CashAmount = 0;
                _context.Entry(foundAR).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(foundAR);
                _context.SaveChanges();

                _context.Cash.Remove(foundCash);
                _context.Arreceipts.Remove(found);
                _context.SaveChanges();
                return RedirectToAction("ARIndex");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
    }
}
