using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AccountingProgram.Models;
using Microsoft.AspNetCore.Mvc;

namespace AccountingProgram.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly AccountingAPIDbContext _context;
        public EmployeesController(AccountingAPIDbContext Context)
        {
            _context = Context;
        }
        public IActionResult EmployeesIndex()
        {
            List<Employee> employeeList = _context.Employee.ToList();

            return View(employeeList);
        }
        public IActionResult GetEmployee(int id)
        {
            Employee found = _context.Employee.Find(id);
            List<Wages> wageList = _context.Wages.Where(x => x.EmployeeId == found.EmpId).ToList();
            found.Wages = wageList;
            return View(found);
        }
        [HttpGet]
        public IActionResult AddEmployee()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddEmployee(Employee employee)
        {
            if(ModelState.IsValid)
            {
                _context.Employee.Add(employee);
                _context.SaveChanges();
            }
            return RedirectToAction("EmployeesIndex");
        }
        [HttpGet]
        public IActionResult UpdateEmployee(int id)
        {
            Employee found = _context.Employee.Find(id);
            if(found != null)
            {
                return View(found);
            }
            else
            {
                return View("ErrorPage");
            }
        }
        [HttpPost]
        public IActionResult UpdateEmployee(Employee updatedEmployee)
        {
            Employee old = _context.Employee.Find(updatedEmployee.EmpId);
            old.FirstName = updatedEmployee.FirstName;
            old.LastName = updatedEmployee.LastName;
            old.StreetAddress = updatedEmployee.StreetAddress;
            old.City = updatedEmployee.City;
            old.State = updatedEmployee.State;
            old.Zip = updatedEmployee.Zip;
            old.Phone = updatedEmployee.Phone;
            old.Email = updatedEmployee.Email;
            old.Birthdate = updatedEmployee.Birthdate;
            old.Ssn = updatedEmployee.Ssn;

            _context.Entry(old).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.Update(old);
            _context.SaveChanges();

            return RedirectToAction("EmployeesIndex");
        }
       

    }
}
