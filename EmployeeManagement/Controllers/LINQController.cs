using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmployeeManagement.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeManagement.Controllers
{
    public class LINQController : Controller
    {
        private readonly AppDbContext appDbContext;

        public LINQController(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }
           
        public IActionResult Index()
        {
            var x = from employee in appDbContext.Roles.Where(role=>role.NormalizedName=="ADMIN")
                    select employee ;
           return View(x);
        }
    }
}