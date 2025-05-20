using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CSharp_MVC_AssessmentJS.Models;
using CSharp_MVC_AssessmentJS.Models.ViewModels;
using CompanyEmployeeApp.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace CSharp_MVC_AssessmentJS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            var dashboardData = new AdminDashboardViewModel
            {
                TotalCompanies = _context.Companies.Count(),
                TotalEmployees = _context.Employees.Count(),
                RecentCompanies = _context.Companies
                    .OrderByDescending(c => c.Id)
                    .Take(5)
                    .ToList(),
                RecentEmployees = _context.Employees
                    .OrderByDescending(e => e.Id)
                    .Take(5)
                    .ToList()
            };

            return View(dashboardData);
        }

        public IActionResult Companies()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}