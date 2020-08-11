using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class AccountsPayableController : Controller
    {
        private readonly AccountingAPIDbContext _context;

        public AccPayInvExp apie = new AccPayInvExp();

        public AccountsPayableController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult GetAllPayables()
        {
            List<AccountsPayable> payablesList = _context.AccountsPayable.ToList();
            apie.PayableList = payablesList;
            return View(apie);
        }

        public IActionResult IndividualPayable(int id)
        {
            AccountsPayable foundPayable = _context.AccountsPayable.Find(id);
            apie.Payable = foundPayable;
            apie.PaymentList = _context.Payments.Where(x => x.PayId == foundPayable.PayableId).ToList();
                        
            return View(apie);
        }
        public IActionResult MakePayment(int id, decimal amount)
        {
            AccountsPayable payable = _context.AccountsPayable.Find(id);
            payable.PaymentAmount = amount;
            payable.Balance -= amount;
            payable.PaymentDate = DateTime.Today;
            Cash c = new Cash();
            c.Withdrawl = amount;
            c.TransDate = DateTime.Today;
            _context.Cash.Add(c);
            _context.SaveChanges();
            Payments p = new Payments();
            p.PayId = payable.PayableId;
            p.PayDate = DateTime.Today;
            p.Amount = amount;
            p.CashId = c.Id;
            _context.Payments.Add(p);
            _context.Entry(payable).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(payable);
            _context.SaveChanges();

            return RedirectToAction("GetAllPayables");
        }
        [HttpGet]
        public IActionResult AddPayable()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddPayable(AccountsPayable payable)
        {
            if (ModelState.IsValid)
            {
                _context.AccountsPayable.Add(payable);
                _context.SaveChanges();
            }
            return RedirectToAction("GetAllPayables", new { id = payable.PayableId });
        }
        [HttpGet]
        public IActionResult UpdatePayable(int id)
        {
            AccountsPayable foundPayable = _context.AccountsPayable.Find(id);
            if (foundPayable == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                return View(foundPayable);
            }
          
        }

        [HttpPost]
        public IActionResult UpdatePayable(AccountsPayable updatedPayable)
        {
            AccountsPayable oldPayable = _context.AccountsPayable.Find(updatedPayable.PayableId);
            oldPayable.VenId = updatedPayable.VenId;
            oldPayable.VendorName = updatedPayable.VendorName;
            oldPayable.DueDate = updatedPayable.DueDate;
            oldPayable.AmountDue = updatedPayable.AmountDue;
            oldPayable.Balance = updatedPayable.Balance;
            _context.Entry(oldPayable).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldPayable);
            _context.SaveChanges();
            return RedirectToAction("GetAllPayables");
        }
        
    }
}
