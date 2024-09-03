using HCProject9.Models;
using HCProject9.Models.Validation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.ComponentModel.DataAnnotations;

namespace HCProject9.Controllers
{
    public class ValidationController : Controller
    {
        private SalesContext context { get; set; }  // sales context database called
        public ValidationController(SalesContext ctx) => context = ctx; // context set for validation controller

        public JsonResult CheckEmployee(DateTime dob, string firstname, string lastname) // check employee birth and name
        {
            var employee = new Employee // new employee
            {
                Firstname = firstname,
                Lastname = lastname,
                DOB = dob
            };
            string msg = Validate.CheckEmployee(context, employee); //msg set upon employee validation
            if (string.IsNullOrEmpty(msg)) // if msg is empty
                return Json(true); // return true
            else
                return Json(msg); // if not, return the error message.
        }

        public JsonResult CheckManager(int managerId, string firstname, string lastname, DateTime dob) // check manager, using name and birth.
        {
            var employee = new Employee 
            {
                Firstname = firstname,
                Lastname = lastname,
                DOB = dob,
                ManagerId = managerId
            };
            string msg = Validate.CheckManagerEmployeeMatch(context, employee); // validate manager employee match, set error message.
            if (string.IsNullOrEmpty(msg)) // if msg is empty:
                return Json(true); // return data
            else
                return Json(msg);  // if msg is not empty, return error.
        }

        public JsonResult CheckSales(int quarter, int year, int employeeId) // check sales
        {
            var sales = new Sales
            {
                Quarter = quarter,
                Year = year,
                EmployeeId = employeeId
            };
            string msg = Validate.CheckSales(context, sales); // validate sales, set message.
            if (string.IsNullOrEmpty(msg))
                return Json(true); // if no error, return data
            else
                return Json(msg); // if error, show it to user.
        }

    }
}