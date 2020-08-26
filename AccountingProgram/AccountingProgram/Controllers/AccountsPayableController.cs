using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;

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
            apie.PaymentList = _context.Payments.Where(x => x.PayId == foundPayable.PayableId).ToList();
                        
            return View(apie);
        }
        public IActionResult MakePayment(int id, decimal amount)
        {
            AccountsPayable payable = _context.AccountsPayable.Find(id);
            payable.PaymentAmount = amount;
            payable.Balance -= amount;
            payable.PaymentDate = DateTime.Today;
            Cash c = new Cash();
            c.Withdrawl = amount;
            c.TransDate = DateTime.Today;
            _context.Cash.Add(c);
            _context.SaveChanges();
            Payments p = new Payments();
            p.PayId = payable.PayableId;
            p.PayDate = DateTime.Today;
            p.Amount = amount;
            p.CashId = c.Id;
            _context.Payments.Add(p);
            _context.Entry(payable).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(payable);
            _context.SaveChanges();

            return RedirectToAction("GetAllPayables");
        }

        //This is used for inventory only.  Fixed assets are done in the fixed assests controller
        [HttpGet]
        public IActionResult AddPayable()
        {
          //This is for a purchase order for inventory
            return View();
        }
        [HttpPost]
        public IActionResult AddPayable(AccountsPayable payable, List<Inventory> inventoryList)
        {
            foreach (var item in inventoryList)
            {
                if (item.InvId == 0)
                {
                    _context.Inventory.Add(item);
                    _context.SaveChanges();
                }
                else
                {
                    Inventory found = _context.Inventory.Find(item.InvId);
                    found.Quantity += item.Quantity;
                    found.Received += item.Received;
                    found.BackOrdered += item.BackOrdered;
                    _context.Entry(found).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    _context.Update(found);
                    _context.SaveChanges();
                }
            }
                payable.PayableInventory = new List<PayableInventory>();
          
            
                foreach(var item in inventoryList)
                {
                    payable.PayableInventory.Add(new PayableInventory {InventoryId = item.InvId, PayableId = payable.PayableId, InvQuantity = item.Quantity, InvPrice = item.Price, InvBackOrdered = item.BackOrdered, InvReceived = item.Received });

                }

                _context.AccountsPayable.Add(payable);
                _context.SaveChanges();
            

           
            return RedirectToAction("GetAllPayables", new { id = payable.PayableId });
        }
        [HttpGet]
        public IActionResult UpdatePayable(int id)
        {
            AccountsPayable foundPayable = _context.AccountsPayable.Find(id);
            foundPayable.PayableInventory = _context.PayableInventory.Where(x => x.PayableId == foundPayable.PayableId).ToList();
            if (foundPayable == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                return View(foundPayable);
            }
          
        }

        [HttpPost]
        public IActionResult UpdatePayable(AccountsPayable updatedPayable, List<Inventory> updatedInventory) 
        {
            AccountsPayable oldPayable = _context.AccountsPayable.Find(updatedPayable.PayableId);
            oldPayable.PayableInventory = _context.PayableInventory.Where(x => x.PayableId == oldPayable.PayableId).ToList();

            foreach(var item in oldPayable.PayableInventory)
            {
                Inventory found = _context.Inventory.Find(item.InventoryId);
                found.Quantity -= item.InvQuantity;
                found.Received -= item.InvReceived;
                found.BackOrdered -= item.InvBackOrdered;
            }
            oldPayable.PayableInventory.Clear();
            foreach(var item in updatedInventory)
            {
                updatedPayable.PayableInventory.Add(new PayableInventory { InventoryId = item.InvId, PayableId = updatedPayable.PayableId, InvQuantity = item.Quantity, InvPrice = item.Price, InvBackOrdered = item.BackOrdered, InvReceived = item.Received });
                Inventory found = _context.Inventory.First(x => x.InvId == item.InvId);
                found.Quantity += item.Quantity;
                found.Received += item.Received;
                found.BackOrdered = item.BackOrdered;
                _context.Entry(found).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                _context.Update(found);
                _context.SaveChanges();
            }
            oldPayable.PayableInventory = updatedPayable.PayableInventory;
            oldPayable.VenId = updatedPayable.VenId;
            oldPayable.VendorName = updatedPayable.VendorName;
            oldPayable.DueDate = updatedPayable.DueDate;
            oldPayable.AmountDue = updatedPayable.AmountDue;
            oldPayable.Balance = updatedPayable.Balance;
            _context.Entry(oldPayable).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldPayable);
            _context.SaveChanges();
            return RedirectToAction("GetAllPayables");
        }
        [HttpGet]
        public IActionResult EditPayment(int id)
        {
            Payments found = _context.Payments.Find(id);
            return View(found);
        }
        
        [HttpPost]
        public IActionResult EditPayment(Payments updatedPayment)
        {
            Payments oldPayment = _context.Payments.Find(updatedPayment.PaymentId);
            Cash oldCash = _context.Cash.First(x => x.Id == oldPayment.CashId);
            AccountsPayable oldPayable = _context.AccountsPayable.First(x => x.PayableId == oldPayment.PayId);

            oldPayable.PaymentAmount = updatedPayment.Amount;
            oldPayable.Balance = oldPayable.Balance + oldPayment.Amount - updatedPayment.Amount;
            oldPayable.PaymentDate = updatedPayment.PayDate;
            _context.Entry(oldPayable).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldPayable);
            _context.SaveChanges();

            oldPayment.PayDate = updatedPayment.PayDate;
            oldPayment.Amount = updatedPayment.Amount;
            _context.Entry(oldPayment).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldPayment);
            _context.SaveChanges();

            oldCash.Withdrawl = updatedPayment.Amount;
            oldCash.TransDate = updatedPayment.PayDate;
            _context.Entry(oldCash).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldCash);
            _context.SaveChanges();


            return RedirectToAction("GetAllPayables");
        }
    }
}
