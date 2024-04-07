using ChillToeic.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

			var certificate = _context.Certificates
				.Include(c => c.Course)
				.Include(c => c.User)
				.FirstOrDefault(c => c.Id == id);

			return View(certificate);


		}

	}
}
