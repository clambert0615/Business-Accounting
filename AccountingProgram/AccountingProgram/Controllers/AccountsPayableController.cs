using System;
using System.Collections.Generic;
using System.Linq;
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
            
            return View(apie);
        }
        public IActionResult MakePayment(int id, int amount)
        {
            AccountsPayable payable = _context.AccountsPayable.Find(id);
            payable.PaymentAmount = amount;
            payable.Balance -= amount;
            payable.PaymentDate = DateTime.Today;
            Cash c = new Cash();
            c.Withdrawl = amount;
            _context.Cash.Add(c);
            _context.Entry(payable).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(payable);
            _context.SaveChanges();

            return RedirectToAction("GetAllPayables");
        }
    }
}
