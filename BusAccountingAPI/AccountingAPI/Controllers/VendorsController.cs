using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorsController : ControllerBase
    {
        private readonly AccountingAPIDbContext _context;
        public VendorsController(AccountingAPIDbContext context)
        {
            _context = context;
        }
        //get a list of vendor endpoint: api/vendors
        [HttpGet]
        public async Task<ActionResult<List<Vendor>>> GetVendors()
        {
            var ven = await _context.Vendor.ToListAsync();
            return ven;
        }
        //get a specific vendor endpoint: api/vendors/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Vendor>> GetVendor(int id)
        {
            var ven = await _context.Vendor.FindAsync(id);
            if (ven == null)
            {
                return NotFound();
            }
            else
            {
                return ven;
            }
        }

        //Post: api/vendors
        [HttpPost]
        public async Task<ActionResult<Vendor>> AddVendor(Vendor ven)
        {
            if (ModelState.IsValid)
            {
                _context.Vendor.Add(ven);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetVendor), new { id = ven.VenId }, ven);
            }
            else
            {
                return BadRequest();
            }
        }

        //Update vendor api/vendors 
        [HttpPut]
        public async Task<ActionResult> UpdateVendor(Vendor updatedVen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Entry(updatedVen).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        //Delete vendor api/vendors/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVendor(int id)
        {
            var ven = await _context.Vendor.FindAsync(id);
            if (ven == null)
            {
                return NotFound();

            }
            else
            {
                _context.Vendor.Remove(ven);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

    }
}
