using ChillToeic.Infrastructure.EmailSender;
using ChillToeic.Infrastructure.Jwt;
using ChillToeic.Jwt;
using ChillToeic.Models.DTO;
using ChillToeic.Models.Entity;
using ChillToeic.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;
using System.Web;
namespace ChillToeic.Controllers
{
    public class LoginController : Controller
    {
        // GET: LoginController
        private readonly UserService _userService;
        private readonly JwtService _jwtService;
        private readonly IEmailService _emailService;
        private readonly EmailOTPService _emailOTPService;
        private readonly EducationService _educationService;
        public LoginController(UserService userService, JwtService jwtService, IEmailService emailService, EmailOTPService emailOTPService, EducationService educationService)
        {
            _userService = userService;
            _jwtService = jwtService;
            _emailService = emailService;
            _emailOTPService = emailOTPService;
            _educationService = educationService;
        }

		public ActionResult Logout()
		{
			

			// Nếu chưa được xác thực, trả về trang index của login
			return View();
		}
		public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
				//var claimsIdentity = User.Identity as ClaimsIdentity;
				//var nameClaim = claimsIdentity?.FindFirst(ClaimTypes.Name);

				//// Kiểm tra xem claim "Name" có tồn tại không
				//if (nameClaim != null)
				//{
				//	string userName = nameClaim.Value;
				//	// Sử dụng userName ở đây để thực hiện các thao tác tiếp theo
                    
					
				//}
				//// Nếu đã được xác thực, chuyển hướng đến trang index của controller khác
				return RedirectToAction("Index", "Home");
            }

