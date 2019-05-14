using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeManagement.Models
{
    public static class ModelBuilderExtensions
    {

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee
                {
                    Id = 2,
                    Name = "Mark",
                    Department = Dept.IT,
                    Email = " mark@pragim.com"
                },
                new Employee
                {
                    Id = 3,
                    Name = "zika",
                    Department = Dept.HR,
                    Email = " zika@pragim.com"

                }


                );


        }
    }
}
