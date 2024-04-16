using Microsoft.AspNetCore.Mvc;
using ChillToeic.Models.Entity;
using System;
using ChillToeic.Repository;
using ChillToeic.Service;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using ChillToeic.Models;
using Microsoft.AspNetCore.Cors.Infrastructure;
using DocumentFormat.OpenXml.Drawing.Charts;
using ChillToeic.Models;
using Order = ChillToeic.Models.Entity.Order;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using DocumentFormat.OpenXml.Presentation;



namespace ChillToeic
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IVnPayService _vnPayService;
        private readonly ApplicationDbContext _context;
        private readonly CourseService _courseService;
        private readonly UserService _userService;
		private readonly LectureService _lectureService;
        private readonly LearningProgressService _learningProgressService;
		private readonly EducationService _educationService;

		public OrderController(EducationService educationService,LearningProgressService learningProgressService,LectureService lectureService,ApplicationDbContext context, IVnPayService vnPayService, CourseService courseService, UserService userService)
        {
			_learningProgressService = learningProgressService;
			_lectureService = lectureService;
			_context = context;
            _vnPayService = vnPayService;
            _courseService = courseService;
            _userService = userService;
            _educationService = educationService;

		}






        public async Task<IActionResult> Checkout(int courseId)
        {
            var username = User.FindFirst(ClaimTypes.Name).Value;
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
            if (user == null)
            {
                var errorViewModel = new ErrorViewModel { Message = "Invalid user ID." };
                return View("Error", errorViewModel);
            }

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                var errorViewModel = new ErrorViewModel { Message = "Course not found." };
                return View("Error", errorViewModel);
            }

            // Create a new order object
            var order = new Order
            {
                ProductName = course.Name,
                UserId = user.Id,
                CourseId = courseId,
                Status = false,
                OrderDate = DateTime.UtcNow,
                TotalAmount = course.Price // Replace 100.0 with the appropriate value representing the total amount
            }; // Add a semicolon here to end the Order object initialization block

            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error when saving order: {ex.Message}");
                var errorViewModel = new ErrorViewModel { Message = "Failed to create order." };
                return View("Error", errorViewModel);
            }

            // Find the newly created order or handle cases where it might not be found
            var savedOrder = await _context.Orders.FindAsync(order.Id);
            if (savedOrder == null || savedOrder.Status)
            {
                return NotFound($"Order with ID {savedOrder.Id} not found or already paid.");
            }

            // Prepare the payment model
            var vnPayModel = new VnPaymentRequestModel
            {
                OrderId = savedOrder.Id,
                Amount = (double)savedOrder.TotalAmount,
                OrderInfo = $"Payment for order {savedOrder.Id}",
                OrderDate = savedOrder.OrderDate,
                UserId = 1,
                CourseId = 3
            };

            // Create the payment URL
            string paymentUrl = _vnPayService.CreatePaymentUrl(HttpContext, vnPayModel);
            return Redirect(paymentUrl);
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

		public async Task<IActionResult> PaymentCallBack()
        {
            int userid = GetUserId();
            var response = _vnPayService.PaymentExecute(Request.Query);
            string input = response.OrderDescription;
            string[] parts = input.Split(':');
            string orderId = parts[1];
            int orderIdcuoicung = int.Parse(orderId);
            
            if (response.Success )
            {
                var order = await _context.Orders.FindAsync(orderIdcuoicung);

                if (order != null && !order.Status) // Check if the order is not already paid
                {
                    order.Status = true; // Mark the order as paid
                    order.OrderDate = DateTime.UtcNow; // Set the payment date, if needed
                                                       // Update other fields if necessary, e.g., Discount, TotalAmount, etc.
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Payment successful.";
                    int courseid = order.CourseId;
					
                   
                    return RedirectToAction("PaymentSuccess", new { courseid = courseid, userid = userid });
                }
                else
                {   
                    TempData["Message"] = "Order has already been paid or not found.";
                    return RedirectToAction("PaymentFail");
                }
            }

            TempData["Message"] = "Payment failed or canceled.";
            return RedirectToAction("PaymentFail");
        }
        public IActionResult Order(int CourseId)
        {
            var course = _courseService.GetCourseById(CourseId);
            return View(course);
        }

        public IActionResult PaymentSuccess(int courseid, int userid )
        {
			IList<Lecture> lectures = _context.Lectures.Where(l => l.CourseId == courseid).Include(m=>m.LectureDetails).ToList();
			foreach (var lecture in lectures)
			{

				_learningProgressService.AddLearningProgress(new LearningProgress { IsCompleted = false, UserId = userid, CourseId = courseid, LectureId = lecture.Id, LectureDetailId = lecture.LectureDetails.FirstOrDefault().Id });

			}
			return View("Success", TempData["Message"]);
        }

        public IActionResult PaymentFail()
        {
            return View("PaymentFail", TempData["Message"]);
        }
      
        [HttpPost]
        public async Task<IActionResult> ApplyDiscount(int courseId, decimal UnitPrice, string discountCode)
        {
            var discount = await _context.Discount
                .Where(d => d.Name == discountCode && d.CourseId == courseId)
                .Select(d => d.DiscountValue)
                .FirstOrDefaultAsync();

            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound("Course does not exist.");
            }

            if (discount > 0)
            {
                var discountedPrice = UnitPrice - (UnitPrice * discount / 100m);
                ViewBag.DiscountedPrice = discountedPrice;
                ViewBag.DiscountPercentage = discount;
            }
            else
            {
                ViewBag.DiscountErrorMessage = "Invalid discount code.";
                ViewBag.DiscountedPrice = UnitPrice;
                ViewBag.DiscountPercentage = 0;
            }

            // The ViewBag will have either the discounted price or the original unit price
            return View("Order", course);
        }
    }
    // ... Other actions and methods ...
}

