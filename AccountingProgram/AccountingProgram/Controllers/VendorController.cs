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
        private readonly AccountingDAL ad = new AccountingDAL();

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
        public async Task<IActionResult> GetAllVendors()
        {
            List<Vendor> vendorList = await ad.GetVendors();
            return View(vendorList);
        }
        public IActionResult GetVendor()
        {
            return View();
        }
        public async Task<IActionResult> IndividualVendor(int id)
        {
            Vendor foundVendor = await ad.GetVendor(id);
            return View(foundVendor);
        }
        public async Task<IActionResult> AddNewVendor(Vendor vendor)
        {
            await ad.AddVendor(vendor);
            return View();
            //return RedirectToAction("GetAllVendors");
        }
        public IActionResult GetVendorToUpdate()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> UpdateVendor(int id)
        {
            Vendor foundVendor = await ad.GetVendor(id);
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
        public async Task<IActionResult> UpdateVendor(Vendor updatedVendor)
        {
            Vendor oldVendor = await ad.GetVendor(updatedVendor.VenId);
             oldVendor.Name = updatedVendor.Name;
            oldVendor.Address = updatedVendor.Address;
            ad.UpdateVendor(oldVendor);
            return RedirectToAction("GetAllVendors");
        }
        public IActionResult GetVendorToDelete()
        {
            return View();
        }
       
        public async Task<IActionResult> DeleteVendor(int id)
        {
            bool isSuccessful = await ad.DeleteVendor(id);
            if (isSuccessful)
            {
                return RedirectToAction("GetAllVendors");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
        


        
    }
}
