using ChillToeic.Models;
using ChillToeic.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        private int GetUserId()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var nameClaim = claimsIdentity?.FindFirst(ClaimTypes.Name);
            if (nameClaim != null)
            {
                var user = _dbCenter.FindEducationCenterByUserName(nameClaim.Value);
                if (user != null)
                {
                    return user.Id;
                }
            }

            throw new InvalidOperationException("User is not authenticated properly.");
        }

        public IActionResult CenterDetail()
        {
            int id = GetUserId();

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
