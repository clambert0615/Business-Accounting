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
        public IActionResult GetIndividualItem(int id)
        {
            Inventory found = _context.Inventory.Find(id);
            return View(found);
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
        [HttpGet]
        public IActionResult UpdateInventory(int id)
        {
            Inventory foundItem = _context.Inventory.Find(id);
            if (foundItem == null)
            {
                return RedirectToAction("ErrorPage");
            }
            else
            {
                return View(foundItem);
            }
        }
        [HttpPost]
        public IActionResult UpdateInventory(Inventory updatedItem)
        {
            Inventory oldItem = _context.Inventory.Find(updatedItem.InvId);
            oldItem.Item = updatedItem.Item;
            oldItem.Description = updatedItem.Description;
            oldItem.Price = updatedItem.Price;
            oldItem.Quantity = updatedItem.Quantity;
            oldItem.Received = updatedItem.Received;
            oldItem.BackOrdered = updatedItem.BackOrdered;
            _context.Entry(oldItem).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(oldItem);
            _context.SaveChanges();
            return RedirectToAction("InventoryIndex");
        }
        public IActionResult DeleteInventory(int id)
        {
            Inventory found = _context.Inventory.Find(id);
            if (found != null)
            {
                _context.Inventory.Remove(found);
                _context.SaveChanges();
                return RedirectToAction("InventoryIndex");
            }
            else
            {
                return RedirectToAction("ErrorPage");
            }
        }
    }
}
