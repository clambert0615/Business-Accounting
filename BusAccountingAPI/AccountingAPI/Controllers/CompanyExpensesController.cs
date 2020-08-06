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
    public class CompanyExpensesController : ControllerBase
    {
        private readonly AccountingAPIDbContext _context;
        public CompanyExpensesController(AccountingAPIDbContext context)
        {
            _context = context;
        }
        //get a list of expenses endpoint: api/companyexpenses
        [HttpGet]
        public async Task<ActionResult<List<Expenses>>> GetExpenses()
        {
            var expenseList = await _context.Expenses.ToListAsync();
            return expenseList;
        }
        //get a specific expense endpoint: api/companyexpenses/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Expenses>> GetExpense(int id)
        {
            var exp = await _context.Expenses.FindAsync(id);
            if (exp == null)
            {
                return NotFound();
            }
            else
            {
                return exp;
            }
        }

        //Post: api/companyexpenses
        [HttpPost]
        public async Task<ActionResult<Expenses>> AddExpense(Expenses exp)
        {
            if (ModelState.IsValid)
            {
                _context.Expenses.Add(exp);
                await _context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetExpense), new { id = exp.ExpId }, exp);
            }
            else
            {
                return BadRequest();
            }
        }

        //Update expense api/companyexpenses
        [HttpPut]
        public async Task<ActionResult> UpdateExpense(Expenses updatedExp)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                _context.Entry(updatedExp).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

        //Delete expense api/companyexpenses/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteExpense(int id)
        {
            var exp = await _context.Expenses.FindAsync(id);
            if (exp == null)
            {
                return NotFound();

            }
            else
            {
                _context.Expenses.Remove(exp);
                await _context.SaveChangesAsync();
                return NoContent();
            }
        }

    }
}
