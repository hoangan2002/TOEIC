using ChillToeic.Models;
using ChillToeic.Models.Entity;
using ChillToeic.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace ChillToeic.Controllers
{
    public class HomeController : Controller
    {
        private readonly CourseService _dbCourse;
        private readonly CenterService _dbCenter;
        private readonly TestService _dbTest;
        private readonly TestOfUserService _dbTestOfUser; // Assuming there's a service for TestOfUser
        private readonly UserService _userService;
        private readonly EducationService _educationService;
        public HomeController(UserService userService, EducationService educationService,CourseService dbCourse, CenterService dbCenter, TestService dbTest, TestOfUserService dbTestOfUser)
        {
            _dbCourse = dbCourse;
            _dbCenter = dbCenter;
            _dbTest = dbTest;
            _dbTestOfUser = dbTestOfUser;
            _userService = userService;
                _educationService = educationService;

		}
		private int GetUserId()
		{
			var claimsIdentity = User.Identity as ClaimsIdentity;
			var nameClaim = claimsIdentity?.FindFirst(ClaimTypes.Name);
			string userName = nameClaim.Value;
			if (_userService.FindUserByUserName(userName) != null)
			{
				return _userService.FindUserByUserName(userName).Id;
			}
			return _educationService.FindEducationCenterByUserName(userName).Id;
		}
		public IActionResult Index()
        {
            int id = 31;
			var courses = _dbCourse.GetAllCourses().ToList(); // Get all courses from all centers
            var centers = _dbCenter.GetAllCenter().ToList(); // Get all centers
            var tests = _dbTest.GetAllTests().ToList(); // Get all tests

            // Get user's average score
            var userAverageScore = _dbTestOfUser.GetAverageScoreByUserId(id);

            // 1. Filter courses by level within +/- 50 of user's average score
            var coursesWithinRange = courses.Where(c => c.level >= userAverageScore - 50 && c.level <= userAverageScore + 50).ToList();

            // 2. Get top 3 courses by QuantityOfRegister
            var top3Courses = courses.OrderByDescending(c => c.QuantityOfRegister).Take(3).ToList();

            var model = new HomeModel
            {
                Centers = centers,
                CoursesWithinRange = coursesWithinRange,
                Top3Courses = top3Courses,
                Tests = tests
            };

            return View(model);
        }
    }
}