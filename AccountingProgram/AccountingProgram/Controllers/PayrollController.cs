using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace AccountingProgram.Controllers
{
    public class PayrollController : Controller
    {
        private readonly AccountingAPIDbContext _context;

        public PayrollController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult PayrollIndex()
        {
            List<Wages> wageList = _context.Wages.ToList();

            return View(wageList);
        }

        public IActionResult IndividualPay(int id)
        {
            PayrollPayable found = _context.PayrollPayable.Find(id);
            List<PayrollTaxesPayable> ptpList = _context.PayrollTaxesPayable.Where(x => x.PayrollPayId == id).ToList();

            if (found != null)
            {
                found.PayrollTaxesPayable = ptpList;
                return View(found);
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }

        }
        public IActionResult PaySalary(int payrollId, decimal salariesPay, DateTime paymentDate)
        {
            PayrollPayable found = _context.PayrollPayable.Find(payrollId);
            found.SalaryPayment = salariesPay;
            found.SalaryBalance -= salariesPay;
            found.PaymentDate = paymentDate;
            _context.Update(found);
            _context.SaveChanges();

            Cash c = new Cash();
            c.PayrollId = found.PayrollId;
            c.TransDate = paymentDate;
            c.Withdrawl = salariesPay;
            _context.Cash.Add(c);
            _context.SaveChanges();

            return RedirectToAction("PayrollIndex");
        }

        [HttpGet]
        public IActionResult AddWages()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddWages(Wages wage)
        {
            if (ModelState.IsValid)
            {
                _context.Wages.Add(wage);
                _context.SaveChanges();

                PayrollPayable pp = new PayrollPayable();
                pp.PayableDate = wage.PayDate;
                pp.WageId = wage.WageId;
                pp.MedicalIns = wage.InsuranceDed;
                pp.SalariesPay = wage.NetPay;
                pp.SalaryBalance = wage.NetPay;
                pp.EmployerMedIns = (wage.InsuranceDed ?? 0) * (decimal)1.50;
                pp.BenefitsTotal = pp.MedicalIns + pp.EmployerMedIns;
                pp.SavingsDed = wage.SavingsDed;
                pp.BenefitsBalance = pp.BenefitsTotal;
                pp.SavingsDedBalance = pp.SavingsDed;
                _context.PayrollPayable.Add(pp);
                _context.SaveChanges();

                PayrollTaxesPayable ptp = new PayrollTaxesPayable();
                ptp.PayrollDate = wage.PayDate;
                ptp.PayrollPayId = pp.PayrollId;
                ptp.FedInTaxWithheld = wage.FedIncTax;
                ptp.StateIncTaxWithheld = wage.StateIncTax;
                ptp.LocalIncomeTaxWithheld = wage.LocalIncTax;
                ptp.Ficasstax = wage.Sstax;
                ptp.Ficamed = wage.MedicareTax;
                ptp.EmployerFicass = wage.Sstax;
                ptp.EmployerFicamed = wage.MedicareTax;
                ptp.Futataxes = (wage.GrossPay ?? 0) * (decimal)0.008;
                ptp.Sutataxes = (wage.GrossPay ?? 0) * (decimal)0.027;
                ptp.Amount = ptp.FedInTaxWithheld + ptp.StateIncTaxWithheld +ptp.LocalIncomeTaxWithheld + ptp.Ficasstax +
                    ptp.Ficamed + ptp.EmployerFicass + ptp.EmployerFicamed + ptp.Futataxes + ptp.Sutataxes;
                ptp.Balance = ptp.Amount;
                _context.PayrollTaxesPayable.Add(ptp);
                _context.SaveChanges();


                Wages w = _context.Wages.Find(wage.WageId);
                w.PayrollPayableId = pp.PayrollId;
                _context.Update(w);
                _context.SaveChanges();


                Expenses exp = new Expenses();
                exp.PaymentDate = wage.PayDate;
                exp.Description = "Salary Expense";
                exp.Amount = wage.GrossPay;

                Expenses exp2 = new Expenses();
                exp2.Description = "Payroll Tax";
                exp2.PaymentDate = wage.PayDate;
                exp2.Amount = ptp.EmployerFicass + ptp.EmployerFicamed + ptp.Futataxes + ptp.Sutataxes;

                Expenses exp3 = new Expenses();
                exp3.Description = "EmployeeBenefits";
                exp3.Amount = pp.EmployerMedIns;

                _context.Expenses.Add(exp);
                _context.Expenses.Add(exp2);
                _context.Expenses.Add(exp3);
                _context.SaveChanges();

                return RedirectToAction("PayrollIndex");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        public IActionResult PayTaxes()
        {
            List<PayrollTaxesPayable> ptpList = _context.PayrollTaxesPayable.ToList();
            return View(ptpList);
        }
        [HttpGet]
        public IActionResult MakeTaxPayment(int id)
        {
            PayrollTaxesPayable found = _context.PayrollTaxesPayable.Find(id);
            if (found != null)
            {
                return View(found);
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        public IActionResult MakeTaxPayment(int payTaxesId, DateTime paymentDate, decimal paymentAmount)
        {
            PayrollTaxesPayable found = _context.PayrollTaxesPayable.Find(payTaxesId);
            found.PaymentDate = paymentDate;
            found.PaymentAmount = paymentAmount;
            found.Balance -= paymentAmount;
            _context.Update(found);
            _context.SaveChanges();

            Cash c = new Cash();
            c.TransDate = paymentDate;
            c.PayrollId = found.PayrollPayId;
            c.Withdrawl = paymentAmount;
            _context.Cash.Add(c);
            _context.SaveChanges();

            return RedirectToAction("PayTaxes");

        }
        
        public IActionResult PayBenefits()
        {
            List<PayrollPayable> ppList = _context.PayrollPayable.ToList();
            return View(ppList);
        }
        [HttpGet]
        public IActionResult MakeBenefitsPayment(int id)
        {
            PayrollPayable found = _context.PayrollPayable.Find(id);
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
        public IActionResult MakeBenefitsPayment(int payrollId, DateTime paymentDate, decimal? benefitsPayment, decimal? savingsPayment )
        {
            PayrollPayable found = _context.PayrollPayable.Find(payrollId);
            found.PaymentDate = paymentDate;
            found.BenefitsPayment = benefitsPayment;
            found.SavingsPayment = savingsPayment;
            found.BenefitsBalance -= benefitsPayment;
            found.SavingsDedBalance -= savingsPayment;
            _context.Update(found);
            _context.SaveChanges();

            Cash c = new Cash();
            c.TransDate = paymentDate;
            c.Withdrawl = (benefitsPayment ?? 0) + (savingsPayment ?? 0);
            c.PayrollId = payrollId;
            _context.Cash.Add(c);
            _context.SaveChanges();

            return RedirectToAction("PayBenefits");
        }
    }
}
