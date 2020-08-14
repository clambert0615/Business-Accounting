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
        public AccRecReiptsView arv = new AccRecReiptsView();

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
            arv.Recipts = _context.Arreceipts.Where(x => x.AccountsRecId == ar.Id).ToList();
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
    }
}
