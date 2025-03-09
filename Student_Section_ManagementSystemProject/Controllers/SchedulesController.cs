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
            .Select(s => new
            {
                s.Id,
                s.StartTime,
                s.EndTime,
                SubjectName = s.Subject.Name
            })
            .ToList();

        return View(schedules);
    }

        public IActionResult Create()
        {
        ViewBag.Subjects = new SelectList(_context.Subjects, "Id", "Name");
            return View();
        }

        [HttpPost]
        public IActionResult Create(Schedule schedule)
        {
            if (ModelState.IsValid)
            {
                _context.Schedules.Add(schedule);
                _context.SaveChanges();
            TempData["SuccessMessage"] = "Schedule created successfully!";
            return RedirectToAction("Index");
            }
            return View(schedule);
        }
