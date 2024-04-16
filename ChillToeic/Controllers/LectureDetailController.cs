using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using ChillToeic.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ChillToeic.Controllers
{
	public class LectureDetailController : Controller
	{
		
		private readonly ApplicationDbContext _context;
		private readonly UserService _userService;
		public LectureDetailController(ApplicationDbContext _context, UserService userService)
		{
			this._context = _context;
            _userService= userService;

        }

		public IActionResult Index(int id)
		{
			var _lectureDetail = _context.LectureDetails.Include(x => x.LectureType).Where(x => x.LectureId == id).ToList();
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
		public IActionResult DocumentViewer(int lectureDetailId, int action)
		{


			// Tìm lectureDetail dựa trên lectureDetailId và nạp các thuộc tính liên quan
			var lectureDetail = _context.LectureDetails
				.Include(ld => ld.Lecture)
				.ThenInclude(l => l.LectureDetails)
				.Include(ld => ld.LectureType)
				.FirstOrDefault(ld => ld.Id == lectureDetailId);

			List<LectureDetail> lectureDetailsList = _context.LectureDetails.Where(c => c.LectureId == lectureDetail.LectureId).ToList();
			int index = lectureDetailsList.IndexOf(lectureDetail);
			lectureDetail = lectureDetailsList[index + action];
			// Kiểm tra nếu lectureDetail là null
			if (lectureDetail == null)
			{
				return NotFound();
			}
			if (index + action == 0)
			{
				ViewBag.location = -1;
			}
			else if (index + action == lectureDetailsList.Count - 1)
			{
				ViewBag.location = 1;
			}
			else
			{
				ViewBag.location = 0;
			}
			// Tìm lecturetype dựa trên LectureTypeId của lectureDetail
			var lecturetype = _context.LectureTypes.FirstOrDefault(id => id.Id == lectureDetail.LectureTypeId);

			// Kiểm tra nếu lecturetype là null
			if (lecturetype == null)
			{
				return NotFound();
			}

			// Gán LectureType cho lectureDetail
			lectureDetail.LectureType = lecturetype;
			var existingProgress = _context.LearningProgresses
					.FirstOrDefault(lp => lp.UserId == 1002 &&
										  lp.LectureDetailId == lectureDetail.Id);

			if (existingProgress != null)
			{
				TempData["SuccessMessage"] = "Bài học đã được đánh dấu là đã hoàn thành";
			}
			// Trả về view "documentviewer" với lectureDetail
			return View("documentviewer", lectureDetail);
		}
        private int GetUserId()
        {
            var claimsIdentity = User.Identity as ClaimsIdentity;
            var nameClaim = claimsIdentity?.FindFirst(ClaimTypes.Name);
            if (nameClaim != null)
            {
                var user = _userService.FindUserByUserName(nameClaim.Value);
                if (user != null)
                {
                    return user.Id;
                }
            }

            throw new InvalidOperationException("User is not authenticated properly.");
        }
        // Action để đánh dấu bài học đã hoàn thành
        [HttpPost]
		public IActionResult MarkLessonComplete(int lectureDetailId)
		{
			try
			{
				// Lấy ID của người dùng hiện tại từ xác thực (ví dụ từ ClaimsIdentity hoặc cách khác mà bạn sử dụng)
				int userId = GetUserId();

				// Tìm lectureDetail dựa trên lectureDetailId
				var lectureDetail = _context.LectureDetails
					.Include(ld => ld.Lecture)
					.FirstOrDefault(ld => ld.Id == lectureDetailId);

				if (lectureDetail == null)
				{
					return NotFound(); // Nếu không tìm thấy lectureDetail
				}

				// Kiểm tra xem học viên đã hoàn thành bài học này chưa
				var existingProgress = _context.LearningProgresses
					.FirstOrDefault(lp => lp.UserId == userId &&
										  lp.LectureDetailId == lectureDetailId);

				if (existingProgress != null)
				{
					// Học viên đã hoàn thành bài học này
					TempData["SuccessMessage"] = "Bài học đã được đánh dấu là đã hoàn thành";
					return Json(new { success = true, message = "Lesson marked as completed." });

				}

                // Tạo đối tượng LearningProgress mới
                var learningProgress = _context.LearningProgresses
    .Where(lp => lp.UserId == userId &&
                 lp.CourseId == lectureDetail.Lecture.CourseId &&
                 lp.LectureId == lectureDetail.LectureId &&
                 lp.LectureDetailId == lectureDetailId)
    .FirstOrDefault();
				learningProgress.IsCompleted=true;
                // Thêm LearningProgress vào cơ sở dữ liệu
                _context.LearningProgresses.Update(learningProgress);
				_context.SaveChanges();



				// Chuyển hướng trở lại DocumentViewer
				return Json(new { success = true, message = "Lesson marked as completed." });

			}
			catch (Exception ex)
			{
				// Xử lý lỗi nếu có
				return StatusCode(500, $"Đã xảy ra lỗi: {ex.Message}");
			}
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
