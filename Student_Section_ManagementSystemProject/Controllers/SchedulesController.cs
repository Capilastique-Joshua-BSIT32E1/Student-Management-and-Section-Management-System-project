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
        Console.WriteLine($"DEBUG: New Schedule - SubjectId: {schedule.SubjectId}, StartTime: {schedule.StartTime.TimeOfDay}, EndTime: {schedule.EndTime.TimeOfDay}");

        // Debugging: Print all available subjects
        var subjectsInMemory = _context.Subjects.ToList();
        Console.WriteLine("DEBUG: Available Subjects Count = " + subjectsInMemory.Count);
        foreach (var subj in subjectsInMemory)
        {
            Console.WriteLine($"DEBUG: Subject - Id: {subj.Id}, Name: {subj.Name}");
        }

        // Ensure SubjectId exists
        if (!subjectsInMemory.Any(s => s.Id == schedule.SubjectId))
        {
            Console.WriteLine("DEBUG: Subject does not exist!");
            ModelState.AddModelError("SubjectId", "Selected subject does not exist.");
        }

        // Extract only time (ignore date)
        TimeSpan startTime = schedule.StartTime.TimeOfDay;
        TimeSpan endTime = schedule.EndTime.TimeOfDay;

        if (startTime >= endTime)
        {
            Console.WriteLine("DEBUG: StartTime is greater than or equal to EndTime!");
            ModelState.AddModelError("EndTime", "End time must be later than start time.");
        }

        // Check for duplicate schedules in memory (based on time, not date)
        if (_context.Schedules.Any(s => s.SubjectId == schedule.SubjectId &&
                                        s.StartTime.TimeOfDay == startTime &&
                                        s.EndTime.TimeOfDay == endTime))
        {
            Console.WriteLine("DEBUG: Duplicate schedule detected!");
            ModelState.AddModelError("", "A schedule with the same subject and time already exists.");
        }

        if (ModelState.IsValid)
        {
            Console.WriteLine("DEBUG: Schedule is valid, adding to database...");
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
