using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class ChartofAccountsController : Controller
    {
        private readonly AccountingAPIDbContext _context;
        public BalanceSheet bs = new BalanceSheet { Cash = new Cash(), Payable = new AccountsPayable(),
            Receivable = new AccountsReceivable(), Expense = new Expenses(), Inventory = new Inventory(),
            LTAssets = new LongTermAssets(), LTLiabilities = new LongTermLiabilities(), Equity = new OwnersEquity(),
            PayrollPay = new PayrollPayable(), PayTaxesPayable = new PayrollTaxesPayable()};
        public IncomeStatement inc = new IncomeStatement { Sales = new Sales() };
       
        public ChartofAccountsController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult ChartofAccountsIndex()
        {
            return View();
        }
        public IActionResult BalanceSheet()
        {
            List<Cash> cashList = _context.Cash.ToList();
            decimal bal = 0;
            foreach (Cash c in cashList)
            {
                c.Balance = bal + (c.Deposit ?? 0) - (c.Withdrawl ?? 0);
                bal = (c.Balance ?? 0);
            }
            bs.Cash.Balance = bal;
            List<AccountsPayable> payableList = _context.AccountsPayable.ToList();
            decimal apbal = 0;
            foreach (AccountsPayable ap in payableList)
            {
                apbal += (ap.Balance ?? 0);
            }
            bs.Payable.Balance = apbal;
            List<PayrollPayable> payList = _context.PayrollPayable.ToList();
            decimal ppbal = 0;
            foreach(PayrollPayable pp in payList)
            {
                ppbal += (pp.SalaryBalance ?? 0);
            }
            bs.PayrollPay.SalaryBalance = ppbal;
            List<PayrollTaxesPayable> ptpList = _context.PayrollTaxesPayable.ToList();
            decimal ptpbal = 0;
            foreach(PayrollTaxesPayable ptp in ptpList)
            {
                ptpbal += (ptp.Balance ?? 0);
            }
            bs.PayTaxesPayable.Balance = ptpbal; 
            List<AccountsReceivable> arList = _context.AccountsReceivable.ToList();
            decimal arbal = 0;
            foreach (AccountsReceivable ar in arList)
            {
                arbal += (ar.Balance ?? 0);
            }
            bs.Receivable.Balance = arbal;
            List<Expenses> expList = _context.Expenses.ToList();
            decimal expbal = 0;
            foreach (Expenses e in expList)
            {
                expbal += (e.Amount ?? 0);
            }
            bs.Expense.Amount = expbal;
            List<Inventory> invList = _context.Inventory.ToList();
            decimal invbal = 0;
            foreach (Inventory i in invList)
            {
                invbal += (i.Price ?? 0) * (i.Quantity ?? 0);
            }
            bs.Inventory.Price = invbal;

            List<LongTermAssets> ltaList = _context.LongTermAssets.ToList();
            var buildings = ltaList.Where(s => s.Description == "Buildings and Improvements").Select(s => s.Balance).ToList();
            decimal buildbal = 0;
            foreach(decimal d in buildings)
            {
                buildbal += d;
            }
            bs.Buildings = buildbal;

            var equipment = ltaList.Where(s => s.Description == "Equipment").Select(s => s.Balance).ToList();
            decimal equipbal = 0;
            foreach(decimal e in equipment)
            {
                equipbal += e;
            }
            bs.Equipment = equipbal;

            var landList = ltaList.Where(s => s.Description == "Land").Select(s => s.Balance).ToList();
            decimal landbal = 0;
            foreach (decimal land in landList)
            {
                landbal += land;
            }
            bs.Land = landbal;
            List<LongTermLiabilities> ltlList = _context.LongTermLiabilities.ToList();
            var loanList = ltlList.Where(s => s.Ltldescription == "Loan Payable").Select(s => s.Ltlbalance).ToList();
            decimal loanbal = 0;
            foreach(decimal loan in loanList)
            {
                loanbal += loan;
            }
            bs.LoanBalance = loanbal;

            bs.MarketableSecurities = GetSTAssetBalance("Marketable Securities");
            bs.PrepaidInsurance = GetSTAssetBalance("Prepaid Insurance");
            bs.PrepaidRent = GetSTAssetBalance("Prepaid Rent");
            bs.OtherPrepaidExpense = GetSTAssetBalance("Other Prepaid Expense");
            bs.OtherCurrentAsset = GetSTAssetBalance("Other Current Asset");

            bs.ShortTermDebt = GetSTLiabilityBalance("Short Term Debt");
            bs.TaxesPayable = GetSTLiabilityBalance("Taxes Payable");
            bs.UnearnedRevenue = GetSTLiabilityBalance("Unearned Revenue");
            bs.AccruedExpenses = GetSTLiabilityBalance("Accrued Expenses");
            bs.CurrentLTDebt = GetSTLiabilityBalance("Current Portion of Long Term Debt");
            bs.OtherCurrentLiabiltiy = GetSTLiabilityBalance("Other Current Liabilities");
            

            bs.TotalCurrentAssets = (decimal)(bs.Cash.Balance + bs.Inventory.Price + bs.Receivable.Balance + 
                bs.MarketableSecurities + bs.PrepaidInsurance + bs.PrepaidRent + bs.OtherCurrentAsset + bs.OtherPrepaidExpense);
            bs.TotalPPE = (decimal)(bs.Buildings + bs.Equipment + bs.Land);
            bs.TotalAssets = (decimal)(bs.TotalCurrentAssets + bs.TotalPPE);
            bs.CurrentLiabilities = (decimal)(bs.Payable.Balance + bs.PayrollPay.SalaryBalance + bs.PayTaxesPayable.Balance
                + bs.ShortTermDebt + bs.TaxesPayable + bs.UnearnedRevenue + bs.AccruedExpenses + bs.CurrentLTDebt
                + bs.OtherCurrentLiabiltiy); 
            bs.TotalLiabilities = (decimal)(bs.CurrentLiabilities + bs.LoanBalance);
            bs.Equity.Amount = bs.TotalAssets - bs.TotalLiabilities;
            bs.TotalLiabilitiesEquity =(decimal)(bs.TotalLiabilities + bs.Equity.Amount);
            return View(bs);

        }

        public IActionResult IncomeStatement()
        {
            List<Sales> salesList = _context.Sales.ToList();
            decimal salebal = 0;
            foreach(Sales s in salesList)
            {
                salebal += (s.Subtotal ?? 0);
            }
            inc.Sales.Subtotal = salebal;
            //still need to add other revenue and then a total revenue
            inc.TotalRevenue = salebal;

            List<Inventory> invList = _context.Inventory.ToList();
            decimal beginningInv = 0;
            decimal purchases = 0;
            decimal endingInv = 0;
            foreach(Inventory i in invList)
            {
                purchases += (((i.Price ?? 0) * (i.Received ?? 0)) + ((i.Price ?? 0) * (i.BackOrdered ?? 0)));
                endingInv += ((i.Price ?? 0) * (i.Quantity ?? 0));
            }
            inc.CostofGoodsSold = beginningInv + purchases - endingInv;
            inc.GrossProfit = inc.TotalRevenue - inc.CostofGoodsSold;

            
            inc.Advertising = GetExpenseBalance("Advertising");
            inc.Depreciation = GetExpenseBalance("Depreciation");
            inc.EmployeeBenefits = GetExpenseBalance("Employee Benefits");
            inc.Insurance = GetExpenseBalance("Insurance");
            inc.Interest = GetExpenseBalance("Interest");
            inc.Meals = GetExpenseBalance("Meals / Entertainment");
            inc.Supplies = GetExpenseBalance("Supplies");
            inc.Rent = GetExpenseBalance("Rent / Lease");
            inc.Travel = GetExpenseBalance("Travel");
            inc.Utilities = GetExpenseBalance("Utilities");
            inc.Vehicle = GetExpenseBalance("Vehicle");
            inc.Wages = GetExpenseBalance("Wages");
            inc.Other = GetExpenseBalance("Other");
            inc.IncomeTaxExpense = GetExpenseBalance("Income Tax");
            inc.PayrollTax = GetExpenseBalance("Payroll Tax");
            

            inc.Totalexpenses = inc.Advertising + inc.Depreciation + inc.EmployeeBenefits + inc.Insurance + inc.PayrollTax
                + inc.Interest + inc.Meals + inc.Supplies + inc.Rent + inc.Travel + inc.Utilities + inc.Vehicle + inc.Wages + inc.Other;

            inc.IncomeBeforeTax = inc.GrossProfit - inc.Totalexpenses;

            inc.NetIncome = inc.IncomeBeforeTax - inc.IncomeTaxExpense;

            return View(inc);
        }

        public decimal GetExpenseBalance(string description)
        {
            List<Expenses> expenseList = _context.Expenses.ToList();
            var expList = expenseList.Where(s => s.Description == description).Select(s => s.Amount).ToList();
            decimal exp = 0;
            foreach (decimal ie in expList)
            {
                exp += ie;
            }
            return exp;
        }
        public decimal GetSTAssetBalance(string description)
        {
            List<Assets> stassetList = _context.Assets.ToList();
            var assetList = stassetList.Where(s => s.Description == description).Select(s => s.Cost).ToList();
            decimal sta = 0;
            foreach(decimal st in assetList)
            {
                sta += st;
            }
            return sta;
        }
        public decimal GetSTLiabilityBalance(string description)
        {
            List<Stliabilities> stliabiityList = _context.Stliabilities.ToList();
            var liabilityList = stliabiityList.Where(s => s.Description == description).Select(s => s.Balance).ToList();
            decimal stl = 0;
            foreach(decimal sl in liabilityList)
            {
                stl += sl;
            }
            return stl;
        }
    }
}
