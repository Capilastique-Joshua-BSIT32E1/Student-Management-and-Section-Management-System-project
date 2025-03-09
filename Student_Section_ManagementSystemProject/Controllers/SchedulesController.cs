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
        ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name");
        return View();
    }

    // 3️⃣ Handle Schedule Creation (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Schedule schedule)
    {
        // Validate Subject
        if (!_context.Subjects.Any(s => s.Id == schedule.SubjectId))
        {
            ModelState.AddModelError("SubjectId", "Selected subject does not exist.");
        }

        // Validate StartTime & EndTime
        if (schedule.StartTime >= schedule.EndTime)
        {
            ModelState.AddModelError("EndTime", "End time must be later than start time.");
        }

        // Check for Duplicate Schedule (Same subject, same time)
        if (_context.Schedules.Any(s => s.SubjectId == schedule.SubjectId && s.StartTime == schedule.StartTime && s.EndTime == schedule.EndTime))
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
