using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;

namespace MappingExample
{
    public class CompanyContextInitializer : DropCreateDatabaseAlways<CompanyContext>
    {
        protected override void Seed(CompanyContext context)
        {
            base.Seed(context);

            Employee emp1 = new Employee
            {
                Name = "Jenson",
                Username = "jenson",
                PhoneNumber = "9876",
            };

            Employee emp2 = new Employee
            {
                Name = "Checo",
                Username = "checo",
                PhoneNumber = "1234",
            };

            Employee emp3 = new Employee
            {
                Name = "Fernando",
                Username = "fernando",
                PhoneNumber = "6543",
            };

            Employee emp4 = new Employee
            {
                Name = "Felipe",
                Username = "felipe",
                PhoneNumber = "8765",
            };

            Employee emp5 = new Employee
            {
                Name = "Seb",
                Username = "seb",
                PhoneNumber = "2837",
            };

            Department dep1 = new Department
            {
                Name = "Marketing",
                Employees = new List<Employee>
                {
                    emp1,
                    emp2
                }
            };

            Department dep2 = new Department
            {
                Name = "Sales",
                Employees = new List<Employee>
                {
                    emp3,
                    emp4,
                    emp5
                }
            };

            Project proj1 = new Project
            {
                Name = "Ad campaign",
                Employees = new List<Employee>
                {
                    emp1,
                    emp2
                }
            };

            Project proj2 = new Project
            {
                Name = "Website update",
                Employees = new List<Employee>
                {
                    emp1
                }
            };

            context.Departments.Add(dep1);
            context.Departments.Add(dep2);
            context.Projects.Add(proj1);
            context.Projects.Add(proj2);

            context.SaveChanges();

        }
    }
}
