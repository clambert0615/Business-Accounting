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
    public class BankController : ControllerBase
    {
        private readonly AccountingAPIDbContext _context;
        public BankController(AccountingAPIDbContext context)
        {
            _context = context;
        }
        //get a list of cash transactions endpoint: api/bank
        [HttpGet]
        public async Task<ActionResult<List<Cash>>> GetCashTransactions()
        {
            var c = await _context.Cash.ToListAsync();
            return c;
        }
        //get a specific cash transaction endpoint: api/bank/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Cash>> GetCashTransaction(int id)
        {
            var c = await _context.Cash.FindAsync(id);
            if (c == null)
            {
                return NotFound();
            }
            else
            {
                return c;
            }
        }

        //Post: api/bank
        [HttpPost]
        public async Task<ActionResult<Cash>> AddCash(Cash c)
        {
            if (ModelState.IsValid)
            {
                _context.Cash.Add(c);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetCashTransaction), new { id = c.Id }, c);
            }
            else
            {
                return BadRequest();
            }
        }

        //Update cash transaaction api/bank 
        [HttpPut]
        public async Task<ActionResult> UpdateCash(Cash updatedTransaction)
        { 
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Entry(updatedTransaction).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        //Delete cash transaction api/bank/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCashTransaction(int id)
        {
            var c = await _context.Cash.FindAsync(id);
            if (c == null)
            {
                return NotFound();

            }
            else
            {
                _context.Cash.Remove(c);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }
    }
}
