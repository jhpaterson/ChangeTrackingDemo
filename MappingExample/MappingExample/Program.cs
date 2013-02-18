using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;


namespace MappingExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // set database initializer
            Database.SetInitializer<CompanyContext>(new CompanyContextInitializer());

            CompanyContext db = new CompanyContext();
            db.Configuration.LazyLoadingEnabled = false;
            db.Configuration.ProxyCreationEnabled = true;
            db.Configuration.AutoDetectChangesEnabled = false;

            // new entity
            var newEmp = new Employee { Name = "Kimi" };
            var newEmpEntity = db.Entry(newEmp);
            Console.WriteLine("NEW ENTITY NOT ATTACHED");
            Console.WriteLine(string.Format("Employee {0}, state = {1}", newEmp.Name, newEmpEntity.State));
            PrintTrackedEmployees(db);

            // attach new entity
            db.Entry(newEmp).State = System.Data.EntityState.Added;
            Console.WriteLine("\nNEW ENTITY ATTACHED");
            PrintTrackedEmployees(db);

            #region GET DATA
            // get some objects, eager load so we can access all necessary properties with LL disabled
            var jenson = db.Employees
                .Include(e => e.Projects)
                .Include(e => e.Department)
                .Where(e => e.EmployeeId==1)
                .First();
            var fernando = db.Employees
                .Include(e => e.Projects)
                .Include(e => e.Department)
                .Where(e => e.EmployeeId == 3)
                .First();
            var marketing = db.Departments
                .Include(d => d.Employees)
                .Where(d => d.DepartmentId == 1)
                .First();
            var sales = db.Departments
                .Include(d => d.Employees)
                .Where(d => d.DepartmentId == 2)
                .First();
            var adCampaign = db.Projects
                .Include(p => p.Employees)
                .Where(p => p.ProjectId == 1)
                .First();
            var website = db.Projects
                .Include(p => p.Employees)
                .Where(p => p.ProjectId == 2)
                .First();
            #endregion

            Console.WriteLine("\nRETRIEVED FROM DATABASE");
            PrintDetails(jenson);

            PrintTrackedEmployees(db);

            // change Department for Employee
            jenson.Department = sales;                                          // was marketing
                                                                                // jenson in sales, sales has 4 employees including jenson
            db.ChangeTracker.DetectChanges();                                   // will fix-up always

            Console.WriteLine("\nJENSON CHANGED DEPARTMENT TO SALES");
            PrintDetails(jenson);

            // remove Employee from Project and add Project to Employee
            jenson.Projects.Remove(adCampaign);                                // was on adcampaign and website
            fernando.Projects.Add(website);                                    // was not on any projects
                                                                               // jenson on website only, website includes fernando
            db.Employees.Add(new Employee { Name = "new" });                   // will fix-up if AutoDetectChangesEnabled
            //db.ChangeTracker.DetectChanges();

            Console.WriteLine("\nJENSON REMOVED ADCAMPAIGN, FERNANDO ADDED WEBSITE");
            PrintDetails(jenson);

            marketing.Employees.Add(jenson);                                  // was in sales
                                                                              // jenson in marketing, marketing has 2 employees including jenson
            db.SaveChanges();                                                 // will fix-up if AutoDetectChangesEnabled
            //db.ChangeTracker.DetectChanges();

            Console.WriteLine("\nMARKETING ADDED JENSON");
            PrintDetails(jenson);

            Console.WriteLine("Press any key to finish...");
            
            Console.ReadLine();
        }

        private static void PrintDetails(Employee emp)
        {
            Console.WriteLine(string.Format("Employee: {0} is in Department: {1}\n", emp.Name, emp.Department.Name));
            Console.WriteLine(string.Format("Department: {0} has Employees:", emp.Department.Name));
            foreach (var e in emp.Department.Employees)
            {
                Console.WriteLine(string.Format("{0}", e.Name));
            }

            Console.WriteLine(string.Format("\nEmployee: {0} is on Projects:", emp.Name));
            foreach (var p in emp.Projects)
            {
                Console.WriteLine(string.Format("{0}", p.Name));
                Console.WriteLine(string.Format("    Project: {0} has Employees:", p.Name));
                foreach (var e in p.Employees)
                {
                    Console.WriteLine(string.Format("    {0}", e.Name));
                }
            }
        }

        private static void PrintTrackedEmployees(DbContext db)
        {
            Console.WriteLine("Tracked Employee entities:");
            foreach (var ent in db.ChangeTracker.Entries<Employee>())
            {
                Console.WriteLine(ent.Member("Name").CurrentValue);
            }
        }
    }
}
