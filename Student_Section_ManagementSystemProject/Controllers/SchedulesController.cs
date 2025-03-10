using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Section_ManagementSystemProject.Data;
using System;
using System.Linq;
using System.Threading.Tasks;
using Student_Section_ManagementSystemProject.Models;

public class SchedulesController : Controller
{
    private readonly ApplicationDbContext _context;

    public SchedulesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // ✅ Display List of Schedules
    public async Task<IActionResult> Index()
    {
        var schedules = await _context.Schedules.Include(s => s.Subject).ToListAsync();
        return View(schedules);
    }

    // ✅ Show Form to Create Schedule
    public IActionResult Create()
    {
        ViewBag.Subjects = _context.Subjects.ToList();
        return View();
    }

    // ✅ Store Schedule
    [HttpPost]
    public async Task<IActionResult> Create(Schedule schedule)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Subjects = _context.Subjects.ToList();
            return View(schedule);
        }

        // ✅ Ensure EndTime is later than StartTime
        if (schedule.EndTime <= schedule.StartTime)
        {
            TempData["ScheduleErrorMessage"] = "End time must be later than start time.";
            ViewBag.Subjects = _context.Subjects.ToList();
            return View(schedule);
        }

        // ✅ Prevent Exact Duplicate Schedules for the SAME Subject
        bool isDuplicate = await _context.Schedules.AnyAsync(s =>
            s.SubjectId == schedule.SubjectId &&
            s.StartTime == schedule.StartTime &&
            s.EndTime == schedule.EndTime
        );

        if (isDuplicate)
        {
            TempData["ScheduleErrorMessage"] = "This schedule already exists for this subject.";
            ViewBag.Subjects = _context.Subjects.ToList();
            return View(schedule);
        }

        // ✅ Prevent Conflict in Different Subjects (Same Time)
        bool isTimeConflict = await _context.Schedules.AnyAsync(s =>
    s.SubjectId != schedule.SubjectId && // ✅ Prevent the same subject from checking itself
    s.StartTime < schedule.EndTime &&
    s.EndTime > schedule.StartTime
);

        if (isTimeConflict)
        {
            TempData["ScheduleErrorMessage"] = "This time slot is already taken by another subject.";
            ViewBag.Subjects = _context.Subjects.ToList();
            return View(schedule);
        }

        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();

        // ✅ Success Message
        TempData["ScheduleSuccessMessage"] = "Schedule added successfully!";
        return RedirectToAction("Index");
    }

    // ✅ Display Schedule Details
    public IActionResult Details(int id)
    {
        var schedule = _context.Schedules
            .Include(s => s.EnrolledStudents)
            .Include(s => s.Subject)
            .FirstOrDefault(s => s.Id == id);

        if (schedule == null)
            return NotFound();

        // ✅ Allow ALL students to enroll (no restrictions)
        ViewBag.Students = _context.Students.ToList();

        return View(schedule);
    }
}
