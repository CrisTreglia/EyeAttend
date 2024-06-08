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
using Microsoft.Extensions.Hosting;

namespace EyeAttend.Controllers
{
    public class StudentPhotoesController : Controller
    {
        private readonly EyeAttendDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentPhotoesController(EyeAttendDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: StudentPhotoes
        public async Task<IActionResult> Index()
        {
            var eyeAttendDbContext = _context.StudentPhotos.Include(s => s.Student);
            return View(await eyeAttendDbContext.ToListAsync());
        }

        // GET: StudentPhotoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentPhoto = await _context.StudentPhotos
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentPhoto == null)
            {
                return NotFound();
            }

            return View(studentPhoto);
        }

        // GET: StudentPhotoes/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Email");
            return View();
        }

        // POST: StudentPhotoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int studentId, List<IFormFile> imageFiles)
        {
            foreach (var imageFile in imageFiles)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    // Create directory if it doesn't exist
                    Directory.CreateDirectory(uploadsFolder);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(stream);
                    }

                    var studentPhoto = new StudentPhoto
                    {
                        StudentId = studentId,
                        ImageURL = fileName // Store the file name only
                    };

                    _context.StudentPhotos.Add(studentPhoto);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Students", "Index");
        }

        // GET: StudentPhotoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentPhoto = await _context.StudentPhotos.FindAsync(id);
            if (studentPhoto == null)
            {
                return NotFound();
            }
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Email", studentPhoto.StudentId);
            return View(studentPhoto);
        }

        // POST: StudentPhotoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ImageURL,StudentId")] StudentPhoto studentPhoto)
        {
            if (id != studentPhoto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentPhoto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentPhotoExists(studentPhoto.Id))
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
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Email", studentPhoto.StudentId);
            return View(studentPhoto);
        }

        // GET: StudentPhotoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentPhoto = await _context.StudentPhotos
                .Include(s => s.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (studentPhoto == null)
            {
                return NotFound();
            }

            return View(studentPhoto);
        }

        // POST: StudentPhotoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentPhoto = await _context.StudentPhotos.Include(s => s.Student).FirstOrDefaultAsync(m => m.Id == id);
            var profileId = studentPhoto.Student.ProfileID;
            var username = studentPhoto.Student.Username;
            var fileName = Path.GetFileName(studentPhoto.ImageURL);
            var ExitingFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/" + profileId + "/" + username + "/", fileName);
            if (studentPhoto != null)
            {
                System.IO.File.Delete(ExitingFile);
                _context.StudentPhotos.Remove(studentPhoto);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Students");
        }

        private bool StudentPhotoExists(int id)
        {
            return _context.StudentPhotos.Any(e => e.Id == id);
        }
    }
}
