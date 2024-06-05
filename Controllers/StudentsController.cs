using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EyeAttend.Data;
using EyeAttend.Models;
using Microsoft.AspNetCore.Hosting;

namespace EyeAttend.Controllers
{
    public class StudentsController : Controller
    {
        private readonly EyeAttendDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentsController(EyeAttendDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
            var eyeAttendDbContext = _context.Students.Include(s => s.Course).Include(s => s.Profile).Include(s => s.SessionYear);
            return View(await eyeAttendDbContext.ToListAsync());
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Course)
                .Include(s => s.Profile)
                .Include(s => s.SessionYear)
                .Include(s => s.StudentPhoto)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName");
            ViewData["ProfileID"] = new SelectList(_context.Profiles, "ProfileID", "ProfileName");
            ViewData["SessionYearId"] = new SelectList(_context.SessionYears, "Id", "Id");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Password,FirstName,LastName,Username,Address,ProfileID,Gender,SessionYearId,CourseId,CreatedAt,UpdatedAt")] Student student, List<IFormFile> imageFiles)
        {
            if (ModelState.IsValid)
            {
                // 1. Save the image files (if any)
                if (imageFiles != null && imageFiles.Count > 0)
                {
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/" + student.ProfileID + "/" + student.Username);
                    Directory.CreateDirectory(uploadsFolder); // Ensure directory exists

                    foreach (var imageFile in imageFiles)
                    {
                        if (imageFile.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                            var filePath = Path.Combine(uploadsFolder, fileName);
                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await imageFile.CopyToAsync(stream);
                            }

                            // Create a new StudentImage record
                            student.StudentPhoto.Add(new StudentPhoto
                            {
                                ImageURL = fileName
                            });
                        }
                    }
                }

                // 2. Save the student data (including images)
                student.CreatedAt = DateTime.Now;
                student.UpdatedAt = DateTime.Now;
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If ModelState is not valid, repopulate ViewData and return the view with errors
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", student.CourseId);
            ViewData["ProfileID"] = new SelectList(_context.Profiles, "ProfileID", "ProfileName", student.ProfileID);
            ViewData["SessionYearId"] = new SelectList(_context.SessionYears, "Id", "Id", student.SessionYearId);
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Profile)
                .Include(s => s.SessionYear)
                .Include(s => s.Course)
                .Include(s => s.StudentPhoto) // Eager load photos
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            PopulateDropDownLists(student); // Move to after fetching the student
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Password,FirstName,LastName,Username,Address,ProfileID,Gender,SessionYearId,CourseId,CreatedAt,UpdatedAt")] Student student, IFormFile[] imageFiles)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 1. Handle Image Uploads (if any)
                    if (imageFiles != null && imageFiles.Length > 0)
                    {
                        var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", student.ProfileID.ToString(), student.Username);
                        Directory.CreateDirectory(uploadsFolder);

                        foreach (var imageFile in imageFiles)
                        {
                            if (imageFile.Length > 0)
                            {
                                var fileName = Guid.NewGuid() + Path.GetExtension(imageFile.FileName);
                                var filePath = Path.Combine(uploadsFolder, fileName);
                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await imageFile.CopyToAsync(stream);
                                }

                                student.StudentPhoto.Add(new StudentPhoto
                                {
                                    ImageURL = fileName // Store filename only
                                });
                            }
                        }
                    }
                    // Set Updated at to current time
                    student.UpdatedAt = DateTime.Now;

                    // 2. Update Student Record
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            PopulateDropDownLists(student);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Course)
                .Include(s => s.Profile)
                .Include(s => s.SessionYear)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            //var studentPhoto = await _context.StudentPhotos.FindAsync(id);
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        private void PopulateDropDownLists(Student student)
        {
            ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "CourseName", student.CourseId);
            ViewData["ProfileID"] = new SelectList(_context.Profiles, "ProfileID", "ProfileName", student.ProfileID);
            ViewData["SessionYearId"] = new SelectList(_context.SessionYears, "Id", "Id", student.SessionYearId);
        }
    }
}
