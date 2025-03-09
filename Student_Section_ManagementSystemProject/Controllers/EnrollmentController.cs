using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_Section_ManagementSystemProject.Data;
using Student_Section_ManagementSystemProject.Models;
using System.Linq;
using System.Threading.Tasks;

public class EnrollmentController : Controller
{
    private readonly ApplicationDbContext _context;

    public EnrollmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Display Enrollment Page
    public async Task<IActionResult> Enroll(int scheduleId)
    {
        var schedule = await _context.Schedules
            .Include(s => s.Subject)
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Student)
            .FirstOrDefaultAsync(s => s.Id == scheduleId);

        if (schedule == null)
        {
            TempData["ErrorMessage"] = "Schedule not found!";
            return RedirectToAction("Index", "Schedules");
        }

        ViewBag.Students = new SelectList(_context.Students, "Id", "Name");
        return View(schedule);
    }

    // Handle Enrollment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Enroll(int scheduleId, int studentId)
    {
        var schedule = await _context.Schedules.FindAsync(scheduleId);
        var student = await _context.Students.FindAsync(studentId);

        if (schedule == null || student == null)
        {
            TempData["ErrorMessage"] = "Invalid schedule or student.";
            return RedirectToAction("Index", "Schedules");
        }

        // Check if student is already enrolled
        bool isAlreadyEnrolled = await _context.Enrollments
            .AnyAsync(e => e.ScheduleId == scheduleId && e.StudentId == studentId);

        if (isAlreadyEnrolled)
        {
            TempData["ErrorMessage"] = "Student is already enrolled in this schedule.";
        }
        else
        {
            _context.Enrollments.Add(new Enrollment { ScheduleId = scheduleId, StudentId = studentId });
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Student enrolled successfully!";
        }

        return RedirectToAction("Enroll", new { scheduleId });
    }

    // Handle Unenrollment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Unenroll(int scheduleId, int studentId)
    {
        var enrollment = await _context.Enrollments
            .FirstOrDefaultAsync(e => e.ScheduleId == scheduleId && e.StudentId == studentId);

        if (enrollment != null)
        {
            _context.Enrollments.Remove(enrollment);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Student removed from schedule.";
        }
        else
        {
            TempData["ErrorMessage"] = "Enrollment not found.";
        }

        return RedirectToAction("Enroll", new { scheduleId });
    }
}
