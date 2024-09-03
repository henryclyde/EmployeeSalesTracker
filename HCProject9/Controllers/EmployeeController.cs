using HCProject9.Models;
using HCProject9.Models.Validation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HCProject9.Controllers
{
    public class EmployeeController : Controller
    {
        private SalesContext context { get; set; } // sales context database created
        public EmployeeController(SalesContext ctx) => context = ctx; // employee controller calls sales context database.

        public IActionResult Index() => RedirectToAction("Index", "Home"); // index method, redirecting to home page.

        [HttpGet]
        public IActionResult Add() // add employee method
        {
            ViewBag.Employees = context.Employees.OrderBy(e => e.Firstname).ToList(); // ordered employees in list
            return View(); // view returned.
        }

        [HttpPost]
        public IActionResult Add(Employee employee)
        {
            // server side checks for remote validation
            string msg = Validate.CheckEmployee(context, employee); // validation message
            if (!string.IsNullOrEmpty(msg)) // if  the msg string is null
            {
                ModelState.AddModelError(nameof(Employee.DOB), msg); // add error message for the date of birth
            }
            msg = Validate.CheckManagerEmployeeMatch(context, employee); // validate the manager and employee match
            if (!string.IsNullOrEmpty(msg)) // if the msg is null
            {
                ModelState.AddModelError(nameof(Employee.ManagerId), msg); // create error message for matching manager
            }

            if (ModelState.IsValid) // if modelstate is valid
            {
                context.Employees.Add(employee); // employee added.
                context.SaveChanges(); // database saved.
                TempData["message"] = $"Employee {employee.Fullname} added"; // message saying which employee is added.
                return RedirectToAction("Index", "Home"); // returns home page.
            }
            else
            {
                ViewBag.Employees = context.Employees.OrderBy(e => e.Firstname).ToList(); // if model state is not valid, return list
                return View(); // return view.
            }
        }
    }
}