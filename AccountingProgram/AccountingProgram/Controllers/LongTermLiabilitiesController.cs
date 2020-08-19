using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class LongTermLiabilitiesController : Controller
    {
        private readonly AccountingAPIDbContext _context;
        public LongTermLiabilitiesController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult LTLiabilitiesIndex()
        {
            List<LongTermLiabilities> ltlList = _context.LongTermLiabilities.ToList();
            return View(ltlList);
        }
        public IActionResult IndividualLTL(int id)
        {
            LongTermLiabilities found = _context.LongTermLiabilities.Find(id);
            List<Payments> paymentList = _context.Payments.Where(x => x.LongTermLiabId == found.LtliabilitiesId).ToList();
            found.Payments = paymentList;
            return View(found);
        }

        [HttpGet]
        public IActionResult AddLTLiability()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddLTLiability(LongTermLiabilities liability)
        {
            if (ModelState.IsValid)
            {
                _context.LongTermLiabilities.Add(liability);
                _context.SaveChanges();

                return RedirectToAction("LTLiabilitiesIndex");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }

        public IActionResult MakePayment(Payments payment)
        {
            if (ModelState.IsValid)
            {

                _context.Payments.Add(payment);
                _context.SaveChanges();

                Expenses expense = new Expenses();
                expense.Description = "Interest";
                expense.Amount = payment.InterestExpense;
                expense.PaymentDate = payment.PayDate;
                expense.PaymentId = payment.PaymentId;
                expense.LongTermLiabId = payment.LongTermLiabId;
                _context.Expenses.Add(expense);
                _context.SaveChanges();

                Cash c = new Cash();
                c.ExpenseId = expense.ExpId;
                c.TransDate = payment.PayDate;
                c.Withdrawl = payment.TotalAmount;
                _context.Cash.Add(c);
                _context.SaveChanges();

                LongTermLiabilities ltl = _context.LongTermLiabilities.First(x => x.LtliabilitiesId == payment.LongTermLiabId);
                ltl.PaymentId = payment.PaymentId;
                ltl.Ltlbalance -= payment.Amount;
                _context.Entry(ltl).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(ltl);
                _context.SaveChanges();

                return RedirectToAction("LTLiabilitiesIndex");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }

        }
        [HttpGet]
        public IActionResult UpdateLTL(int id)
        {
            LongTermLiabilities found = _context.LongTermLiabilities.Find(id);
            if (found != null)
            {
                return View(found);
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        [HttpPost]
        public IActionResult UpdateLTL(LongTermLiabilities updatedLTL)
        {
            LongTermLiabilities oldLTL = _context.LongTermLiabilities.Find(updatedLTL.LtliabilitiesId);
            oldLTL.Ltlitem = updatedLTL.Ltlitem;
            oldLTL.Ltldescription = updatedLTL.Ltldescription;
            oldLTL.OriginDate = updatedLTL.OriginDate;
            oldLTL.TotalAmount = updatedLTL.TotalAmount;
            oldLTL.TotalNumberofPayments = updatedLTL.TotalNumberofPayments;
            oldLTL.Ltlbalance = updatedLTL.Ltlbalance;
            _context.Entry(oldLTL).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldLTL);
            _context.SaveChanges();

            return RedirectToAction("LTLiabilitiesIndex");
        }
        [HttpGet]
        public IActionResult UpdatePayment(int id)
        {
            Payments payment = _context.Payments.Find(id);
            if(payment != null)
            {
                return View(payment);
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        [HttpPost]
        public IActionResult UpdatePayment(Payments updatedPayment)
        {
            Payments oldPay = _context.Payments.Find(updatedPayment.PaymentId);
            Expenses oldExpense = _context.Expenses.First(x => x.PaymentId == updatedPayment.PaymentId);
            Cash oldCash = _context.Cash.First(x => x.ExpenseId == oldExpense.ExpId);
            LongTermLiabilities oldLTL = _context.LongTermLiabilities.First(x => x.PaymentId == updatedPayment.PaymentId);

            oldLTL.Ltlbalance = oldLTL.Ltlbalance + oldPay.Amount - updatedPayment.Amount;
            _context.Entry(oldLTL).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldLTL);
            _context.SaveChanges();

            oldPay.PayDate = updatedPayment.PayDate;
            oldPay.Amount = updatedPayment.Amount;
            oldPay.InterestExpense = updatedPayment.InterestExpense;
            oldPay.TotalAmount = updatedPayment.TotalAmount;
            _context.Entry(oldPay).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldPay);
            _context.SaveChanges();

            oldExpense.Amount = updatedPayment.InterestExpense;
            oldExpense.PaymentDate = updatedPayment.PayDate;
            _context.Entry(oldExpense).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldExpense);
            _context.SaveChanges();

            oldCash.TransDate = updatedPayment.PayDate;
            oldCash.Withdrawl = updatedPayment.TotalAmount;
            _context.Entry(oldCash).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldCash);
            _context.SaveChanges();

            return RedirectToAction("LTLiabilitiesIndex");

        }
    }
}
