using Microsoft.AspNetCore.Mvc;
using Student_Section_ManagementSystemProject.Data;
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
            TempData["Success"] = "Subject added successfully!";
            return RedirectToAction("Index");
        }
        return View(subject);
    }

    // 4️⃣ Show Edit Subject Form
    public IActionResult Edit(int id)
    {
        var subject = _context.Subjects.Find(id);
        if (subject == null) return NotFound();
        return View(subject);
    }

    // 5️⃣ Handle Subject Editing (POST)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Subject subject)
    {
        if (ModelState.IsValid)
        {
            _context.Subjects.Update(subject);
            _context.SaveChanges();
            TempData["Success"] = "Subject updated successfully!";
            return RedirectToAction("Index");
        }
        return View(subject);
    }

    // 6️⃣ Show Delete Confirmation Page
    public IActionResult Delete(int id)
    {
        var subject = _context.Subjects.Find(id);
        if (subject == null) return NotFound();
        return View(subject);
    }

    // 7️⃣ Handle Subject Deletion (POST)
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteConfirmed(int id)
    {
        var subject = _context.Subjects.Find(id);
        if (subject != null)
        {
            _context.Subjects.Remove(subject);
            _context.SaveChanges();
            TempData["Success"] = "Subject deleted successfully!";
        }
        return RedirectToAction("Index");
    }
}