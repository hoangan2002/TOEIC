using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ChillToeic.Repository;
using ChillToeic.Service;
using ChillToeic.Models.Entity;

namespace ChillToeic.Controllers
{

    public class CenterController : Controller
    {
        private readonly CenterService _dbCenter;

        public CenterController(CenterService dbCenter)
        {
            _dbCenter = dbCenter;
        }

        public IActionResult Center()
        {
            var centers = _dbCenter.GetCentersIsApprove().ToList();
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
