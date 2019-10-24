using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using EmployeeManagement.Models;
using EmployeeManagement.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;

namespace EmployeeManagement.Controllers
{
    public class HomeController : Controller
    {
        private IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        // Inject IEmployeeRepository using Constructor Injection
        public HomeController(IEmployeeRepository employeeRepository,
                            IHostingEnvironment hostingEnvironment)
        {
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public IActionResult jquery()
        {

            // retrieve all the employees
            var model = _employeeRepository.GetAllEmployees();
            // Pass the list of employees to the view

            return View(model);
        }

        [HttpPost]
        public IActionResult jquery(int i=0)
        {

            // retrieve all the employees
            var model = _employeeRepository.GetAllEmployees();
            // Pass the list of employees to the view

            return Json(model);
        }




        // Retrieve employee name and return
        [AllowAnonymous]
        public ViewResult Index()
        {
            
            // retrieve all the employees
            var model = _employeeRepository.GetAllEmployees();
            // Pass the list of employees to the view
            
            return View(model);
        }
        
        public ViewResult Details(int id)
        {

            Employee Employee = _employeeRepository.GetEmployee(id);
            if (Employee == null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", id);
            }

            // Pass the ViewModel object to the View() helper method
            return View(Employee);
        }
        [HttpGet]

        public ViewResult Create()
        {

            return View();
        }

        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath

            };
            return View(employeeEditViewModel);
        }
        [HttpPost]
        public IActionResult Edit(EmployeeEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                

                if (model.Photos != null) {
                    if (model.ExistingPhotoPath != null)
                    {

                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                            }

                    employee.PhotoPath = ProcessUploadedFile(model);
                }

               

                _employeeRepository.Update(employee);
                return RedirectToAction("index");
            }
            return View();
        }

        private string ProcessUploadedFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photos != null)
            {
                
                {
                    string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photos.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using(var fileStream = new FileStream(filePath, FileMode.Create)) {
                        model.Photos.CopyTo(fileStream); 
                    }
                    
                } 
            }

            return uniqueFileName;
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadedFile(model);

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName

                };

                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }

    }
}
