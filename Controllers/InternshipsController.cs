using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Internship_Application_Management_System.Data;
using Internship_Application_Management_System.Models;
using Microsoft.AspNetCore.Authorization;

namespace Internship_Application_Management_System.Controllers
{
    [Authorize]
    public class InternshipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InternshipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Internships
        public async Task<IActionResult> Index()
        {
            return View(await _context.Internships.ToListAsync());
        }

        // GET: Internships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internship = await _context.Internships
                .FirstOrDefaultAsync(m => m.Id == id);
            if (internship == null)
            {
                return NotFound();
            }

            return View(internship);
        }

        // GET: Internships/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Internships/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,Location,ClosingDate")] Internship internship)
        {
            if (ModelState.IsValid)
            {
                _context.Add(internship);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(internship);
        }

        // GET: Internships/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internship = await _context.Internships.FindAsync(id);
            if (internship == null)
            {
                return NotFound();
            }
            return View(internship);
        }

        // POST: Internships/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,Location,ClosingDate")] Internship internship)
        {
            if (id != internship.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(internship);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InternshipExists(internship.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(internship);
        }

        // GET: Internships/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var internship = await _context.Internships
                .FirstOrDefaultAsync(m => m.Id == id);
            if (internship == null)
            {
                return NotFound();
            }

            return View(internship);
        }

        // POST: Internships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var internship = await _context.Internships.FindAsync(id);
            if (internship != null)
            {
                _context.Internships.Remove(internship);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InternshipExists(int id)
        {
            return _context.Internships.Any(e => e.Id == id);
        }
    }
}
