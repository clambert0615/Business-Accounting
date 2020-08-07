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

        public IActionResult VendorIndex(int option)
        {
            if (option == 1)
            {
                return RedirectToAction("GetAllVendors");
            }
            else if (option == 2)
            {
                return RedirectToAction("GetVendor");
            }
            else if (option == 3)
            {
                return RedirectToAction("AddNewVendor");
            }
            else if (option == 4)
            {
                return RedirectToAction("GetVendorToUpdate");
            }
            else if (option == 5)
            {
                return RedirectToAction("GetVendorToDelete");
            }
            else
            {
                return View();
            }
        }
        public IActionResult GetAllVendors()
        {
            List<Vendor> vendorList = _context.Vendor.ToList();
            return View(vendorList);
        }
        public IActionResult GetVendor()
        {
            return View();
        }
        public IActionResult IndividualVendor(int id)
        {
            Vendor foundVendor = _context.Vendor.Find(id);
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
            return RedirectToAction("VendorIndex", new { id = vendor.VenId });

        }
        public IActionResult GetVendorToUpdate()
        {
            return View();
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
            _context.Entry(oldVendor).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldVendor);
            _context.SaveChanges();
            return RedirectToAction("GetAllVendors");
        }
        public IActionResult GetVendorToDelete()
        {
            return View();
        }

        public IActionResult DeleteVendor(int id)
        {
            Vendor found = _context.Vendor.Find(id);
            if (found != null)
            {
                _context.Vendor.Remove(found);
                _context.SaveChanges();
                return RedirectToAction("VendorIndex");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }




    }
}
