using Microsoft.AspNetCore.Mvc;
using ChillToeic.Models.Entity;
using System.Linq;
using ChillToeic.Repository;
using ChillToeic.Service;
using System.Security.Claims;

namespace ChillToeic.Controllers
{
    public class PaymentHistoryController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserService _userService;
        public PaymentHistoryController(ApplicationDbContext context,UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public IActionResult PaymentHistory()
        {
            // Assuming you have a way to identify the current user
            int userId = GetUserId();
            var orders = _context.Orders
                                 .Where(o => o.UserId == userId)
                                 .ToList();

            return View(orders);
        }

        // Method to get current user's ID - placeholder
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
    }
}
