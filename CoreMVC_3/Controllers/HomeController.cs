using CoreMVC_3.Models;
using CoreMVC_3.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMVC_3.Controllers
{
    //public class HomeController
    //{
    //    public string Index() // default action in Controller.
    //    {
    //        return "Hello from MVC";
    //    }
    //}

    // If we add using Microsoft.AspNetCore.Mvc; then we have more to work with
    public class HomeController : Controller // including that we can inherit from Controller
    {
        //public string Index() // default action in Controller.
        // private IEmployeeRepository _employeeRepository;
        private readonly IEmployeeRepository _employeeRepository; // Add readonly as good practice.

        // Called Constructor Injection
        public HomeController(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        //public JsonResult Index() // default action in Controller.
        //{
        //    //return this.   // So we can now return a View, JSON, etc.
        //    return Json(new { id = 1, name = "Miguelito" });  // anonymous JSON              
        //}

        //public string Index() // default action in Controlls - returned string
        public ViewResult Index() // default action in Controller - returns ViewResult
        {
            // ViewBag.Title = "Index"; // Set in View
            // return anonymous JSON
            //return Json(new { id = 1, name = "Migueito" });  // anonymous JSON

            // Original returned 1 Employee as string
            //return _employeeRepository.GetEmployee(1).Name;    

            var model = _employeeRepository.GetAllEmployee();
            return View(model);

        }

        //public JsonResult Details() // default action in Controller.
        //{
        //    Employee model = _employeeRepository.GetEmployee(1);

        //    // If an API, return the Json
        //    return Json(model);              
        //}

        // Use Fiddler to test this. 
        // To use it requires a service to make XML formatting in StartUp.cs. AddXmlSerializerFormatters 
        //public ObjectResult Details() // default action in Controller.
        //{
        //    Employee model = _employeeRepository.GetEmployee(1);

        //    // If an API, return the Json
        //    return new ObjectResult(model);
        //}

        // If you aren't making an API though, you want to return a View to present the model data.
        public ViewResult Details() // default action in Controller.
        {
            Employee model = _employeeRepository.GetEmployee(1);

            // If not an API, return the Json
            // return View(model); // The View returns HTML, 
            // Notice thatt here are 4 overloads. So...
            // return View("Test"); // The View returns HTML, 

            // This also looks for View file of the same name as the aAction method.
            // return View();

            // Or you could specify an Absolute path which then needs the file extension
            // return View("MyViews/Test.cshtml"); // The View returns HTML, 
            // or return View("MyViews/Test.cshtml"); // The View returns HTML, 
            // or return View("~/MyViews/Test.cshtml"); // The View returns HTML, 

            // Relative path does not use file extension
            // or return View("../../MyViews/Test"); // It starts looking in the View/Home folder 

            // Video22 -  - Passing data to view in ASP NET Core MVC
            ViewData["Employee"] = model;
            ViewData["PageTitle"] = "Employee Details VD";

            model = _employeeRepository.GetEmployee(2);
            // Names must not match what is in ViewData, because ViewBag is wrapper around ViewData
            ViewBag.EmployeeVB = model; 
            ViewBag.PageTitleVB = "Employee Details VB";
            //-return View();

            //model = _employeeRepository.GetEmployee(0); // This was a problem...
            // or just make it a strongly typed View by passing the model that way.
            //return View(model);

            // Or using a ViewModel
            // A View model may not have all the information needed, so you use a ViewModel,
            // basically a composite model. Also known as Data Transfer Objects (DTO).
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel
            {
                Employee = _employeeRepository.GetEmployee(1),
                PageTitle = "Details ViewModel"
            };
            return View(homeDetailsViewModel);
        }

        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }

        [HttpPost]
        public RedirectToActionResult Create(Employee employee)
        {
            Employee newEmployee = _employeeRepository.Add(employee);
            return RedirectToAction("details", new { id = newEmployee.Id });
        }

    } // End public class HomeController : Controller
}     // End namespace
