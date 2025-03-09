using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_Section_ManagementSystemProject.Data;
using Student_Section_ManagementSystemProject.Models;
using System.Linq;

public class EnrollmentController : Controller
{
    private readonly ApplicationDbContext _context;

    public EnrollmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // 1️⃣ Show Enrollment Page
    public IActionResult Enroll(int scheduleId)
    {
        var schedule = _context.Schedules
            .Include(s => s.Subject)
            .Include(s => s.Enrollments)
            .ThenInclude(e => e.Student)
            .FirstOrDefault(s => s.Id == scheduleId);

        if (schedule == null)
        {
            TempData["ErrorMessage"] = "Schedule not found!";
            return RedirectToAction("Index", "Schedules");
        }

        ViewBag.Students = new SelectList(_context.Students, "Id", "Name");
        return View(schedule);
    }

    // 2️⃣ Handle Enrollment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Enroll(int scheduleId, int studentId)
    {
        var schedule = _context.Schedules.Find(scheduleId);
        var student = _context.Students.Find(studentId);

        if (schedule == null || student == null)
        {
            TempData["ErrorMessage"] = "Invalid schedule or student.";
            return RedirectToAction("Index", "Schedules");
        }

        if (_context.Enrollments.Any(e => e.ScheduleId == scheduleId && e.StudentId == studentId))
        {
            TempData["ErrorMessage"] = "Student is already enrolled in this schedule.";
        }
        else
        {
            _context.Enrollments.Add(new Enrollment { ScheduleId = scheduleId, StudentId = studentId });
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Student enrolled successfully!";
        }

        return RedirectToAction("Enroll", new { scheduleId });
    }

    // 3️⃣ Handle Unenrollment
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Unenroll(int scheduleId, int studentId)
    {
        var enrollment = _context.Enrollments
            .FirstOrDefault(e => e.ScheduleId == scheduleId && e.StudentId == studentId);

        if (enrollment != null)
        {
            _context.Enrollments.Remove(enrollment);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Student removed from schedule.";
        }
        else
        {
            TempData["ErrorMessage"] = "Enrollment not found.";
        }

        return RedirectToAction("Enroll", new { scheduleId });
    }
}
