using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CSharp_MVC_AssessmentJS.Models;
using CompanyEmployeeApp.Data;
using System.Linq;

namespace CSharp_MVC_AssessmentJS.Controllers
{
    public class CompaniesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CompaniesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Companies
        public IActionResult Index()
        {
            var companies = _context.Companies.ToList();
            return View(companies);
        }

        // GET: Companies/Show/5
        public IActionResult Show(int? id)
        {
            if (id == null) return NotFound();

            var company = _context.Companies
                .Include(c => c.Employees) // Include associated employees
                .FirstOrDefault(m => m.Id == id);

            if (company == null) return NotFound();

            return View(company);
        }
        
        // GET: Companies/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Companies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create([Bind("Name,Email,LogoPath,Website")] Company company)
        {
            if (ModelState.IsValid)
            {
                _context.Add(company);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();
            var company = _context.Companies.Find(id);
            if (company == null) return NotFound();
            return View(company);
        }

        // POST: Companies/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, [Bind("Id,Name,Email,LogoPath,Website")] Company company)
        {
            if (id != company.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(company);
                _context.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(company);
        }

        // GET: Companies/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            var company = _context.Companies.FirstOrDefault(m => m.Id == id);
            if (company == null) return NotFound();
            return View(company);
        }

        // POST: Companies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var companyToDelete = _context.Companies.Find(id);
            if (companyToDelete != null)
            {
                _context.Companies.Remove(companyToDelete);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
