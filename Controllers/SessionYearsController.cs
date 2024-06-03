using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EyeAttend.Data;
using EyeAttend.Models;

namespace EyeAttend.Controllers
{
    public class SessionYearsController : Controller
    {
        private readonly EyeAttendDbContext _context;

        public SessionYearsController(EyeAttendDbContext context)
        {
            _context = context;
        }

        // GET: SessionYears
        public async Task<IActionResult> Index()
        {
            return View(await _context.SessionYears.ToListAsync());
        }

        // GET: SessionYears/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessionYear = await _context.SessionYears
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sessionYear == null)
            {
                return NotFound();
            }

            return View(sessionYear);
        }

        // GET: SessionYears/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SessionYears/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SessionStartYear,SessionEndYear")] SessionYear sessionYear)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sessionYear);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sessionYear);
        }

        // GET: SessionYears/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessionYear = await _context.SessionYears.FindAsync(id);
            if (sessionYear == null)
            {
                return NotFound();
            }
            return View(sessionYear);
        }

        // POST: SessionYears/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SessionStartYear,SessionEndYear")] SessionYear sessionYear)
        {
            if (id != sessionYear.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sessionYear);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SessionYearExists(sessionYear.Id))
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
            return View(sessionYear);
        }

        // GET: SessionYears/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sessionYear = await _context.SessionYears
                .FirstOrDefaultAsync(m => m.Id == id);
            if (sessionYear == null)
            {
                return NotFound();
            }

            return View(sessionYear);
        }

        // POST: SessionYears/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sessionYear = await _context.SessionYears.FindAsync(id);
            if (sessionYear != null)
            {
                _context.SessionYears.Remove(sessionYear);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SessionYearExists(int id)
        {
            return _context.SessionYears.Any(e => e.Id == id);
        }
    }
}
