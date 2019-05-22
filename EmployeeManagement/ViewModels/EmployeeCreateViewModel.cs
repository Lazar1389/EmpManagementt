using EmployeeManagement.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.ViewModels
{
    public class EmployeeCreateViewModel
    {
        
        [Required]
        public string Name { get; set; }
        [Display(Name = "Office Email")]
        [RegularExpression(@"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$",
        ErrorMessage = "Invaliddd email format")]
        [Required]
        public string Email { get; set; }
        [Required]
        public Dept? Department { get; set; }

        public IFormFile Photos { get; set; }
    }
}
