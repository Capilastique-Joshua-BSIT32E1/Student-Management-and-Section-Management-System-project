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
            .Select(s => new Schedule  // Return a Schedule object
            {
                Id = s.Id,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Subject = s.Subject  // Ensure Subject is included
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
        if (!_context.Subjects.Any(s => s.Id == schedule.SubjectId))
        {
            ModelState.AddModelError("SubjectId", "Selected subject does not exist.");
        }

        if (ModelState.IsValid)
        {
            _context.Schedules.Add(schedule);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Schedule created successfully!";
            return RedirectToAction("Index");
        }

        ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name", schedule.SubjectId);
        TempData["ErrorMessage"] = "Failed to create schedule. Please check your inputs.";
        return View(schedule);
    }
}
