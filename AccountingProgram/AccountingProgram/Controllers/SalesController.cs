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
                sale.SalesInventory = new List<SalesInventory>();
                foreach (var item in list)
                {
                    sale.SalesInventory.Add(new SalesInventory { InventoryId = item.InvId, SalesId = sale.SalesId, InventoryQty = item.Quantity });

                }
            
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
                ar.InvoiceId = invoice.InvoiceId;
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
                ap.InvoiceId = invoice.InvoiceId;
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
        public IActionResult UpdateInvoice(Invoice updatedInvoice,  List<InvGrid> updatedInventory)
        {
           
            Invoice old = _context.Invoice.First(x => x.InvoiceId == updatedInvoice.InvoiceId);
            old.InvoiceInventory = _context.InvoiceInventory.Where(x => x.InvoiceId == old.InvoiceId).ToList();

            AccountsReceivable oldar = _context.AccountsReceivable.First(x => x.InvoiceId == updatedInvoice.InvoiceId);
            oldar.CustomerName = updatedInvoice.CustomerName;
            oldar.DueDate = updatedInvoice.DueDate;
            oldar.Amount = updatedInvoice.AmountDue;
            _context.Entry(oldar).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldar);
            _context.SaveChanges();

            foreach (var item in old.InvoiceInventory)
            {
                Inventory found = _context.Inventory.Find(item.InventoryId);
                found.Quantity += item.InventoryQty;
            }
            old.InvoiceInventory.Clear();
            foreach (var item in updatedInventory)
            {
                updatedInvoice.InvoiceInventory.Add(new InvoiceInventory { InventoryId = item.InvId, InvoiceId = updatedInvoice.InvoiceId, InventoryQty = item.Quantity });
                Inventory update = _context.Inventory.First(x => x.InvId == item.InvId);
                update.Quantity -= item.Quantity;
            }
            old.InvoiceInventory = updatedInvoice.InvoiceInventory;
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

       
            Sales oldSale = _context.Sales.First(x => x.InvoiceId == old.InvoiceId);
            oldSale.TransDate = updatedInvoice.InvDate;
            oldSale.Subtotal = updatedInvoice.Subtotal;
            oldSale.SalesTax = updatedInvoice.SalesTax;
            oldSale.Amount = updatedInvoice.AmountDue;
            _context.Entry(oldSale).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldSale);
            _context.SaveChanges();


             AccountsPayable oldap = _context.AccountsPayable.First(x => x.InvoiceId == old.InvoiceId);
            oldap.AmountDue = updatedInvoice.SalesTax;
            _context.Entry(oldap).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldap);
            _context.SaveChanges();

            return RedirectToAction("SalesIndex");

        }
        
        public IActionResult CashSalesReturnId()
        {
            return View();
        }
        
        public IActionResult CashSalesReturn(int SalesId)
        {
            Sales found = _context.Sales.Find(SalesId);
            found.SalesInventory = _context.SalesInventory.Where(x => x.SalesId == found.SalesId).ToList();
            return View(found);
        }
        
        public IActionResult CashSalesReturnUpdate(Sales updatedSale, List<InvGrid> updatedInventory)
        {
            Sales old = _context.Sales.First(x => x.SalesId == updatedSale.SalesId);
            old.SalesInventory = _context.SalesInventory.Where(x => x.SalesId == old.SalesId).ToList();
            foreach (var item in old.SalesInventory)
            {
                Inventory found = _context.Inventory.Find(item.InventoryId);
                found.Quantity += item.InventoryQty;
            }
            old.SalesInventory.Clear();
            foreach (var item in updatedInventory)
            {
                updatedSale.SalesInventory.Add(new SalesInventory { InventoryId = item.InvId, SalesId = updatedSale.SalesId, InventoryQty = item.Quantity });
                Inventory update = _context.Inventory.First(x => x.InvId == item.InvId);
                update.Quantity -= item.Quantity;
            }
            old.SalesInventory = updatedSale.SalesInventory;
            old.TransDate = updatedSale.TransDate;
            old.Subtotal = updatedSale.Subtotal;
            old.SalesTax = updatedSale.SalesTax;
            old.Amount = updatedSale.Amount;
            old.CashAmount = updatedSale.CashAmount;
            _context.Entry(old).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(old);
            _context.SaveChanges();

            AccountsPayable oldap = _context.AccountsPayable.First(x => x.SalesId == updatedSale.SalesId);
            oldap.AmountDue = updatedSale.SalesTax;
            _context.Entry(oldap).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldap);
            _context.SaveChanges();

            return RedirectToAction("SalesIndex");
        }
    } 
}
