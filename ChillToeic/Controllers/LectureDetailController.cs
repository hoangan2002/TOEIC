using ChillToeic.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ChillToeic.Controllers
{
	public class LectureDetailController : Controller
	{
		
		private readonly ApplicationDbContext _context;
		public LectureDetailController(ApplicationDbContext _context)
		{
			this._context = _context;
		}

		public IActionResult Index()
		{
			var _lectureDetail = _context.LectureDetails.Include(x => x.LectureType).ToList();
			return View(_lectureDetail);
		}

		public IActionResult Video(int lectureId)
		{
			var lectureDetail = _context.LectureDetails
				.Include(ld => ld.Lecture)
				.SingleOrDefault(ld => ld.Id == lectureId);

			if (lectureDetail == null)
			{
				return NotFound(); // Trả về trang 404 nếu không tìm thấy lecturedetail
			}

			return View("Video", lectureDetail);
		}


		public IActionResult Document(int lectureId)
		{
			var lectureDetail = _context.LectureDetails
				.Include(ld => ld.Lecture)
				.SingleOrDefault(ld => ld.Id == lectureId);

			if (lectureDetail == null)
			{
				return NotFound(); // Trả về trang 404 nếu không tìm thấy lecturedetail
			}

			return View("Document", lectureDetail);
		}

	}
}
