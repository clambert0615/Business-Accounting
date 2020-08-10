using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class InventoryController : Controller
    {
        private readonly AccountingAPIDbContext _context;
        public AccPayInvExp apie = new AccPayInvExp();
        public InventoryController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult InventoryIndex()
        {
            List<Inventory> inventoryList = _context.Inventory.ToList();
            apie.InventoryList = inventoryList;
            return View(apie);
        }
        [HttpGet]
        public IActionResult AddInventory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddInventory(Inventory inventory )
        {
            if (ModelState.IsValid)
            {
                _context.Inventory.Add(inventory);
                _context.SaveChanges();
            }
            return RedirectToAction("InventoryIndex", new { id = inventory.InvId });
        }
    }
}
