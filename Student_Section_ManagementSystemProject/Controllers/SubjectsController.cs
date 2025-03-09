using Microsoft.AspNetCore.Mvc;
using Student_Section_ManagementSystemProject.Data;
using Student_Section_ManagementSystemProject.Models;
using System.Linq;

public class SubjectsController : Controller
{
    private readonly ApplicationDbContext _context;

    public SubjectsController(ApplicationDbContext context)
    {
        _context = context;
    }

    // 1️⃣ List All Subjects
    public IActionResult Index()
    {
        TempData.Keep("SubjectSuccessMessage"); // Ensures TempData persists until displayed
        var subjects = _context.Subjects.ToList();
        return View(subjects);
    }

    // 2️⃣ Show Create Subject Form
    public IActionResult Create()
    {
        return View();
    }

    // 3️⃣ Handle Subject Creation (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Subject subject)
    {
        if (ModelState.IsValid)
        {
            _context.Subjects.Add(subject);
            _context.SaveChanges();
            TempData["SubjectSuccessMessage"] = "Subject added successfully!";
            return RedirectToAction("Index");
        }
        TempData["SubjectErrorMessage"] = "Failed to add subject. Please check your inputs.";
        return View(subject);
    }

    // 4️⃣ Show Edit Subject Form
    public IActionResult Edit(int id)
    {
        var subject = _context.Subjects.Find(id);
        if (subject == null)
        {
            TempData["SubjectErrorMessage"] = "Subject not found!";
            return RedirectToAction("Index");
        }
        return View(subject);
    }

    // 5️⃣ Handle Subject Editing (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Subject subject)
    {
        if (ModelState.IsValid)
        {
            _context.Subjects.Update(subject);
            _context.SaveChanges();
            TempData["SubjectSuccessMessage"] = "Subject updated successfully!";
            return RedirectToAction("Index");
        }
        TempData["SubjectErrorMessage"] = "Failed to update subject. Please check your inputs.";
        return View(subject);
    }

    // 6️⃣ Handle Subject Deletion
    public IActionResult Delete(int id)
    {
        var subject = _context.Subjects.Find(id);
        if (subject != null)
        {
            _context.Subjects.Remove(subject);
            _context.SaveChanges();
            TempData["SubjectSuccessMessage"] = "Subject successfully deleted!";
        }
        else
        {
            TempData["SubjectErrorMessage"] = "Subject not found!";
        }
        return RedirectToAction("Index");
    }
}
