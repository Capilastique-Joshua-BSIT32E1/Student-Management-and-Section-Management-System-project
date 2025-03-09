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
        TempData.Keep("ScheduleSuccessMessage");
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
        // 🕒 Extract TimeSpan from DateTime
        TimeSpan startTime = schedule.StartTime.TimeOfDay;
        TimeSpan endTime = schedule.EndTime.TimeOfDay;

        // 📌 Validate Subject Exists
        var subjectExists = _context.Subjects.Any(s => s.Id == schedule.SubjectId);
        if (!subjectExists)
        {
            ModelState.AddModelError("SubjectId", "Selected subject does not exist.");
        }

        // 🔍 Validate Time Logic
        if (startTime >= endTime)
        {
            if (schedule.EndTime.Hour < schedule.StartTime.Hour) // Handle cases where endTime is past midnight
            {
                schedule.EndTime = schedule.EndTime.AddDays(1);
            }
            else
            {
                ModelState.AddModelError("EndTime", "End time must be later than start time.");
            }
        }

        // ❌ If errors exist, return to form
        if (!ModelState.IsValid)
        {
            ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name", schedule.SubjectId);
            return View(schedule);
        }

        // ✅ Save Schedule with Correct Date
        schedule.StartTime = DateTime.Today.Add(startTime);
        schedule.EndTime = DateTime.Today.Add(endTime);
        _context.Schedules.Add(schedule);
        _context.SaveChanges();

        TempData["ScheduleSuccessMessage"] = "Schedule added successfully!";
        return RedirectToAction("Index");
    }

}
