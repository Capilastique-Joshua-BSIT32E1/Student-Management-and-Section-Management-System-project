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

    // Display List of Schedules
    public async Task<IActionResult> Index()
    {
        var schedules = await _context.Schedules.Include(s => s.Subject).ToListAsync();
        return View(schedules);
    }

    // Show Form to Create Schedule
    public IActionResult Create()
    {
        ViewBag.Subjects = _context.Subjects.ToList();
        return View();
    }

    // Store Schedule
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

        // ✅ Prevent Exact Duplicate Schedules
        bool isDuplicate = await _context.Schedules.AnyAsync(s =>
            s.SubjectId == schedule.SubjectId &&
            s.StartTime == schedule.StartTime &&
            s.EndTime == schedule.EndTime
        );

        if (isDuplicate)
        {
            TempData["ScheduleErrorMessage"] = "This schedule already exists.";
            ViewBag.Subjects = _context.Subjects.ToList();
            return View(schedule);
        }

        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();

        Console.WriteLine($"✅ Schedule Created! Total Schedules: {_context.Schedules.Count()}");

        // ✅ Store success message
        TempData["ScheduleSuccessMessage"] = "Schedule added successfully!";
        return RedirectToAction("Index");
    }


}
