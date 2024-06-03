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
    public class AttendancesController : Controller
    {
        private readonly EyeAttendDbContext _context;

        public AttendancesController(EyeAttendDbContext context)
        {
            _context = context;
        }

        // GET: Attendances
        public async Task<IActionResult> Index()
        {
            var eyeAttendDbContext = _context.Attendances.Include(a => a.SessionYear).Include(a => a.Subject);
            return View(await eyeAttendDbContext.ToListAsync());
        }

        // GET: Attendances/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.SessionYear)
                .Include(a => a.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendances/Create
        public IActionResult Create()
        {
            ViewData["SessionYearId"] = new SelectList(_context.SessionYears, "Id", "Id");
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "SubjectName");
            return View();
        }

        // POST: Attendances/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,SubjectId,AttendanceDate,SessionYearId,CreatedAt,UpdatedAt")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendance);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SessionYearId"] = new SelectList(_context.SessionYears, "Id", "Id", attendance.SessionYearId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "SubjectName", attendance.SubjectId);
            return View(attendance);
        }

        // GET: Attendances/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }
            ViewData["SessionYearId"] = new SelectList(_context.SessionYears, "Id", "Id", attendance.SessionYearId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "SubjectName", attendance.SubjectId);
            return View(attendance);
        }

        // POST: Attendances/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,SubjectId,AttendanceDate,SessionYearId,CreatedAt,UpdatedAt")] Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.Id))
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
            ViewData["SessionYearId"] = new SelectList(_context.SessionYears, "Id", "Id", attendance.SessionYearId);
            ViewData["SubjectId"] = new SelectList(_context.Subjects, "Id", "SubjectName", attendance.SubjectId);
            return View(attendance);
        }

        // GET: Attendances/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendances
                .Include(a => a.SessionYear)
                .Include(a => a.Subject)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendances/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendances.FindAsync(id);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendances.Any(e => e.Id == id);
        }
    }
}
