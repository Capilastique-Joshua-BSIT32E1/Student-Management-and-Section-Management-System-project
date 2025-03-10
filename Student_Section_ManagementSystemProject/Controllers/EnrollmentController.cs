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
        var students = await _context.Students.ToListAsync();
        var schedules = await _context.Schedules
            .Include(s => s.Subject)
            .Include(s => s.Enrollments)
            .ToListAsync();

        Console.WriteLine($"Schedules Count: {schedules.Count}");

        if (!schedules.Any())
        {
            TempData["ErrorMessage"] = "No available schedules for enrollment. Please add a schedule.";
        }

        var model = new Enrollment
        {
            StudentsList = students,
            SchedulesList = schedules
        };

        return View(model);
    }
    

    [HttpPost("EnrollMultiple")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> EnrollMultiple(int StudentId, List<int> SelectedScheduleIds)
    {
        var student = await _context.Students.FindAsync(StudentId);

        if (student == null || SelectedScheduleIds == null || !SelectedScheduleIds.Any())
        {
            TempData["ErrorMessage"] = "Invalid student or no schedules selected.";
            return RedirectToAction("Enroll-All");
        }

        List<string> messages = new List<string>();

        foreach (var scheduleId in SelectedScheduleIds)
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
            bool isAlreadyEnrolled = schedule.Enrollments.Any(e => e.StudentId == StudentId);
            if (isAlreadyEnrolled)
            {
                messages.Add($"⚠ Student is already enrolled in {schedule.Subject.Name}.");
                continue;
            }

            // ✅ Ensure Schedule is Not Full
            if (schedule.Capacity > 0 && schedule.Enrollments.Count >= schedule.Capacity)
            {
                messages.Add($"❌ {schedule.Subject.Name} is full.");
                continue;
            }

            // ✅ Enroll the Student
            _context.Enrollments.Add(new Enrollment { ScheduleId = scheduleId, StudentId = StudentId });
            messages.Add($"✅ Student successfully enrolled in {schedule.Subject.Name}.");
        }

        await _context.SaveChangesAsync();
        Console.WriteLine("✅ Enrollment Process Completed");

        TempData["SuccessMessage"] = "<ul><li>" + string.Join("</li><li>", messages) + "</li></ul>";
        return RedirectToAction("Enroll-All");
    }

}
