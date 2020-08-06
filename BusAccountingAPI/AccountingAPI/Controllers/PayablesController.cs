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
    public class PayablesController : ControllerBase
    {
        private readonly AccountingAPIDbContext _context;
        public PayablesController(AccountingAPIDbContext context)
        {
            _context = context;
        }
        //get a list of accounts payable endpoint: api/payables
        [HttpGet]
        public async Task<ActionResult<List<AccountsPayable>>> GetPayables()
        {
            var payables = await _context.AccountsPayable.ToListAsync();
            return payables;
        }
        //get a specific payable endpoint: api/payables/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<AccountsPayable>> GetPayable(int id)
        {
            var payable = await _context.AccountsPayable.FindAsync(id);
            if (payable == null)
            {
                return NotFound();
            }
            else
            {
                return payable;
            }
        }

        //Post: api/payables
        [HttpPost]
        public async Task<ActionResult<AccountsPayable>> AddPayable (AccountsPayable payable)
        {
            if(ModelState.IsValid)
            {
                _context.AccountsPayable.Add(payable);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetPayable), new { id = payable.PayableId }, payable);
            }
            else
            {
                return BadRequest();
            }
        }

        //Update payables api/payables
        [HttpPut]
        public async Task<ActionResult> UpdatePayable (AccountsPayable updatedPayable)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Entry(updatedPayable).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        //Delete payable api/payables/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeletePayable(int id)
        {
            var payable = await _context.AccountsPayable.FindAsync(id);
            if(payable == null)
            {
                return NotFound();

            }
            else
            {
                _context.AccountsPayable.Remove(payable);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

    }
}
