using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ChillToeic.Models.Entity;
using ChillToeic.Repository;

namespace ChillToeic.Controllers
{
	public class ReviewsController : Controller
	{
		private readonly ApplicationDbContext _context;

		public ReviewsController(ApplicationDbContext context)
		{
			_context = context;
		}

		// GET: Reviews
		public async Task<IActionResult> Index()
		{
			var applicationDbContext = _context.Reviews.Include(r => r.Course).Include(r => r.User);
			return View(await applicationDbContext.ToListAsync());
		}

		// GET: Reviews/Details/5
		public async Task<IActionResult> Details(int? id)
		{
			if (id == null || _context.Reviews == null)
			{
				return NotFound();
			}

			var review = await _context.Reviews
				.Include(r => r.Course)
				.Include(r => r.User)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (review == null)
			{
				return NotFound();
			}

			return View(review);
		}

		// GET: Reviews/Create
		public IActionResult Create()
		{
			ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id");
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
			return View();
		}

		// POST: Reviews/Create
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create([Bind("Id,Description,Star,UserId,CourseId")] Review review)
		{
			if (ModelState.IsValid)
			{
				_context.Add(review);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", review.CourseId);
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", review.UserId);
			return View(review);
		}

		// GET: Reviews/Edit/5
		public async Task<IActionResult> Edit(int? id)
		{
			if (id == null || _context.Reviews == null)
			{
				return NotFound();
			}

			var review = await _context.Reviews.FindAsync(id);
			if (review == null)
			{
				return NotFound();
			}
			ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", review.CourseId);
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", review.UserId);
			return View(review);
		}

		// POST: Reviews/Edit/5
		// To protect from overposting attacks, enable the specific properties you want to bind to.
		// For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Edit(int id, [Bind("Id,Description,Star,UserId,CourseId")] Review review)
		{
			if (id != review.Id)
			{
				return NotFound();
			}

			if (ModelState.IsValid)
			{
				try
				{
					_context.Update(review);
					await _context.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!ReviewExists(review.Id))
					{
						return NotFound();
					}
					else
					{
						throw;
					}
				}
				return RedirectToAction(nameof(Index));
			}
			ViewData["CourseId"] = new SelectList(_context.Courses, "Id", "Id", review.CourseId);
			ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", review.UserId);
			return View(review);
		}

		// GET: Reviews/Delete/5
		public async Task<IActionResult> Delete(int? id)
		{
			if (id == null || _context.Reviews == null)
			{
				return NotFound();
			}

			var review = await _context.Reviews
				.Include(r => r.Course)
				.Include(r => r.User)
				.FirstOrDefaultAsync(m => m.Id == id);
			if (review == null)
			{
				return NotFound();
			}

			return View(review);
		}

		// POST: Reviews/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> DeleteConfirmed(int id)
		{
			if (_context.Reviews == null)
			{
				return Problem("Entity set 'ApplicationDbContext.Reviews'  is null.");
			}
			var review = await _context.Reviews.FindAsync(id);
			if (review != null)
			{
				_context.Reviews.Remove(review);
			}

			await _context.SaveChangesAsync();
			return RedirectToAction(nameof(Index));
		}

		private bool ReviewExists(int id)
		{
			return (_context.Reviews?.Any(e => e.Id == id)).GetValueOrDefault();
		}

		public ActionResult SaveRating(/*string tk,*/ int rating, string comment)
		{
			try
			{
				// Tạo một đối tượng Rating để lưu vào cơ sở dữ liệu
				var newReview = new Review
				{
					Description = comment,
					Star = rating,
					UserId = 10,
					CourseId = 4,
				};

				// Thêm đối tượng Rating vào DbSet của DbContext
				_context.Reviews.Add(newReview);

				// Lưu các thay đổi vào cơ sở dữ liệu
				_context.SaveChanges();

				return Json(new { success = true }); // Trả về kết quả thành công dưới dạng JSON
			}
			catch (Exception ex)
			{
				string errorMessage = ex.Message;
				string innerErrorMessage = ex.InnerException?.Message; // Truy xuất thông tin lỗi bên trong (nếu có)

				return Json(new { success = false, error = errorMessage, innerError = innerErrorMessage });
			}
		}
		public IActionResult Reviews()
		{
			return View();
		}
	}
}