using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AccountingAPIDbContext _context;
        public CustomerController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult CustomerIndex()
        {
            List<Customers> customerList = _context.Customers.ToList();
            foreach(Customers c in customerList)
            {
                c.AccountsReceivable = _context.AccountsReceivable.Where(x => x.CustomerId == c.CustId).ToList();
            }
            return View(customerList);
        }
        public IActionResult IndividualCustomer(int id)
        {
            Customers found = _context.Customers.Find(id);
            found.AccountsReceivable = _context.AccountsReceivable.Where(x => x.CustomerId == found.CustId).ToList();
           
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
        public IActionResult AddCustomer()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCustomer(Customers customer)
        {
            if(ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                _context.SaveChanges();
                return RedirectToAction("CustomerIndex");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        [HttpGet]
        public IActionResult UpdateCustomer(int id)
        {
            Customers found = _context.Customers.Find(id);
            return View(found);
        }
        [HttpPost]
        public IActionResult UpdateCustomer(Customers updatedCustomer)
        {
            Customers old = _context.Customers.Find(updatedCustomer.CustId);
            old.Name = updatedCustomer.Name;
            old.StreetAdd = updatedCustomer.StreetAdd;
            old.City = updatedCustomer.City;
            old.State = updatedCustomer.State;
            old.Zip = updatedCustomer.Zip;
            old.Phone = updatedCustomer.Phone;
            old.Email = updatedCustomer.Email;
            _context.Entry(old).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(old);
            _context.SaveChanges();
            return RedirectToAction("CustomerIndex");
        }

        public IActionResult DeleteCustomer(int id)
        {
            Customers found = _context.Customers.Find(id);
            if (found != null)
            {
                _context.Customers.Remove(found);
                _context.SaveChanges();
                return RedirectToAction("CustomerIndex");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
    }
}
