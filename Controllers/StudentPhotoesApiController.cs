using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EyeAttend.Data;
using EyeAttend.Models;
using Microsoft.AspNetCore.Hosting;

namespace EyeAttend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentPhotoesApiController : ControllerBase
    {
        private readonly EyeAttendDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public StudentPhotoesApiController(EyeAttendDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/StudentPhotoesApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentPhoto>>> GetStudentPhotos()
        {
            return await _context.StudentPhotos.ToListAsync();
        }

        // GET: api/StudentPhotoesApi/5
        [HttpGet("student/{studentId}")]
        public IActionResult GetStudentPhotos(int studentId)
        {
            // Fetch student details (to get profile ID and username)
            var student = _context.Students.Include(s => s.Profile).FirstOrDefault(s => s.Id == studentId);
            if (student == null)
            {
                return NotFound("Student not found");
            }

            var profileId = student.ProfileID;
            var username = student.Username;

            // Construct the student's photo directory path
            var studentPhotoDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images", profileId.ToString(), username);

            // Ensure the directory exists
            if (!Directory.Exists(studentPhotoDirectory))
            {
                return NotFound("No photos found for this student");
            }

            // Get all image file names in the directory
            var imageFileNames = Directory.GetFiles(studentPhotoDirectory)
                .Select(Path.GetFileName)
                .ToList();

            return Ok(imageFileNames);
        }

        [HttpGet("student/{studentId}/files")] // New endpoint for file access
        public async Task<IActionResult> GetStudentPhotoFiles(int studentId)
        {
            var student = await _context.Students
                .Include(s => s.Profile)
                .FirstOrDefaultAsync(s => s.Id == studentId);

            if (student == null)
            {
                return NotFound("Student not found");
            }

            var profileId = student.ProfileID;
            var username = student.Username;

            var imageDirectory = Path.Combine(_webHostEnvironment.WebRootPath, "images", profileId.ToString(), username);

            if (!Directory.Exists(imageDirectory))
            {
                return NotFound("No images found for this student");
            }

            var imageFiles = Directory.GetFiles(imageDirectory)
                .Select(file => Path.Combine("images", profileId.ToString(), username, Path.GetFileName(file)));

            return Ok(new { ImageFiles = imageFiles }); // Return image file paths
        }

        // PUT: api/StudentPhotoesApi/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudentPhoto(int id, StudentPhoto studentPhoto)
        {
            if (id != studentPhoto.Id)
            {
                return BadRequest();
            }

            _context.Entry(studentPhoto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentPhotoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/StudentPhotoesApi
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<StudentPhoto>> PostStudentPhoto(StudentPhoto studentPhoto)
        {
            _context.StudentPhotos.Add(studentPhoto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudentPhoto", new { id = studentPhoto.Id }, studentPhoto);
        }

        // DELETE: api/StudentPhotoesApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudentPhoto(int id)
        {
            var studentPhoto = await _context.StudentPhotos.FindAsync(id);
            if (studentPhoto == null)
            {
                return NotFound();
            }

            _context.StudentPhotos.Remove(studentPhoto);
            await _context.SaveChangesAsync();

            return NoContent();
        }


        private bool StudentPhotoExists(int id)
        {
            return _context.StudentPhotos.Any(e => e.Id == id);
        }
    }
}
