using ChillToeic.Models;
using ChillToeic.Service;
using Microsoft.AspNetCore.Mvc;

namespace ChillToeic.Controllers
{
    public class CenterProfileController : Controller
    {
        private readonly CourseService _dbCourse;
        private readonly CenterService _dbCenter;
        private readonly TestService _dbTest;

        public CenterProfileController(CourseService dbCourse, CenterService dbCenter, TestService dbTest)
        {
            _dbCourse = dbCourse;
            _dbCenter = dbCenter;
            _dbTest = dbTest;
        }
        public IActionResult CenterProfile(int id)
        {
            id = 2;

            var courses = _dbCourse.GetCoursesByCenterId(id);
            var centers = _dbCenter.GetCentersByCenterId(id);
            var tests = _dbTest.GetTestByCenterId(id);

            var model = new CenterDetailViewModel
            {
                Centers = centers.ToList(),
                Courses = courses.ToList(),
                Tests = tests.ToList()
            };

            return View(model);
        }
    }
}
