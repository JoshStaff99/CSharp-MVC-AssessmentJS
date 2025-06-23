using System;
using System.Linq;
using CSharp_MVC_AssessmentJS.Models;

namespace CompanyEmployeeApp.Data
{
    public static class DbSeeder
    {
        public static void Seed(ApplicationDbContext context)
        {
            // Apply any pending migrations
            context.Database.EnsureCreated();

            if (!context.Companies.Any())
            {
                var companies = new[]
                {
                    new Company { Name = "TechCorp", Email = "info@techcorp.com", Website = "http://techcorp.com", LogoPath = "wwwroot/logos/placeholder1.svg"},
                    new Company { Name = "InnoSoft", Email = "contact@innosoft.com", Website = "http://innosoft.com", LogoPath = "wwwroot/logos/placeholder1.svg" },
                    new Company { Name = "DevSolutions", Email = "hello@devsolutions.com", Website = "http://devsolutions.com", LogoPath = "wwwroot/logos/placeholder1.svg" }
                };

                context.Companies.AddRange(companies);
                context.SaveChanges();

                var employees = new[]
                {
                    new Employee { FirstName = "Alice", LastName = "Johnson", CompanyId = companies[0].Id, Email = "alice@techcorp.com", PhoneNumber = "123-456-7890" },
                    new Employee { FirstName = "Bob", LastName = "Smith", CompanyId = companies[1].Id, Email = "bob@innosoft.com", PhoneNumber = "234-567-8901" },
                    new Employee { FirstName = "Carol", LastName = "Brown", CompanyId = companies[2].Id, Email = "carol@devsolutions.com", PhoneNumber = "345-678-9012" }
                };

                context.Employees.AddRange(employees);
                context.SaveChanges();
            }
        }
    }
}