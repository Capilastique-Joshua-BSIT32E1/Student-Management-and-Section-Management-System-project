using Microsoft.AspNetCore.Mvc;

namespace Student_Section_ManagementSystemProject.Data
{
    public class ApplicationDbContext : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
