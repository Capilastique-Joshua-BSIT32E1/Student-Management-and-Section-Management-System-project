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
        var schedules = _context.Schedules
            .Select(s => new Schedule
            {
                Id = s.Id,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Subject = s.Subject
            })
            .ToList();

        return View(schedules);
    }

    // 2️⃣ Show Create Schedule Form
    public IActionResult Create()
    {
        ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name");
        return View();
    }

    // 3️⃣ Handle Schedule Creation (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Schedule schedule)
    {
        Console.WriteLine($"DEBUG: StartTime = {schedule.StartTime}, EndTime = {schedule.EndTime}");

        // Ensure subject exists
        if (!_context.Subjects.Any(s => s.Id == schedule.SubjectId))
        {
            ModelState.AddModelError("SubjectId", "Selected subject does not exist.");
        }

        // ✅ FIX: Extra validation before saving
        if (schedule.StartTime >= schedule.EndTime)
        {
            ModelState.AddModelError("EndTime", "End time must be later than start time.");
        }

        if (ModelState.IsValid)
        {
            _context.Schedules.Add(schedule);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Schedule created successfully!";
            return RedirectToAction("Index"); // ✅ Return after successful creation
        }

        // This ensures all paths return a value
        ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name", schedule.SubjectId);
        TempData["ErrorMessage"] = "Failed to create schedule. Please check your inputs.";
        return View(schedule);
    }
}
