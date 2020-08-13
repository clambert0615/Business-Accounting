using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class SalesController : Controller
    {
        private readonly AccountingAPIDbContext _context;
        public InvoiceInventory ii = new InvoiceInventory { Invoice = new Invoice() };

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
            if (found == null)
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
                ap.Balance = sale.SalesTax;
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
            if (ModelState.IsValid)
            {
                invoice.InvoiceInventory = new List<InvoiceInventory>();
                foreach (var item in list)
                {
                    invoice.InvoiceInventory.Add(new InvoiceInventory { InventoryId = item.InvId, InvoiceId = invoice.InvoiceId, InventoryQty = item.Quantity });

                }
                _context.Invoice.Add(invoice);
                _context.SaveChanges();
                AccountsReceivable ar = new AccountsReceivable();
                ar.CustomerName = invoice.CustomerName;
                ar.DueDate = invoice.DueDate;
                ar.Amount = invoice.AmountDue;
                ar.Balance = invoice.AmountDue;
                _context.AccountsReceivable.Add(ar);
                _context.SaveChanges();
                Sales sale = new Sales();
                sale.AccRecId = ar.Id;
                sale.TransDate = invoice.InvDate;
                sale.Subtotal = invoice.Subtotal;
                sale.SalesTax = invoice.SalesTax;
                sale.Amount = invoice.AmountDue;
                sale.InvoiceId = invoice.InvoiceId;
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
            return RedirectToAction("SalesIndex");
        }
        [HttpGet]
        public IActionResult UpdateInvoice(int id)
        {
            Invoice found = _context.Invoice.First(x => x.InvoiceId == id);
            found.InvoiceInventory = _context.InvoiceInventory.Where(x => x.InvoiceId == found.InvoiceId).ToList();
            return View(found);
        }
        [HttpPost]
        public IActionResult UpdateInvoice(Invoice updatedInvoice)
        {
            Invoice old = _context.Invoice.First(x => x.InvoiceId == updatedInvoice.InvoiceId);
            old.InvDate = updatedInvoice.InvDate;
            old.DueDate = updatedInvoice.DueDate;
            old.CustomerName = updatedInvoice.CustomerName;
            old.StreetAddress = updatedInvoice.StreetAddress;
            old.City = updatedInvoice.City;
            old.State = updatedInvoice.State;
            old.Zip = updatedInvoice.Zip;
            old.Subtotal = updatedInvoice.Subtotal;
            old.SalesTax = updatedInvoice.SalesTax;
            old.AmountDue = updatedInvoice.AmountDue;
            _context.Entry(old).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(old);
            _context.SaveChanges();

            AccountsReceivable oldar = _context.AccountsReceivable.First(x => (x.CustomerName == old.CustomerName) && (x.Amount == old.AmountDue));
            oldar.CustomerName = updatedInvoice.CustomerName;
            oldar.DueDate = updatedInvoice.DueDate;
            oldar.Amount = updatedInvoice.AmountDue;
            _context.Entry(oldar).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldar);
            _context.SaveChanges();

            Sales oldSale = _context.Sales.First(x => x.InvoiceId == old.InvoiceId);
            oldSale.TransDate = updatedInvoice.InvDate;
            oldSale.Subtotal = updatedInvoice.Subtotal;
            oldSale.SalesTax = updatedInvoice.SalesTax;
            oldSale.Amount = updatedInvoice.AmountDue;
            _context.Entry(oldSale).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldSale);
            _context.SaveChanges();

            //AccountsPayable oldap = _context.AccountsPayable.First(x =>

            return RedirectToAction("SalesIndex");

        }
    }
}
