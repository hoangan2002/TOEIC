using ChillToeic.Models;
using ChillToeic.Models.DTO;
using ChillToeic.Models.Entity;
using ChillToeic.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChillToeic.Controllers
{
    public class CenterProfileController : Controller
    {
        private readonly CourseService _dbCourse;
        private readonly CenterService _dbCenter;
		private readonly EducationService _service;
		private readonly TestService _dbTest;

        public CenterProfileController(EducationService service,CourseService dbCourse, CenterService dbCenter, TestService dbTest)
        {
            _dbCourse = dbCourse;
            _dbCenter = dbCenter;
            _dbTest = dbTest;
			_service = service;

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
		public IActionResult CenterProfile()
        {
           int id = GetUserId();

            var coursesa = _dbCourse.GetCourseByCenterIdApprove(id);
			var courses = _dbCourse.GetCourseByCenterIdNotApprove(id);
			
            var testsa = _dbTest.GetTestByCenterIdApprove(id);
			var tests = _dbTest.GetTestByCenterIdNotApprove(id);
			var model = new CenterDetail
			{
				CoursesApprove = coursesa,
				CoursesNotApprove =courses,
				TestsApprove = testsa,
				TestsNotApprove = tests,
				EducationCenter = _service.GetEducationCenterById(id)
			};

            return View(model);
        }
		public IActionResult CenterDetail(int id)
		{
			var centerIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

			var centers = _dbCenter.GetAllCenter().ToList();
			if (!centers.Any())
			{
				return View("NoCentersAvailable");
			}

			var center = centers.FirstOrDefault(c => c.Id == id);
			if (center == null)
			{
				// If no specific ID, or the provided ID isn't found, use the first one
				center = centers.First();
			}

			var courses = _dbCourse.GetCoursesByCenterId(center.Id).ToList();
			var tests = _dbTest.GetTestByCenterId(center.Id).ToList();

			var model = new CenterDetailViewModel
			{
				Centers = new List<EducationCenter> { center },
				Courses = courses,
				Tests = tests
			};

			return View(model);
		}

	}
}
