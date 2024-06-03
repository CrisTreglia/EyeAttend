using EyeAttend.Models;
using Microsoft.EntityFrameworkCore;

namespace EyeAttend.Data;

public class EyeAttendDbContext : DbContext
{
    public EyeAttendDbContext(DbContextOptions<EyeAttendDbContext> options)
        : base(options)
    {
    }

        // DbSets for all entities
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<SessionYear> SessionYears { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<StudentPhoto> StudentPhotos { get; set; }
        public DbSet<Subject> Subjects { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Define relationships here 

            // Profile relationships
            modelBuilder.Entity<Admin>()
                .HasOne(a => a.Profile)
                .WithMany() // A Profile can be associated with many Admins
                .HasForeignKey(a => a.ProfileID);

            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Profile)
                .WithMany() // A Profile can be associated with many Staffs
                .HasForeignKey(s => s.ProfileID);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Profile)
                .WithMany()
                .HasForeignKey(s => s.ProfileID);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.SessionYear)
                .WithMany(sy => sy.Students)
                .HasForeignKey(s => s.SessionYearId);

            modelBuilder.Entity<Student>()
                .HasOne(s => s.Course)
                .WithMany(c => c.Students)  // Assuming your Course model has a collection of Students
                .HasForeignKey(s => s.CourseId);

            modelBuilder.Entity<Student>()
                .HasMany(s => s.StudentPhoto)
                .WithOne(sp => sp.Student)
                .HasForeignKey(sp => sp.StudentId);

            // Subject to Course relationship
            modelBuilder.Entity<Subject>()
                .HasOne(s => s.Staff)
                .WithMany()
                .HasForeignKey("StaffId")
                .HasPrincipalKey(p => p.ProfileID);

            // Subject to Staff relationship
            modelBuilder.Entity<Subject>()
                    .HasOne(s => s.Staff)
                    .WithMany() // A Staff can teach many Subjects
                    .HasForeignKey(s => s.StaffId);

            // Attendance relationships
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.Subject)
                .WithMany(s => s.Attendances)
                .HasForeignKey(a => a.SubjectId);

            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.SessionYear)
                .WithMany(sy => sy.Attendances)
                .HasForeignKey(a => a.SessionYearId);

            // StudentPhoto to Student relationship
            modelBuilder.Entity<StudentPhoto>()
                .HasOne(sp => sp.Student)
                .WithMany(s => s.StudentPhoto)
                .HasForeignKey(sp => sp.StudentId);
        }
}

