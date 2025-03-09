using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Student_Section_ManagementSystemProject.Data;
using Student_Section_ManagementSystemProject.Models;
using System;
using System.Linq;

public class SchedulesController : Controller
{
    private readonly ApplicationDbContext _context;

    public SchedulesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // 1️⃣ List All Schedules
    public IActionResult Index()
    {
        TempData.Keep("ScheduleSuccessMessage"); // Persist TempData
        var schedules = _context.Schedules.ToList();
        return View(schedules);
    }

    // 2️⃣ Show Create Schedule Form
    public IActionResult Create()
    {
        var subjects = _context.Subjects.ToList();
        if (!subjects.Any())
        {
            TempData["ScheduleErrorMessage"] = "No subjects available. Please add subjects first.";
            return RedirectToAction("Index");
        }

        ViewBag.Subjects = new SelectList(subjects, "Id", "Name");
        return View();
    }

    // 3️⃣ Handle Schedule Creation (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Schedule schedule)
    {
        if (!_context.Subjects.Any(s => s.Id == schedule.SubjectId))
        {
            ModelState.AddModelError("SubjectId", "Selected subject does not exist.");
        }

        if (schedule.StartTime >= schedule.EndTime)
        {
            ModelState.AddModelError("EndTime", "End time must be later than start time.");
        }

        // Convert times to UTC to avoid time zone issues
        DateTime startUtc = schedule.StartTime.ToUniversalTime();
        DateTime endUtc = schedule.EndTime.ToUniversalTime();

        if (_context.Schedules.Any(s => s.SubjectId == schedule.SubjectId &&
                                        s.StartTime.ToUniversalTime() == startUtc &&
                                        s.EndTime.ToUniversalTime() == endUtc))
        {
            ModelState.AddModelError("", "A schedule with the same subject and time already exists.");
        }

        if (ModelState.IsValid)
        {
            _context.Schedules.Add(schedule);
            _context.SaveChanges();
            TempData["ScheduleSuccessMessage"] = "Schedule added successfully!";
            return RedirectToAction("Index");
        }

        TempData["ScheduleErrorMessage"] = "Failed to create schedule. Please check your inputs.";
        ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name", schedule.SubjectId);
        return View(schedule);
    }
}
