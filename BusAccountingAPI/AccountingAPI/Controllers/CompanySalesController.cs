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
    public class CompanySalesController : ControllerBase
    {
        private readonly AccountingAPIDbContext _context;
        public CompanySalesController(AccountingAPIDbContext context)
        {
            _context = context;
        }
        //get a list of sales endpoint: api/companysales
        [HttpGet]
        public async Task<ActionResult<List<Sales>>> GetSales()
        {
            var salesList = await _context.Sales.ToListAsync();
            return salesList;
        }
        //get a specific sale endpoint: api/companysales/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Sales>> GetSale(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }
            else
            {
                return sale;
            }
        }

        //Post: api/companysales
        [HttpPost]
        public async Task<ActionResult<Sales>> AddSale(Sales sale)
        {
            if (ModelState.IsValid)
            {
                _context.Sales.Add(sale);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetSale), new { id = sale.SalesId }, sale);
            }
            else
            {
                return BadRequest();
            }
        }

        //Update sales api/companysales
        [HttpPut]
        public async Task<ActionResult> UpdateSales(Sales updatedSale)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Entry(updatedSale).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        //Delete sale api/companysales/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteSale(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();

            }
            else
            {
                _context.Sales.Remove(sale);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
