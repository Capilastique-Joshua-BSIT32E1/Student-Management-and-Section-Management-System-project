using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Student_Section_ManagementSystemProject.Data;
using Student_Section_ManagementSystemProject.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[Route("Enrollment")]
public class EnrollmentController : Controller
{
    private readonly ApplicationDbContext _context;

    public EnrollmentController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ✅ Enroll a Student in a Specific Schedule (Single Enrollment)
    [HttpGet("Enroll")]
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

        ViewBag.Students = new SelectList(await _context.Students.ToListAsync(), "Id", "Name");
        return View(schedule);
    }

    [HttpGet("Enroll-All")]
    public async Task<IActionResult> Enroll()
    {
        ViewBag.Students = new SelectList(await _context.Students.ToListAsync(), "Id", "Name");

        var schedules = await _context.Schedules
            .Include(s => s.Subject)   // Include Subject for better display
            .Include(s => s.Enrollments) // Include Enrollments
            .ToListAsync();

        // ✅ Debugging: Check if schedules exist
        Console.WriteLine($"Schedules Count: {schedules.Count}");

        if (!schedules.Any())
        {
            TempData["ErrorMessage"] = "No available schedules for enrollment. Please add a schedule.";
        }

        ViewBag.Schedules = schedules;
        return View();
    }



    // ✅ Post method for multiple enrollments
    [HttpPost("EnrollMultiple")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnrollMultiple(int studentId, List<int> scheduleIds)
    {
        var student = await _context.Students.FindAsync(studentId);

        if (student == null || scheduleIds == null || !scheduleIds.Any())
        {
            TempData["ErrorMessage"] = "Invalid student or no schedules selected.";
            return RedirectToAction("Enroll-All");
        }

        List<string> messages = new List<string>();

        foreach (var scheduleId in scheduleIds)
        {
            var schedule = await _context.Schedules
                .Include(s => s.Enrollments)
                .FirstOrDefaultAsync(s => s.Id == scheduleId);

            if (schedule == null)
            {
                messages.Add($"⚠ Schedule ID {scheduleId} not found.");
                continue;
            }

            // ✅ Check if student is already enrolled
            bool isAlreadyEnrolled = schedule.Enrollments.Any(e => e.StudentId == studentId);
            if (isAlreadyEnrolled)
            {
                messages.Add($"⚠ Student is already enrolled in {schedule.Subject.Name}.");
                continue;
            }

            // ✅ Ensure Schedule is Not Full (If you have capacity)
            if (schedule.Capacity > 0 && schedule.Enrollments.Count >= schedule.Capacity)
            {
                messages.Add($"❌ {schedule.Subject.Name} is full.");
                continue;
            }

            // ✅ Enroll the Student
            _context.Enrollments.Add(new Enrollment { ScheduleId = scheduleId, StudentId = studentId });
            messages.Add($"✅ Student successfully enrolled in {schedule.Subject.Name}.");
        }

        await _context.SaveChangesAsync();
        Console.WriteLine("✅ Enrollment Process Completed");

        TempData["SuccessMessage"] = "<ul><li>" + string.Join("</li><li>", messages) + "</li></ul>";
        return RedirectToAction("Enroll-All");
    }

}
