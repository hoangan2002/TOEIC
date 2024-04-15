using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChillToeic.Models.Entity;
using ChillToeic.Service;
using System.Security.Claims;

[Authorize]
public class UserProfileController : Controller
{
	private readonly UserService _userService;

	public UserProfileController(UserService userService)
	{
		_userService = userService;
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

	[HttpGet]
	public IActionResult UserProfile()
	{
		var userId = GetUserId();
		var user = _userService.GetUserById(userId);
		if (user != null)
		{
			return View(user);
		}
		else
		{
			// If the user was not found, redirect to the login page or show an error
			return RedirectToAction("Index", "Login");
		}
	}

	[HttpPost]
	[ValidateAntiForgeryToken]
	public IActionResult UserProfile(User updatedUser, string Fullname, string Email, DateTime? DateOfBirth)
	{
		
		 var userId = GetUserId();
		var userInDb = _userService.GetUserById(userId);

		if (userInDb != null)
		{
			userInDb.FullName = Fullname;
			userInDb.Email = Email;
			userInDb.DateOfBirth = DateOfBirth;
			

			_userService.UpdateUser(userInDb);

			return RedirectToAction(nameof(UserProfile)); 
		}
		else
		{
			
			return NotFound();
		}
	}

	// ... Additional methods for password updates or other functionalities
}
