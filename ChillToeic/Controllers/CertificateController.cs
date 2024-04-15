using ChillToeic.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ChillToeic.Models.Entity;

namespace ChillToeic.Controllers
{
	public class CertificateController : Controller
	{
		private readonly ApplicationDbContext _context;

		public CertificateController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Index()
		{
			var certificates = _context.Certificates.Include(c => c.Course).ToList();
			return View(certificates);
		}

		public IActionResult CertificateDetail(int id)
		{



			// Lấy thông tin user từ database
			var certificate = _context.Certificates.FirstOrDefault(c => c.Id == id);

			// Kiểm tra nếu certificate tồn tại
			if (certificate == null)
			{
				return NotFound();
			}

			// Lấy thông tin liên quan của người dùng từ cơ sở dữ liệu và gán vào thuộc tính User của certificate
			var user = _context.Users.FirstOrDefault(u => u.Id == certificate.UserId);
			certificate.User = user;

			// Lấy thông tin liên quan của trung tâm giáo dục từ cơ sở dữ liệu và gán vào thuộc tính EducationCenter của certificate
			var Course = _context.Courses.FirstOrDefault(e => e.Id == certificate.CourseId);
			certificate.Course = Course;
			// Kiểm tra xem education center có tồn tại không
			var EducationCenter = _context.EducationCenters.FirstOrDefault(e => e.Id == Course.EducationCenterId);

			// Đảm bảo rằng user không null trước khi truyền vào view
			// Thêm thông tin cần thiết vào ViewBag để truy cập trong view
			ViewBag.EducationCenterName = EducationCenter.Name;
			// Trả về view với thông tin certificate
			return View(certificate);
		}

	}
}