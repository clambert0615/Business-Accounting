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
    public class ReceivablesController : ControllerBase
    {
        private readonly AccountingAPIDbContext _context;
        public ReceivablesController(AccountingAPIDbContext context)
        {
            _context = context;
        }
        //get a list of accounts receivable endpoint: api/receivables
        [HttpGet]
        public async Task<ActionResult<List<AccountsReceivable>>> GetReceivables()
        {
            var receivablesList = await _context.AccountsReceivable.ToListAsync();
            return receivablesList;
        }
        //get a specific receivable endpoint: api/receivables/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountsReceivable>> GetReceivable(int id)
        {
            var rec = await _context.AccountsReceivable.FindAsync(id);
            if (rec== null)
            {
                return NotFound();
            }
            else
            {
                return rec;
            }
        }

        //Post: api/receivables
        [HttpPost]
        public async Task<ActionResult<AccountsReceivable>> AddReceivable(AccountsReceivable rec)
        {
            if (ModelState.IsValid)
            {
                _context.AccountsReceivable.Add(rec);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetReceivable), new { id = rec.Id }, rec);
            }
            else
            {
                return BadRequest();
            }
        }

        //Update receivables api/receivables
        [HttpPut]
        public async Task<ActionResult> UpdateReceivable(AccountsReceivable updatedRec)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Entry(updatedRec).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        //Delete receivable api/receivables/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReceivable(int id)
        {
            var rec= await _context.AccountsReceivable.FindAsync(id);
            if (rec == null)
            {
                return NotFound();

            }
            else
            {
                _context.AccountsReceivable.Remove(rec);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

    }
}
