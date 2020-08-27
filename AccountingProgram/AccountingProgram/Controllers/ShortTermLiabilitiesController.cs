using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class ShortTermLiabilitiesController : Controller
    {
        private readonly AccountingAPIDbContext _context;
        public STLiabilityPayment stlp = new STLiabilityPayment();

        public ShortTermLiabilitiesController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult STLiabilityIndex()
        {
            List<Stliabilities> stlList = _context.Stliabilities.ToList();
            return View(stlList);
        }
        public IActionResult IndividualLiability(int id)
        {
            Stliabilities found = _context.Stliabilities.Find(id);
            stlp.STLiability = found;
            stlp.PaymentList = _context.Payments.Where(x => x.StliabilityId == found.StliabilityId).ToList();

            if(found != null)
            {
                return View(stlp);
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        [HttpGet]
        public IActionResult AddLiability()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddLiability(Stliabilities liability)
        {
            if(ModelState.IsValid)
            {
                _context.Stliabilities.Add(liability);
                _context.SaveChanges();

                if(liability.Description == "Unearned Revenue")
                {
                    Cash cash = new Cash();
                    cash.TransDate = (DateTime)liability.OriginDate;
                    cash.Deposit = liability.Amount;
                    cash.StliabilityId = liability.StliabilityId;
                    _context.Cash.Add(cash);
                    _context.SaveChanges();
                   
                }
                return RedirectToAction("STLiabilityIndex");

            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        [HttpGet]
        public IActionResult UpdateLiability(int id)
        {
            Stliabilities found = _context.Stliabilities.Find(id);
            return View(found);
        }
        [HttpPost]
        public IActionResult UpdateLiability(Stliabilities updatedLiability)
        {
            Stliabilities old = _context.Stliabilities.Find(updatedLiability.StliabilityId);
            old.OriginDate = updatedLiability.OriginDate;
            old.Item = updatedLiability.Item;
            old.Description = updatedLiability.Description;
            old.Amount = updatedLiability.Amount;
            old.Balance = updatedLiability.Balance;
            _context.Entry(old).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(old);
            _context.SaveChanges();

            if(updatedLiability.Description == "Unearned Revenue")
            {
                Cash found = _context.Cash.First(x => x.StliabilityId == updatedLiability.StliabilityId);
                found.TransDate = (DateTime)updatedLiability.OriginDate;
                found.Deposit = updatedLiability.Amount;
                _context.Update(found);
                _context.SaveChanges();

            }

            return RedirectToAction("STLiabilityIndex");
        }
        public IActionResult MakePayment(int id, decimal amount, DateTime payDate)
        {
            Cash cash = new Cash
            {
                Withdrawl = amount,
                TransDate = payDate
            };
            _context.Cash.Add(cash);
            _context.SaveChanges();

            Payments payment = new Payments
            {
                StliabilityId = id,
                Amount = amount,
                PayDate = payDate,
                CashId = cash.Id

            };
            _context.Payments.Add(payment);
            _context.SaveChanges();

            Stliabilities stl = _context.Stliabilities.Find(id);
            stl.Balance -= amount;
            
            _context.Update(stl);
            _context.SaveChanges();

            return RedirectToAction("STLiabilityIndex");
            
        }
        [HttpGet]
        public IActionResult EditPayment(int id)
        {
            Payments payment = _context.Payments.Find(id);
            return View(payment);
        }
        [HttpPost]
        public IActionResult EditPayment(int paymentId, decimal amount, DateTime payDate )
        {

            Payments oldpayment = _context.Payments.Find(paymentId);
            Stliabilities stl = _context.Stliabilities.First(x => x.StliabilityId == oldpayment.StliabilityId);
            stl.Balance = stl.Balance + oldpayment.Amount - amount;
            _context.Update(stl);
            _context.SaveChanges();

            oldpayment.Amount = amount;
            oldpayment.PayDate = payDate;
            _context.Update(oldpayment);
            _context.SaveChanges();

            Cash cash = _context.Cash.First(x => x.Id == oldpayment.CashId);
            cash.Withdrawl = amount;
            cash.TransDate = payDate;
            _context.Update(cash);
            _context.SaveChanges();

           

            return RedirectToAction("STLiabilityIndex");

        }

        public IActionResult NewAdjustingEntry(int stliabilityId, decimal amount, DateTime date)
        {
            Stliabilities found = _context.Stliabilities.Find(stliabilityId);
            found.Balance -= amount;
            found.PaymentDate = date;
            _context.Update(found);
            _context.SaveChanges();

            Sales sale = new Sales();
            sale.TransDate = date;
            sale.Amount = amount;
            sale.StliabilityId = found.StliabilityId;
            _context.Sales.Add(sale);
            _context.SaveChanges();

            return RedirectToAction("STLiabilityIndex");
        }

    }
}
