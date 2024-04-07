using ChillToeic.Models;
using ChillToeic.Service;
using Microsoft.AspNetCore.Mvc;

namespace ChillToeic.Controllers
{
    public class CenterDetailController : Controller
    {
        private readonly CourseService _dbCourse;
        private readonly CenterService _dbCenter;
        private readonly TestService _dbTest;

        public CenterDetailController(CourseService dbCourse, CenterService dbCenter, TestService dbTest)
        {
            _dbCourse = dbCourse;
            _dbCenter = dbCenter;
            _dbTest = dbTest;
        }

        public IActionResult CenterDetail(int id)
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
