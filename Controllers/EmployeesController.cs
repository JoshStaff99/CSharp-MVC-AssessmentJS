using Microsoft.AspNetCore.Mvc;
using CSharp_MVC_AssessmentJS.Models;
using CompanyEmployeeApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;

namespace CSharp_MVC_AssessmentJS.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EmployeesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Employees
        public IActionResult Index()
        {
            var employees = _context.Employees.ToList();
            return View(employees);
        }

        // GET: Employees/Show/5
        public IActionResult Show(int? id)
        {
            if (id == null) return NotFound();
            var employee = _context.Employees.FirstOrDefault(m => m.Id == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("FirstName,LastName,Email,PhoneNumber,CompanyId")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var employee = _context.Employees.Find(id);
            if (employee == null) return NotFound();

            // Pass companies as SelectList to ViewData
            ViewData["Companies"] = new SelectList(_context.Companies, "Id", "Name");

            return View(employee);
        }

        // POST: Employees/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,FirstName,LastName,Email,PhoneNumber,CompanyId")] Employee employee)
        {
            if (id != employee.Id) return NotFound();

            if (ModelState.IsValid)
            {
                _context.Update(employee); // Update the employee in the context
                _context.SaveChanges();    // Save the changes to the database
                return RedirectToAction(nameof(Index)); // Redirect to the Index page
            }

            // Repopulate the companies dropdown if model state is invalid
            ViewData["Companies"] = new SelectList(_context.Companies, "Id", "Name", employee.CompanyId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var employee = _context.Employees.FirstOrDefault(m => m.Id == id);
            if (employee == null) return NotFound();
            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var employeeToDelete = _context.Employees.Find(id);
            if (employeeToDelete != null)
            {
                _context.Employees.Remove(employeeToDelete);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
