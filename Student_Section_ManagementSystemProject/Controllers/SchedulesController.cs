using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Student_Section_ManagementSystemProject.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

public class ScheduleController : Controller
{
    private readonly ApplicationDbContext _context;

    public ScheduleController(ApplicationDbContext context)
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
        if (ModelState.IsValid)
        {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        ViewBag.Subjects = _context.Subjects.ToList();
        return View(schedule);
    }

    // Delete Schedule
    public async Task<IActionResult> Delete(int id)
        {
        var schedule = await _context.Schedules.FindAsync(id);
        if (schedule != null)
            {
            _context.Schedules.Remove(schedule);
            await _context.SaveChangesAsync();
        }
        return RedirectToAction("Index");
    }
}
