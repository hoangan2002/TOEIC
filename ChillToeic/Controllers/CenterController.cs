using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ChillToeic.Controllers
{
    public class CenterController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CenterController(ApplicationDbContext _context)
        {
            this._context = _context;
        }
       public IActionResult Center()
        {
            var centers = _context.EducationCenters.ToList();
            return View(centers);
        }

        public IActionResult Details(int id)
        {
            EducationCenter center = GetCourseByIdFromDatabase(id);

            if (center == null)
            {
                return NotFound();
            }

            ViewData["EducationCenter"] = center;

            return View();
        }

        private List<EducationCenter> GetCoursesFromDatabase()
        {

            return null;

        }

        private EducationCenter GetCourseByIdFromDatabase(int id)
        {
            var centers = GetCoursesFromDatabase();

            foreach (var center in centers)
            {
                if (center.Id == id)
                {
                    return center;
                }
            }

            return null;
        }
    }
}