            // Nếu chưa được xác thực, trả về trang index của login
            return View();
        }
        [HttpPost]
        public ActionResult Index(string username, string password)
        {
            {
                User user = _userService.FindUserByUserNamewithRole(username);
                EducationCenter edu = _educationService.FindEducationCenterByUserNamewithRole(username);
                if (edu != null )
                {   if(edu.IsApprove == false) { ViewBag.approve = "Tài khoản chưa được cấp phép"; }
                    
                    if (edu.IsApprove == true)
                    {
                        if (edu.Password == password)
                        {
                            UserModel userModel = new UserModel { Username = edu.UserName, Role = edu.Role.Name };
                            string jwtToken = _jwtService.GenerateJSONWebToken(userModel);

                            Response.Cookies.Append("jwt", jwtToken);

                            return RedirectToAction("Index", "Home");
                        }
                    }
                }
                
            
                if (user != null )
                { if(user.Password == password )
                    { 
                        UserModel userModel = new UserModel { Username= user.UserName , Role = user.Role.Name };
                     string jwtToken=   _jwtService.GenerateJSONWebToken(userModel);
                      
                        Response.Cookies.Append("jwt", jwtToken);

                        return RedirectToAction("Index", "Home");
                    }
                    return View();
                }
                return View();
             }
        }
        
        public ActionResult UserRegister()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UserRegister(UserRegisterDTO userRegisterDTO)
        {
            if (ModelState.IsValid)
            {
                if (_userService.FindUserByUserName(userRegisterDTO.UserName) != null || _educationService.FindEducationCenterByUserName(userRegisterDTO.UserName) != null)
                {
                    if (_userService.FindUserByEmail(userRegisterDTO.Email) != null || _educationService.FindEducationCenterByEmail(userRegisterDTO.Email) != null)
                    {
                        if (_emailOTPService.GetOTPByEmail(userRegisterDTO.Email) != userRegisterDTO.VerifyOTP)
                        {
                            ViewBag.otp = "OTP not valid";
                            
                        }
                        ViewBag.email = "Email has been used";
                        
                    }
                    ViewBag.username = "Username has been used";
                    return View();
                }
                 
                 
                else
                {   
                    _emailOTPService.DeleteEmailOTP(userRegisterDTO.Email);
                    _userService.AddUser(new Models.Entity.User { FullName = userRegisterDTO.FullName, UserName = userRegisterDTO.UserName, Password = userRegisterDTO.Password, Email = userRegisterDTO.Email } );
                    return RedirectToAction("Index", "Login");
                }
            } else
            {
                return View();
            }
        }
        public ActionResult EducationRegister()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> EducationRegister(UserRegisterDTO userRegisterDTO,IFormFile filecertificate)
        {
            if (ModelState.IsValid)
            {
                if (_userService.FindUserByUserName(userRegisterDTO.UserName) != null || _educationService.FindEducationCenterByUserName(userRegisterDTO.UserName) != null)
                {
                    if (_userService.FindUserByEmail(userRegisterDTO.Email) != null || _educationService.FindEducationCenterByEmail(userRegisterDTO.Email) != null)
                    {
                        if (_emailOTPService.GetOTPByEmail(userRegisterDTO.Email) != userRegisterDTO.VerifyOTP)
                        {
                            ViewBag.otp = "OTP not valid";

                        }
                        ViewBag.email = "Email has been used";

                    }
                    ViewBag.username = "Username has been used";
                    return View();
                }
                else
                {

                    _emailOTPService.DeleteEmailOTP(userRegisterDTO.Email);
                    EducationCenter education = new EducationCenter { Name = userRegisterDTO.FullName, UserName = userRegisterDTO.UserName, Password = userRegisterDTO.Password, Email = userRegisterDTO.Email, Img = filecertificate.FileName, IsApprove=false};
                    _educationService.AddEducationCenter(education);
                    string folder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\img\\Education{education.Id}";
                    string duongdanmoi = Path.Combine(folder, filecertificate.FileName);
                    using (var stream = new FileStream(duongdanmoi, FileMode.Create))
                    {
                        await filecertificate.CopyToAsync(stream);
                    }
                    return RedirectToAction("Index", "Login");
                }
            }
            else
            {
                return View();
            }
        }
        [HttpPost]
        public async Task<ActionResult> SendOTP(string email)
        {   if(email!= null )
            {
                if(_userService.FindUserByEmail(email)==null && _educationService.FindEducationCenterByEmail(email) == null)
                {
                  await  _emailService.SendEmailAsync(email);
                    
                    return Content("OTP sent successfully"); 
                } else {

                    
                    return Content("Email has been created yet");
                }
            }
            return Content("Email has not been entered"); 
        }
        [HttpPost]
        public async Task<ActionResult> SendOTPResetPassword(string email)
        {
            if (email != null)
            {
                if (_userService.FindUserByEmail(email) != null || _educationService.FindEducationCenterByEmail(email) != null)
                {
                    await _emailService.SendEmailAsync(email);

                    return Content("OTP sent successfully");
                }
                else
                {
                    return Content("Email has not been created yet");
                }
            }
            return Content("Email has not been entered");
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(UserResetPasswordDTO userResetPasswordDTO)
        {
            if (ModelState.IsValid)
            {
                if (_userService.FindUserByEmail(userResetPasswordDTO.Email) != null || _educationService.FindEducationCenterByEmail(userResetPasswordDTO.Email) != null)
                {
                    if(_emailOTPService.GetOTPByEmail(userResetPasswordDTO.Email) != userResetPasswordDTO.VerifyOTP)
                    {
                        return Content("OTP not valid");
                    } else
                    {
                        _emailOTPService.DeleteEmailOTP(userResetPasswordDTO.Email);
                        return View(userResetPasswordDTO);
                    }
                }
                return Content("Email has not been created yet");
            }
            return View();
            
        }
        [HttpPost]
        public ActionResult ResetPassword2(UserResetPasswordDTO userResetPasswordDTO)
        {
            if(userResetPasswordDTO.Password == userResetPasswordDTO.ConfirmPassword)
            {
                User user = _userService.FindUserByEmail(userResetPasswordDTO.Email);
                if(user != null)
                {
                    user.Password = userResetPasswordDTO.Password;
                    _userService.UpdateUser(user);
                } else
                {
                    EducationCenter educationCenter = _educationService.FindEducationCenterByEmail(userResetPasswordDTO.Email);
                    educationCenter.Password = userResetPasswordDTO.Password;
                    _educationService.UpdateEducationCenter(educationCenter);
                }
               
                return RedirectToAction("Index","Login");
            }
            // Confirm Pass not valid
            return View("ResetPassword", userResetPasswordDTO);


        }
        public async Task LoginGmail()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme,
                new AuthenticationProperties
                {
                    RedirectUri = Url.Action("GoogleResponse")
                });
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var claims = result.Principal.Identities.FirstOrDefault().Claims.Select(claim => new
            {
                claim.Issuer,
                claim.OriginalIssuer,
                claim.Type,
                claim.Value

            });

            return Json(claims);

        }






        // GET: LoginController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        



        // GET: LoginController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: LoginController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: LoginController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: LoginController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: LoginController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
