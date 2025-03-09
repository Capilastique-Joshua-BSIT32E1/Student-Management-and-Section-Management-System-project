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
        Console.WriteLine("DEBUG: Attempting to create a new schedule...");

        // Extract only time (ignore date)
        TimeSpan startTime = schedule.StartTime.TimeOfDay;
        TimeSpan endTime = schedule.EndTime.TimeOfDay;

        Console.WriteLine($"DEBUG: New Schedule - SubjectId: {schedule.SubjectId}, StartTime: {startTime}, EndTime: {endTime}");

        // Ensure SubjectId exists
        var subjectsInMemory = _context.Subjects.ToList();
        if (!subjectsInMemory.Any(s => s.Id == schedule.SubjectId))
        {
            Console.WriteLine("DEBUG: Subject does not exist!");
            ModelState.AddModelError("SubjectId", "Selected subject does not exist.");
        }

        // ✅ Time validation: Handle cases where end time is on the next day
        if (startTime >= endTime)
        {
            if (schedule.EndTime.Hour < schedule.StartTime.Hour)
            {
                schedule.EndTime = schedule.EndTime.AddDays(1);
            }
            else
            {
                ModelState.AddModelError("EndTime", "End time must be later than start time.");
            }
        }

        // Check for duplicate schedules in memory (based on time, not date)
        if (_context.Schedules.Any(s => s.SubjectId == schedule.SubjectId &&
                                        s.StartTime.TimeOfDay == startTime &&
                                        s.EndTime.TimeOfDay == endTime))
        {
            Console.WriteLine("DEBUG: Duplicate schedule detected!");
            ModelState.AddModelError("", "A schedule with the same subject and time already exists.");
        }

        // Log ModelState errors for debugging
        foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
        {
            Console.WriteLine("DEBUG: ModelState Error - " + error.ErrorMessage);
        }

        if (ModelState.IsValid)
        {
            Console.WriteLine("DEBUG: Schedule is valid, adding to database...");

            // Store only the time (set date to today's date)
            schedule.StartTime = DateTime.Today.Add(startTime);
            schedule.EndTime = DateTime.Today.Add(endTime);

            _context.Schedules.Add(schedule);
            _context.SaveChanges();
            Console.WriteLine("DEBUG: Schedule saved successfully!");

            TempData["ScheduleSuccessMessage"] = "Schedule added successfully!";
            return RedirectToAction("Index");
        }

        Console.WriteLine("DEBUG: ModelState is invalid, returning to Create view.");
        TempData["ScheduleErrorMessage"] = "Failed to create schedule. Please check your inputs.";

        // Ensure subjects are passed to the view
        ViewBag.Subjects = new SelectList(subjectsInMemory, "Id", "Name", schedule.SubjectId);
        return View(schedule);
    }

}
