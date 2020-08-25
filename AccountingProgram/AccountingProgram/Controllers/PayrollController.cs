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
            found.SalaryPayment += salariesPay;
            found.SalaryBalance = found.SalariesPay - found.SalaryPayment;
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
                exp.WageId = wage.WageId;
                exp.PaymentDate = wage.PayDate;
                exp.Description = "Salary Expense";
                exp.Amount = wage.GrossPay;

                Expenses exp2 = new Expenses();
                exp.WageId = wage.WageId;
                exp2.Description = "Payroll Tax";
                exp2.PaymentDate = wage.PayDate;
                exp2.Amount = ptp.EmployerFicass + ptp.EmployerFicamed + ptp.Futataxes + ptp.Sutataxes;

                Expenses exp3 = new Expenses();
                exp.WageId = wage.WageId;
                exp3.PaymentDate = wage.PayDate;
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
        public IActionResult WageDetails(int id)
        {
            Wages found = _context.Wages.Find(id);
            if(found != null)
            {
                return View(found);
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        [HttpGet]
        public IActionResult UpdateWage(int id)
        {
            Wages found = _context.Wages.Find(id);
            return View(found);
        }
        [HttpPost]
        public IActionResult UpdateWage(Wages updatedWage)
        {
            Wages old = _context.Wages.Find(updatedWage.WageId);
            if (ModelState.IsValid)
            {
                old.PayDate = updatedWage.PayDate;
                old.GrossPay = updatedWage.GrossPay;
                old.Sstax = updatedWage.Sstax;
                old.MedicareTax = updatedWage.MedicareTax;
                old.FedIncTax = updatedWage.FedIncTax;
                old.StateIncTax = updatedWage.StateIncTax;
                old.LocalIncTax = updatedWage.LocalIncTax;
                old.InsuranceDed = updatedWage.InsuranceDed;
                old.SavingsDed = updatedWage.SavingsDed;
                old.NetPay = updatedWage.NetPay;
                _context.Entry(old).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(old);
                _context.SaveChanges();

                PayrollPayable ppfound = _context.PayrollPayable.First(x => x.WageId == updatedWage.WageId);
                ppfound.PayableDate = updatedWage.PayDate;
                ppfound.WageId = updatedWage.WageId;
                ppfound.MedicalIns = updatedWage.InsuranceDed;
                ppfound.SalariesPay = updatedWage.NetPay;
                ppfound.SalaryBalance = updatedWage.NetPay;
                ppfound.EmployerMedIns = (updatedWage.InsuranceDed ?? 0) * (decimal)1.50;
                ppfound.BenefitsTotal = ppfound.MedicalIns + ppfound.EmployerMedIns;
                ppfound.SavingsDed = updatedWage.SavingsDed;
                ppfound.BenefitsBalance = ppfound.BenefitsTotal;
                ppfound.SavingsDedBalance = ppfound.SavingsDed;
                _context.Entry(ppfound).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(ppfound);
                _context.SaveChanges();

                PayrollTaxesPayable ptpfound = _context.PayrollTaxesPayable.First(x => x.PayrollPayId == ppfound.PayrollId);
                ptpfound.PayrollDate = updatedWage.PayDate;
                ptpfound.PayrollPayId = ppfound.PayrollId;
                ptpfound.FedInTaxWithheld = updatedWage.FedIncTax;
                ptpfound.StateIncTaxWithheld = updatedWage.StateIncTax;
                ptpfound.LocalIncomeTaxWithheld = updatedWage.LocalIncTax;
                ptpfound.Ficasstax = updatedWage.Sstax;
                ptpfound.Ficamed = updatedWage.MedicareTax;
                ptpfound.EmployerFicass = updatedWage.Sstax;
                ptpfound.EmployerFicamed = updatedWage.MedicareTax;
                ptpfound.Futataxes = (updatedWage.GrossPay ?? 0) * (decimal)0.008;
                ptpfound.Sutataxes = (updatedWage.GrossPay ?? 0) * (decimal)0.027;
                ptpfound.Amount = ptpfound.FedInTaxWithheld + ptpfound.StateIncTaxWithheld + ptpfound.LocalIncomeTaxWithheld + ptpfound.Ficasstax +
                    ptpfound.Ficamed + ptpfound.EmployerFicass + ptpfound.EmployerFicamed + ptpfound.Futataxes + ptpfound.Sutataxes;
                ptpfound.Balance = ptpfound.Amount;
                _context.Entry(ptpfound).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(ptpfound);
                _context.SaveChanges();

                Expenses foundexp1 = _context.Expenses.First(x => (x.WageId == updatedWage.WageId) && (x.Description == "Salary Expense"));
                foundexp1.PaymentDate = updatedWage.PayDate;
                foundexp1.Amount = updatedWage.GrossPay;
                _context.Update(foundexp1);
                _context.SaveChanges();

                Expenses foundexp2 = _context.Expenses.First(x => (x.WageId == updatedWage.WageId) && (x.Description == "Payroll Tax"));
                foundexp2.PaymentDate = updatedWage.PayDate;
                foundexp2.Amount = ptpfound.EmployerFicass + ptpfound.EmployerFicamed + ptpfound.Futataxes + ptpfound.Sutataxes;
                _context.Update(foundexp2);
                _context.SaveChanges();

                Expenses foundexp3 = _context.Expenses.First(x => (x.WageId == updatedWage.WageId) && (x.Description == "EmployeeBenefits"));
                foundexp3.PaymentDate = updatedWage.PayDate;
                foundexp3.Amount = ppfound.EmployerMedIns;
                _context.Update(foundexp3);
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
            found.PaymentAmount += paymentAmount;
            found.Balance = found.Amount - found.PaymentAmount;
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
        public IActionResult MakeBenefitsPayment(int payrollId, DateTime benefitPaymentDate, decimal? benefitsPayment, decimal? savingsPayment )
        {
            PayrollPayable found = _context.PayrollPayable.Find(payrollId);
            found.PaymentDate = benefitPaymentDate;
            found.BenefitsPayment += benefitsPayment;
            found.SavingsPayment += savingsPayment;
            found.BenefitsBalance = found.BenefitsTotal - found.BenefitsPayment;
            found.SavingsDedBalance = found.SavingsDed - found.SavingsPayment;
            _context.Update(found);
            _context.SaveChanges();

            Cash c = new Cash();
            c.TransDate = benefitPaymentDate;
            c.Withdrawl = (benefitsPayment ?? 0) + (savingsPayment ?? 0);
            c.PayrollId = payrollId;
            _context.Cash.Add(c);
            _context.SaveChanges();

            return RedirectToAction("PayBenefits");
        }
    }
}
