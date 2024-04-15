using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChillToeic.Models.Entity;
using ChillToeic.Service;
using System.Security.Claims;


public class UserController : Controller
{
	private readonly UserService _userService;
	private readonly TestOfUserService _testofuserService;
	public UserController(TestOfUserService testofuserService, UserService userService)
	{
		_userService = userService;
		_testofuserService = testofuserService;
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

	public IActionResult ScoreHistory()
	{
		// Assuming you have a way to identify the current user
		int userId = GetUserId();
		IEnumerable<TestOfUser> tests =_testofuserService.GetTestOfUserByUserIdIncludeTest(userId);

		return View(tests);
	}

	// ... Additional methods for password updates or other functionalities
}
