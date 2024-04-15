using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using ChillToeic.Service;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChillToeic.Controllers
{
	public class CourseController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly CourseService db;
		private List<Course> courses = new List<Course>();
		private readonly UserService _userService;
		private readonly EducationService _educationService;
		public CourseController(ApplicationDbContext _context, EducationService educationService, UserService userService)
		{
			this._context = _context;
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
		public IActionResult Course()
		{
			int userid = GetUserId();
			var purchasedCourses = _context.Orders
								.Where(o => o.UserId == userid && o.Status == true)
								.Select(o => o.CourseId)
								.ToList();
			var courses = _context.Courses.ToList();
			var coursesNotPurchased = courses.Where(c => !purchasedCourses.Contains(c.Id)).ToList();
			return View(coursesNotPurchased);
		}

		public IActionResult Details(int id)
		{
			Course course = GetCourseByIdFromDatabase(id);

			if (course == null)
			{
				return NotFound();
			}

			ViewData["Course"] = course;

			return View();
		}

		private List<Course> GetCoursesFromDatabase()
		{

			return null;

		}

		private Course GetCourseByIdFromDatabase(int id)
		{
			var courses = GetCoursesFromDatabase();

			foreach (var course in courses)
			{
				if (course.Id == id)
				{
					return course;
				}
			}

			return null;
		}
		[HttpGet]
		public JsonResult SearchCourses(string searchTerm)
		{
			// Tìm kiếm khóa học dựa trên tên (hoặc bất kỳ điều kiện nào khác)
			var courses = _context.Courses
				.Where(course => course.Name.Contains(searchTerm))
				.Select(course => new
				{
					Id = course.Id,
					Name = course.Name
				})
				.ToList();

			// Trả về kết quả dưới dạng JSON
			return Json(courses);
		}
	}
}
