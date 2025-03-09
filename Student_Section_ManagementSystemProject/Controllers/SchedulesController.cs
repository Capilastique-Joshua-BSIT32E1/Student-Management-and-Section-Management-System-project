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
            ModelState.AddModelError("EndTime", "End time must be later than start time.");
            ViewBag.Subjects = _context.Subjects.ToList();
            return View(schedule);
        }

        // ✅ Check for overlapping schedules
        bool isOverlapping = await _context.Schedules.AnyAsync(s =>
            s.SubjectId == schedule.SubjectId &&
            ((schedule.StartTime >= s.StartTime && schedule.StartTime < s.EndTime) ||
             (schedule.EndTime > s.StartTime && schedule.EndTime <= s.EndTime) ||
             (schedule.StartTime <= s.StartTime && schedule.EndTime >= s.EndTime))
        );

        if (isOverlapping)
        {
            ModelState.AddModelError("", "This schedule conflicts with an existing schedule.");
            ViewBag.Subjects = _context.Subjects.ToList();
            return View(schedule);
        }

        _context.Schedules.Add(schedule);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

}
