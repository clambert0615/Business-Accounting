using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class VendorController : Controller
    {
        private readonly AccountingAPIDbContext _context;

        public VendorController(AccountingAPIDbContext context)
        {
            _context = context;
        }

        
        public IActionResult GetAllVendors()
        {
            List<Vendor> vendorList = _context.Vendor.ToList();
            foreach(Vendor v in vendorList)
            {
                v.AccountsPayable = _context.AccountsPayable.Where(x => x.VenId == v.VenId).ToList();
            }
            return View(vendorList);
        }
     
        public IActionResult IndividualVendor(int id)
        {
            Vendor foundVendor = _context.Vendor.Find(id);
            foundVendor.AccountsPayable = _context.AccountsPayable.Where(x => x.VenId == foundVendor.VenId).ToList();
            return View(foundVendor);
        }
        [HttpGet]
        public IActionResult AddNewVendor()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNewVendor(Vendor vendor)
        {
            if (ModelState.IsValid)
            {
                _context.Vendor.Add(vendor);
                _context.SaveChanges();
            }
            return RedirectToAction("GetAllVendors", new { id = vendor.VenId });

        }
       
        [HttpGet]
        public IActionResult UpdateVendor(int id)
        {
            Vendor foundVendor = _context.Vendor.Find(id);
            if (foundVendor == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                return View(foundVendor);
            }
        }
        [HttpPost]
        public IActionResult UpdateVendor(Vendor updatedVendor)
        {
            Vendor oldVendor = _context.Vendor.Find(updatedVendor.VenId);
            oldVendor.Name = updatedVendor.Name;
            oldVendor.Address = updatedVendor.Address;
            oldVendor.City = updatedVendor.City;
            oldVendor.State = updatedVendor.State;
            oldVendor.Zip = updatedVendor.Zip;
            oldVendor.Phone = updatedVendor.Phone;
            oldVendor.Email = updatedVendor.Email;
            _context.Entry(oldVendor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldVendor);
            _context.SaveChanges();
            return RedirectToAction("GetAllVendors");
        }
      

        public IActionResult DeleteVendor(int id)
        {
            Vendor found = _context.Vendor.Find(id);
            if (found != null)
            {
                _context.Vendor.Remove(found);
                _context.SaveChanges();
                return RedirectToAction("GetAllVendors");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }




    }
}
