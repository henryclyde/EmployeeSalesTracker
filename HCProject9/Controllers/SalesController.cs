using HCProject9.Models;
using HCProject9.Models.Validation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace HCProject9.Controllers
{
    public class SalesController : Controller
    {
        private SalesContext context { get; set; } // database called
        public SalesController(SalesContext ctx) => context = ctx; // sales controller called with context of database

        public IActionResult Index() => RedirectToAction("Index", "Home"); // index home method for sales controller

        [HttpGet]
        public IActionResult Add() // request for add sales
        {
            ViewBag.Employees = context.Employees.OrderBy(e => e.Firstname).ToList(); // create list of employees, put into list
            return View(); // view returned.
        }

        [HttpPost]
        public IActionResult Add(Sales sales)
        {
            // server side check for remote validation
            string msg = Validate.CheckSales(context, sales); // msg set for validating sales
            if (!string.IsNullOrEmpty(msg)) // if msg is empty, add the error message.
            {
                ModelState.AddModelError(nameof(sales.EmployeeId), msg);
            }

            if (ModelState.IsValid)
            {
                context.Sales.Add(sales); // add sales if the model state is valid
                context.SaveChanges(); // save changes to database.
                TempData["message"] = "Sales added"; // message updates user
                return RedirectToAction("Index", "Home"); // redirected to home
            }
            else
            {
                ViewBag.Employees = context.Employees.OrderBy(e => e.Firstname).ToList(); // return employee list, if model state isn't valid
                return View(); // returned view
            }

        }
    }
}