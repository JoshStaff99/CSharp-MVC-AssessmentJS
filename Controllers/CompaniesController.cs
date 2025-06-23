using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CSharp_MVC_AssessmentJS.Models;
using CompanyEmployeeApp.Data;
using System.Linq;
using System.IO;

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
        public async Task<IActionResult> Create(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.LogoFile != null && company.LogoFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logos");
                    Directory.CreateDirectory(uploadsFolder); // ensure the folder exists

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(company.LogoFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await company.LogoFile.CopyToAsync(stream);
                    }

                    company.LogoPath = "/logos/" + uniqueFileName;
                }

                _context.Add(company);
                await _context.SaveChangesAsync();

                TempData["SuccessCreateCompany"] = "Company Created successfully!";
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
        public async Task<IActionResult> Edit(int id, Company company)
        {
            if (id != company.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var existingCompany = await _context.Companies.FindAsync(id);
                if (existingCompany == null) return NotFound();

                existingCompany.Name = company.Name;
                existingCompany.Email = company.Email;
                existingCompany.Website = company.Website;

                if (company.LogoFile != null && company.LogoFile.Length > 0)
                {
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "logos");
                    Directory.CreateDirectory(uploadsFolder);

                    var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(company.LogoFile.FileName);
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await company.LogoFile.CopyToAsync(stream);
                    }

                    existingCompany.LogoPath = "/logos/" + uniqueFileName;
                }

                _context.Update(existingCompany);
                await _context.SaveChangesAsync();

                TempData["SuccessEditCompany"] = "Company edited successfully!";
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
        public IActionResult DeleteConfirmed(int id, string returnUrl)
        {
            var companyToDelete = _context.Companies.Find(id);
            if (companyToDelete != null)
            {
                bool hasEmployees = _context.Employees.Any(e => e.CompanyId == companyToDelete.Id);
                if (hasEmployees)
                {
                    TempData["DeleteError"] = true;
                    TempData["ErrorMessage"] = "Cannot delete company: there are employees associated with this company.";

                    // Ensure the return URL is local to prevent open redirect attacks
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);

                    return RedirectToAction(nameof(Index));
                }

                _context.Companies.Remove(companyToDelete);
                _context.SaveChanges();

                TempData["SuccessMessageCompany"] = "Company deleted successfully!";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
