using ChillToeic.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChillToeic.Controllers
{
	public class LecturesController : Controller
	{
		private readonly ApplicationDbContext _context;

		public LecturesController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var lectures = _context.Lectures
				.Include(l => l.LectureDetails)
				.ToList();
			return View(lectures);
		}
	}
}
