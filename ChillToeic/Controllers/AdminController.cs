using ChillToeic.Models;
using ChillToeic.Models.Entity;
using ChillToeic.Service;
using Microsoft.AspNetCore.Mvc;

namespace ChillToeic.Controllers
{
	public class AdminController : Controller
	{
        private readonly CenterService _dbCenter;
        private readonly CourseService _dbCourse;
        private readonly TestService _dbTest;
        private readonly UserService _dbUser;
        private readonly TestOfUserService _dbTestOfUser;
        private readonly LearningProgressService _dbLearningProgress;

        public AdminController(CenterService centerService, CourseService courseService, 
                                TestService testService, UserService userService, 
                                TestOfUserService testOfUserService, LearningProgressService learningProgressService)
        {
            _dbCenter = centerService;
            _dbCourse = courseService;
            _dbTest = testService;
            _dbUser = userService;
            _dbTestOfUser = testOfUserService;
            _dbLearningProgress = learningProgressService;

        }
        public IActionResult Dashboard()
		{
			return View();
		}

        public IActionResult UserManagement()
        {
            return View();
        }

        public IActionResult CenterManagement()
        {
            var center = _dbCenter.GetAllCenter();
            return View(center);
        }

        public IActionResult CourseManagement()
        {
            return View();
        }

        public IActionResult TestManagement()
        {
            return View();
        }

        public IActionResult UserDetailManagement()
        {
            return View();
        }

        public IActionResult CenterDetailManagement(int id)
        {
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

        public IActionResult CourseDetailManagement()
        {
            return View();
        }

        public IActionResult TestDetailManagement(int id)
        {
            var testDetail = _dbTest.GetTestById2(id).FirstOrDefault();

            if (testDetail == null)
            {
                return NotFound();
            }

            return View(testDetail);
        }
    }
}
