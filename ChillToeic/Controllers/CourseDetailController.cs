using ChillToeic.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChillToeic.Controllers
{
	public class CourseDetailController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CourseDetailController(ApplicationDbContext context)
		{
			_context = context;
		}
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null)
			{
				return NotFound();
			}

			var course = await _context.Courses
				.FirstOrDefaultAsync(m => m.Id == id);
			if (course == null)
			{
				return NotFound();
			}

			return View("DetailsCourse", course);
		}
	}
}
