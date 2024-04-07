using ChillToeic.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChillToeic.Controllers
{
	public class CoursePurchasedController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CoursePurchasedController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult CoursePurchased()
		{
			var course = _context.Courses
				.Include(c => c.Lectures)
				.ThenInclude(l => l.LectureDetails)
				.FirstOrDefault();

			return View(course);
		}
	}
}
