using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace ChillToeic.Controllers
{
	public class CourseController : Controller
	{
		private readonly ApplicationDbContext _context;
	//	private readonly CourseService db;
		private List<Course> courses = new List<Course>();

		public CourseController(ApplicationDbContext _context)
		{
			this._context = _context;
		}

		public IActionResult Course()
		{

			var courses = _context.Courses.ToList();
			return View(courses);
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
	}
}
