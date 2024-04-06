using ChillToeic.Infrastructure.EmailSender;
using ChillToeic.Jwt;
using ChillToeic.Models.Entity;
using ChillToeic.Models.DTO;
using ChillToeic.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChillToeic.Controllers
{
	public class TestController : Controller
	{
		private readonly TestOfUserService _testofuserService;
		private readonly UserService _userService;
		private readonly TestService _testService;
		private readonly QuestionOfTestService _questionOfTestService;
		private readonly QuestionService _questionService;
		private readonly AnswerService _answerService;
		private readonly QuestionDetailService _questionDetailService;
		public TestController(QuestionDetailService questionDetailService,AnswerService answerService, UserService userService, QuestionService questionService, TestOfUserService testofuserService, TestService testService, QuestionOfTestService questionOfTestService)
		{
			_userService = userService;
			_testofuserService = testofuserService;
			_testService = testService;
			_questionOfTestService = questionOfTestService;
			_questionService = questionService;
			_answerService = answerService;
			_questionDetailService = questionDetailService;
		}

		
		public IActionResult Test(int idTest, int? idCourse)
		{    
			var claimsIdentity = User.Identity as ClaimsIdentity;
				var nameClaim = claimsIdentity?.FindFirst(ClaimTypes.Name);
			   string userName = nameClaim.Value;
			//Tạo 1 bài test

			var testofuser = _testofuserService.GetTestOfUserByUserIdAndTestId(GetUserId(), idTest);
			if (testofuser != null)
			{
				_testofuserService.DeleteTestOfUser(testofuser.Id);
			}
			if (idCourse != null)
			{
                _testofuserService.AddTestOfUser(new TestOfUser { UserId = _userService.FindUserByUserName(userName).Id, TestId = idTest, CourseId = idCourse });
			} else
			{
				_testofuserService.AddTestOfUser(new TestOfUser { UserId = _userService.FindUserByUserName(userName).Id, TestId = idTest });

			}
			Test test = _testService.GetTestById(idTest);
			ViewBag.Duration = test.Duration;
			//Lấy danh sách câu hỏi
			IEnumerable<Question> questions = _questionOfTestService.GetQuestionsByTestId(idTest);
			ViewBag.idTest= idTest;
            return View(questions);
		}
		public ActionResult QuestionDetails(int questionId)
		{
			// Lấy danh sách chi tiết câu hỏi từ questionId
			var questionDetails = _questionService.GetQuestionDetailsByQuestionId(questionId); // Lấy danh sách chi tiết câu hỏi từ questionId, có thể từ cơ sở dữ liệu hoặc bất kỳ nguồn dữ liệu nào khác

	// Trả về một phần tử giao diện dạng PartialView hiển thị danh sách chi tiết câu hỏi
	return PartialView("_QuestionDetails", questionDetails);
		}
		public async Task<IActionResult> TestSubmit( int testId, IEnumerable<ChillToeic.Models.DTO.AnswerDTO> answers)
		{
			var testofuser = _testofuserService.GetTestOfUserByUserIdAndTestId(GetUserId(), testId);
			int testofuserid = testofuser.Id;
			int i = 0;
			foreach (var answer in answers)
			{
				int answervalue = 0;
				if (answer.AnswerValue != null)
				{
					answervalue = int.Parse(answer.AnswerValue);
				}
				
				_answerService.AddAnswer(new Answer { AnswerQuestion = answervalue,QuestionNumber = answer.QuestionDetailId, TestOfUserId = testofuserid });
				if(answervalue == _questionDetailService.GetQuestionDetailById(answer.QuestionDetailId).CorrectAnswer)
				{
					i=i+10;
				}
			}
			testofuser.Score = i;
			_testofuserService.UpdateTestOfUser(testofuser);
			return await Task.FromResult(Content(""+i));
		}
		private int GetUserId()
		{
			var claimsIdentity = User.Identity as ClaimsIdentity;
			var nameClaim = claimsIdentity?.FindFirst(ClaimTypes.Name);
			string userName = nameClaim.Value;
			return _userService.FindUserByUserName(userName).Id;
		}
	}
}