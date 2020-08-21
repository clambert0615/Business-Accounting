using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

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
            List <Wages> wageList = _context.Wages.ToList();
            return View(wageList);
        }

        [HttpGet]
        public IActionResult AddWages()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddWages(Wages wage)
        {
            if(ModelState.IsValid)
            {
                _context.Wages.Add(wage);
                _context.SaveChanges();

                PayrollPayable pp = new PayrollPayable();
                pp.FedIncTaxWithheld = wage.FedIncTax;
                pp.StateIncTaxWithheld = wage.StateIncTax;
                pp.Ficasstax = wage.Sstax;
                pp.Ficamed = wage.MedicareTax;
                pp.MedicalIns = wage.InsuranceDed;
                pp.SalariesPay = wage.NetPay;
                pp.EmployerFicass = wage.Sstax;
                pp.EmployerMed = wage.MedicareTax;
                pp.Futataxes = (wage.GrossPay ?? 0) * (decimal)0.008;
                pp.Sutataxes = (wage.GrossPay ?? 0) * (decimal)0.027;
                _context.PayrollPayable.Add(pp);
                _context.SaveChanges();



                Expenses exp = new Expenses();
                exp.PaymentDate = wage.PayDate;
                exp.Description = "Salary Expense";
                exp.Amount = wage.GrossPay;

                Expenses exp2 = new Expenses();
                exp2.Description = "Payroll Tax"; 
                exp2.PaymentDate = wage.PayDate;
                exp2.Amount = pp.EmployerFicass + pp.EmployerMed + pp.Futataxes + pp.Sutataxes;

                _context.Expenses.Add(exp);
                _context.Expenses.Add(exp2);
                _context.SaveChanges();

                return RedirectToAction("PayrollIndex");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
    }
}
