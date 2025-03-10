using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Section_ManagementSystemProject.Data;
using Student_Section_ManagementSystemProject.Models;
using System.Linq;
using System.Threading.Tasks;

namespace Student_Section_ManagementSystemProject.Controllers
{
    public class EnrollmentsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EnrollmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // ✅ Enroll Student to Schedule
        [HttpPost]
        public async Task<IActionResult> Enroll(int scheduleId, int studentId)
        {
            var schedule = await _context.Schedules.Include(s => s.EnrolledStudents)
                .FirstOrDefaultAsync(s => s.Id == scheduleId);

            var student = await _context.Students.FindAsync(studentId);

            if (schedule == null || student == null)
            {
                TempData["EnrollmentErrorMessage"] = "Failed to enroll. Invalid schedule or student.";
                return RedirectToAction("Details", "Schedules", new { id = scheduleId });
            }

            // ✅ Prevent Duplicate Enrollment
            if (schedule.EnrolledStudents.Any(s => s.Id == studentId))
            {
                TempData["EnrollmentErrorMessage"] = "This student is already enrolled in this schedule.";
                return RedirectToAction("Details", "Schedules", new { id = scheduleId });
            }

            // ✅ Enroll Student
            schedule.EnrolledStudents.Add(student);
            student.ScheduleId = scheduleId;
            await _context.SaveChangesAsync();

            TempData["EnrollmentSuccessMessage"] = $"Student '{student.Name}' successfully enrolled!";
            return RedirectToAction("Details", "Schedules", new { id = scheduleId });
        }

        // ✅ Remove Student from Schedule
        [HttpPost]
        public async Task<IActionResult> Remove(int scheduleId, int studentId)
        {
            var schedule = await _context.Schedules.Include(s => s.EnrolledStudents)
                .FirstOrDefaultAsync(s => s.Id == scheduleId);

            var student = await _context.Students.FindAsync(studentId);

            if (schedule == null || student == null)
            {
                TempData["EnrollmentErrorMessage"] = "Failed to remove student.";
                return RedirectToAction("Details", "Schedules", new { id = scheduleId });
            }

            // ✅ Remove Student
            schedule.EnrolledStudents.Remove(student);
            student.ScheduleId = null;
            await _context.SaveChangesAsync();

            TempData["EnrollmentSuccessMessage"] = $"Student '{student.Name}' successfully removed!";
            return RedirectToAction("Details", "Schedules", new { id = scheduleId });
        }
    }
}
