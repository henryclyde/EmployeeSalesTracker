using HCProject9.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HCProject9.Controllers
{
    public class HomeController : Controller
    {
        private SalesContext context { get; set; } // sales context established
        public HomeController(SalesContext ctx) => context = ctx; // home controller called with sales context

        [HttpGet]
        public ViewResult Index(int id) // employee id passed in, for filtering by employee
        {
            // build sales query based on whether there's an employee id to filter by
            IQueryable<Sales> query = context.Sales
                .Include(s => s.Employee)
                .OrderBy(s => s.Year);
            if (id > 0)
                query = query.Where(s => s.EmployeeId == id); // if an id is selected, query the sales by the employee chosen

            //saleslistviewmodel called, with queried info about employee and their sales
            var vm = new SalesListViewModel {
                Sales = query.ToList(),  // execute sales query
                Employees = context.Employees.OrderBy(e => e.Firstname).ToList(),
                EmployeeId = id
            };
            return View(vm); // SalesListViewModel view returned.
        }

        [HttpPost]
        public RedirectToActionResult Index(Employee employee) { //redirected to employee results,
            if (employee.EmployeeId > 0)
                return RedirectToAction("Index", new { id = employee.EmployeeId }); // if an employee is chosen
            else
                // pass empty string for id segment to clear any previous values
                return RedirectToAction("Index", new { id = "" }); // if ID is null or not chosen, return main home page.
        }
            
    }
}
