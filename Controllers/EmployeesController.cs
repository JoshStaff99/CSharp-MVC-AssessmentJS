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
            // Pass companies as SelectList to ViewData to populate dropdown in the view
            ViewData["Companies"] = new SelectList(_context.Companies, "Id", "Name");
            return View();
        }

        // POST: Employees/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,PhoneNumber,CompanyId")] Employee employee)
        {
            // Add extensive logging
            Console.WriteLine("Create POST action called");
            Console.WriteLine($"Employee ID (may be empty for new entity): {employee.Id}");
            Console.WriteLine($"Employee FirstName: {employee.FirstName}");
            Console.WriteLine($"Employee LastName: {employee.LastName}");

            // Log ModelState errors if any
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid:");
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine($"- Error in {state.Key}: {error.ErrorMessage}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("ModelState is valid");
            }

            try
            {
                Console.WriteLine("Attempting to add employee to context");

                // Add new employee
                await _context.Employees.AddAsync(employee);

                Console.WriteLine("Attempting to save changes");
                int rowsAffected = await _context.SaveChangesAsync();
                Console.WriteLine($"SaveChangesAsync completed, rows affected: {rowsAffected}");

                // Commit transaction explicitly (optional for Create, but included for parity)
                using var transaction = await _context.Database.BeginTransactionAsync();
                await transaction.CommitAsync();
                Console.WriteLine("Transaction committed");

                Console.WriteLine("Redirecting to Index");
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected exception: {ex.Message}");
                ModelState.AddModelError("", "An unexpected error occurred.");
            }

            Console.WriteLine("Preparing to re-render Create view");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", employee.CompanyId);
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
        public async Task<IActionResult> Edit(int id, Employee employee)
        {
            // Add extensive logging
            Console.WriteLine($"Edit POST action called");
            Console.WriteLine($"Route ID: {id}");
            Console.WriteLine($"Employee ID: {employee.Id}");
            Console.WriteLine($"Employee FirstName: {employee.FirstName}");
            Console.WriteLine($"Employee LastName: {employee.LastName}");

            if (id != employee.Id)
            {
                Console.WriteLine("ID mismatch, returning NotFound");
                return NotFound();
            }

            // Log ModelState errors if any
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState is invalid:");
                foreach (var state in ModelState)
                {
                    if (state.Value.Errors.Count > 0)
                    {
                        foreach (var error in state.Value.Errors)
                        {
                            Console.WriteLine($"- Error in {state.Key}: {error.ErrorMessage}");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("ModelState is valid");
            }

            // Even if ModelState is invalid, try to update the employee
            try
            {
                // Get the existing employee
                var existingEmployee = await _context.Employees.FindAsync(id);
                if (existingEmployee == null)
                {
                    Console.WriteLine($"Employee with ID {id} not found in database");
                    return NotFound();
                }

                Console.WriteLine("Found existing employee, updating properties");

                // Update each property manually and log any issues
                try { existingEmployee.FirstName = employee.FirstName; }
                catch (Exception ex) { Console.WriteLine($"Error updating FirstName: {ex.Message}"); }

                try { existingEmployee.LastName = employee.LastName; }
                catch (Exception ex) { Console.WriteLine($"Error updating LastName: {ex.Message}"); }

                try { existingEmployee.Email = employee.Email; }
                catch (Exception ex) { Console.WriteLine($"Error updating Email: {ex.Message}"); }

                try { existingEmployee.PhoneNumber = employee.PhoneNumber; }
                catch (Exception ex) { Console.WriteLine($"Error updating PhoneNumber: {ex.Message}"); }

                try { existingEmployee.CompanyId = employee.CompanyId; }
                catch (Exception ex) { Console.WriteLine($"Error updating CompanyId: {ex.Message}"); }

                try
                {
                    Console.WriteLine("Attempting to save changes");
                    int rowsAffected = await _context.SaveChangesAsync();
                    Console.WriteLine($"SaveChangesAsync completed, rows affected: {rowsAffected}");

                    // Force a commit
                    using var transaction = await _context.Database.BeginTransactionAsync();
                    await transaction.CommitAsync();
                    Console.WriteLine("Transaction committed");

                    Console.WriteLine("Redirecting to Index");
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception during save: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                    }
                    ModelState.AddModelError("", "An error occurred while saving changes.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Top-level exception: {ex.Message}");
                ModelState.AddModelError("", "An unexpected error occurred.");
            }

            Console.WriteLine("Preparing to re-render Edit view");
            ViewData["CompanyId"] = new SelectList(_context.Companies, "Id", "Name", employee.CompanyId);
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
