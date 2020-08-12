using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class SalesController : Controller
    {
        private readonly AccountingAPIDbContext _context;

        public SalesController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        //There is no delete on a sales item, instead a refund must be issued
        //Sales can't be updated, instead a correct must be issued
        //Subtotal field is the sales amount to use for balance sheet/ income tax purposes
        public IActionResult SalesIndex()
        {
            List<Sales> salesList = _context.Sales.ToList();
            return View(salesList);
        }
        public IActionResult IndividualSale(int id)
        {
            Sales found = _context.Sales.Find(id);
            if(found == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                return View(found);
            }
        }
        [HttpGet]
        public IActionResult AddCashSale()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCashSale(Sales sale, List<InvGrid> list)
        {
            if (ModelState.IsValid)
            {
                _context.Sales.Add(sale);
                _context.SaveChanges();
                Cash c = new Cash();
                c.Deposit = sale.CashAmount;
                c.TransDate = sale.TransDate;
                c.SalesId = sale.SalesId;
                _context.Cash.Add(c);
                _context.SaveChanges();
                AccountsPayable ap = new AccountsPayable();
                ap.VenId = 15;
                ap.VendorName = "State of Michigan (Sales Tax)";
                ap.DueDate = new DateTime(2021, 01, 01);
                ap.AmountDue = sale.SalesTax;
                _context.AccountsPayable.Add(ap);
                _context.SaveChanges();
                    
            }
            foreach (var item in list)
            {
                Inventory found = _context.Inventory.Find(item.InvId);
                found.Quantity -= item.Quantity;
                _context.Entry(found).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(found);
                _context.SaveChanges();

             
            }
            return RedirectToAction("SalesIndex");
        }

        //adding a sale/accounts receivable from an invoice
        [HttpGet]
        public IActionResult AddInvoice()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddInvoice(Invoice invoice, List<InvGrid> list)
        {
            if(ModelState.IsValid)
            {
                _context.Invoice.Add(invoice);
                _context.SaveChanges();
                AccountsReceivable ar = new AccountsReceivable();
                ar.CustomerName = invoice.CustomerName;
                ar.DueDate = invoice.DueDate;
                ar.AccRecAmount = invoice.AmountDue;
                _context.AccountsReceivable.Add(ar);
                _context.SaveChanges();
                Sales sale = new Sales();
                sale.AccRecId = ar.Id;
                sale.TransDate = invoice.InvDate;
                sale.Subtotal = invoice.Subtotal;
                sale.SalesTax = invoice.SalesTax;
                sale.Amount = invoice.AmountDue;
                _context.Sales.Add(sale);
                _context.SaveChanges();
                AccountsPayable ap = new AccountsPayable();
                ap.VenId = 15;
                ap.VendorName = "State of Michigan (Sales Tax)";
                ap.DueDate = new DateTime(2021, 01, 01);
                ap.AmountDue = sale.SalesTax;
                _context.AccountsPayable.Add(ap);
                _context.SaveChanges();
            }
            foreach (var item in list)
            {
                Inventory found = _context.Inventory.Find(item.InvId);
                found.Quantity -= item.Quantity;
                _context.Entry(found).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(found);
                _context.SaveChanges();


            }
            return RedirectToAction("SalesIndex" );
        }
    }
}
