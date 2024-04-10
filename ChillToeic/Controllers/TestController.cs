using ChillToeic.Infrastructure.EmailSender;
using ChillToeic.Jwt;
using ChillToeic.Models.Entity;
using ChillToeic.Models.DTO;
using ChillToeic.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Hosting.Server;
using ExcelDataReader;

using Microsoft.AspNetCore.Http;

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
		private readonly EducationService _educationService;
		public TestController(EducationService educationService, QuestionDetailService questionDetailService,AnswerService answerService, UserService userService, QuestionService questionService, TestOfUserService testofuserService, TestService testService, QuestionOfTestService questionOfTestService)
		{
			_userService = userService;
			_testofuserService = testofuserService;
			_testService = testService;
			_questionOfTestService = questionOfTestService;
			_questionService = questionService;
			_answerService = answerService;
			_questionDetailService = questionDetailService;
			_educationService = educationService;
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
			IEnumerable<Question> questions = _questionOfTestService.GetQuestionsByTestId(idTest).OrderBy(q => q.Part); 
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
			if (_userService.FindUserByUserName(userName) != null)
			{
				return _userService.FindUserByUserName(userName).Id;
			}
			return _educationService.FindEducationCenterByUserName(userName).Id;
		}
		public ActionResult UploadFile(int questionId)
		{
			return View();
		}
        // POST: Excel/Upload
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile excelFile, string TestName, string Description, string TestType)
        {
			//try
			//{

				System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

				// Upload File
				if (excelFile != null && excelFile.Length > 0)
				{
					var uploadDirectory = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads";

					if (!Directory.Exists(uploadDirectory))
					{
						Directory.CreateDirectory(uploadDirectory);
					}

					var filePath = Path.Combine(uploadDirectory, excelFile.FileName);

					using (var stream = new FileStream(filePath, FileMode.Create))
					{
						await excelFile.CopyToAsync(stream);
					}

					//Read File
					using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
					{
						var excelData = new List<QuestionDetailDTO>();
						using (var reader = ExcelReaderFactory.CreateReader(stream))
						{
							string filerecord = null;
							do
							{
								int i = 0;
								while (reader.Read())
								{
									if (i == 0)
									{
										filerecord = (string)reader.GetValue(1);
									}
									if (i > 1)
									{
										QuestionDetailDTO rowData = new QuestionDetailDTO();
										rowData.QuestionNumber = (int)(double)reader.GetValue(1);
										rowData.ImagePath = (string)reader.GetValue(2);
										rowData.Content = (string)reader.GetValue(3);
										rowData.OptionA = (string)reader.GetValue(4);
										rowData.OptionB = (string)reader.GetValue(5);
										rowData.OptionC = (string)reader.GetValue(6);
										rowData.OptionD = (string)reader.GetValue(7);
										rowData.CorrectAnswer = reader.GetValue(8).ToString();
										excelData.Add(rowData);
									}
									i++;
								}
							} while (reader.NextResult());
							Test test = new Test();
							if (TestType.ToLower().Trim() == "toeic")
							{
								test = new Test { Name = TestName, Description = Description, TestTypeId = 1, NumberOfQuestion = 200, NumberOfPart = 7, NumberOfUserTest = 0, Duration = 180, Score = 990, Status = false, EducationCenterId = GetUserId() };
								_testService.AddTest(test);
							}
							int testid = test.Id;
							ViewBag.ExcelData = excelData;
							Question question1 = new Question { Part = 1, Title = "Part 1", Detail = filerecord, QuestionTypeId = 1 };
							Question question2 = new Question { Part = 2, Title = "Part 2", QuestionTypeId = 1 };
							Question question3 = new Question { Part = 3, Title = "Part 3", QuestionTypeId = 1 };
							Question question4 = new Question { Part = 4, Title = "Part 4", QuestionTypeId = 2 };
							Question question5 = new Question { Part = 5, Title = "Part 5", QuestionTypeId = 2 };
							Question question6 = new Question { Part = 6, Title = "Part 6", QuestionTypeId = 2 };
							Question question7 = new Question { Part = 7, Title = "Part 7", QuestionTypeId = 2 };
						_questionService.AddQuestion(question1);
						_questionService.AddQuestion(question2);
						_questionService.AddQuestion(question3);
						_questionService.AddQuestion(question4);
						_questionService.AddQuestion(question5);
						_questionService.AddQuestion(question6);
						_questionService.AddQuestion(question7);
						//Part 1
						for (int i = 0; i < 6; i++)
							{
								string cr = excelData[i].CorrectAnswer;
								QuestionDetail questionDetail = new QuestionDetail
								{
									NumberQuestion = (int)excelData[i].QuestionNumber,
									Img = excelData[i].ImagePath,
									AnswerA = excelData[i].OptionA,
									AnswerB = excelData[i].OptionB,
									AnswerC = excelData[i].OptionC,
									AnswerD = excelData[i].OptionD,
									CorrectAnswer = int.Parse(cr),
									QuestionId = question1.Id
								};
								_questionDetailService.AddQuestionDetail(questionDetail);
							}
							//Part 2
							for (int i = 6; i < 32; i++)
							{
								string cr = excelData[i].CorrectAnswer;
								QuestionDetail questionDetail = new QuestionDetail
								{
									NumberQuestion = (int)excelData[i].QuestionNumber,
									AnswerA = excelData[i].OptionA,
									AnswerB = excelData[i].OptionB,
									AnswerC = excelData[i].OptionC,
									AnswerD = excelData[i].OptionD,
									CorrectAnswer = int.Parse(cr),
									QuestionId = question2.Id
								};
								_questionDetailService.AddQuestionDetail(questionDetail);
							}
							//Part 3
							for (int i = 32; i < 71; i++)
							{
								string cr = excelData[i].CorrectAnswer;
								QuestionDetail questionDetail = new QuestionDetail
								{
									NumberQuestion = (int)excelData[i].QuestionNumber,
									Description = excelData[i].Content,
									AnswerA = excelData[i].OptionA,
									AnswerB = excelData[i].OptionB,
									AnswerC = excelData[i].OptionC,
									AnswerD = excelData[i].OptionD,
									CorrectAnswer = int.Parse(cr),
									QuestionId = question3.Id
								};
								_questionDetailService.AddQuestionDetail(questionDetail);
							}
							//Part 4
							for (int i = 71; i < 101; i++)
							{
								string cr = excelData[i].CorrectAnswer;
								QuestionDetail questionDetail = new QuestionDetail
								{
									NumberQuestion = (int)excelData[i].QuestionNumber,
									Img = excelData[i].ImagePath,
									Description = excelData[i].Content,
									AnswerA = excelData[i].OptionA,
									AnswerB = excelData[i].OptionB,
									AnswerC = excelData[i].OptionC,
									AnswerD = excelData[i].OptionD,
									CorrectAnswer = int.Parse(cr),
									QuestionId = question4.Id
								};
								_questionDetailService.AddQuestionDetail(questionDetail);
							}
							//Part 5
							for (int i = 101; i < 131; i++)
							{
								string cr = excelData[i].CorrectAnswer;
								QuestionDetail questionDetail = new QuestionDetail
								{
									NumberQuestion = (int)excelData[i].QuestionNumber,
									Img = excelData[i].ImagePath,
									Description = excelData[i].Content,
									AnswerA = excelData[i].OptionA,
									AnswerB = excelData[i].OptionB,
									AnswerC = excelData[i].OptionC,
									AnswerD = excelData[i].OptionD,
									CorrectAnswer = int.Parse(cr),
									QuestionId = question5.Id
								};
								_questionDetailService.AddQuestionDetail(questionDetail);
							}
							//Part 6
							for (int i = 131; i < 147; i++)
							{
								string cr = excelData[i].CorrectAnswer;
								QuestionDetail questionDetail = new QuestionDetail
								{
									NumberQuestion = (int)excelData[i].QuestionNumber,
									Img = excelData[i].ImagePath,
									Description = excelData[i].Content,
									AnswerA = excelData[i].OptionA,
									AnswerB = excelData[i].OptionB,
									AnswerC = excelData[i].OptionC,
									AnswerD = excelData[i].OptionD,
									CorrectAnswer = int.Parse(cr),
									QuestionId = question6.Id
								};
								_questionDetailService.AddQuestionDetail(questionDetail);
							}
							//Part 7
							for (int i = 147; i < 200; i++)
							{
								string cr = excelData[i].CorrectAnswer;
								QuestionDetail questionDetail = new QuestionDetail
								{
									NumberQuestion = (int)excelData[i].QuestionNumber,
									Img = excelData[i].ImagePath,
									Description = excelData[i].Content,
									AnswerA = excelData[i].OptionA,
									AnswerB = excelData[i].OptionB,
									AnswerC = excelData[i].OptionC,
									AnswerD = excelData[i].OptionD,
									CorrectAnswer = int.Parse(cr),
									QuestionId = question7.Id
								};
								_questionDetailService.AddQuestionDetail(questionDetail);
							}
						}
					}
				}

				return View();
			//} catch(Exception ex)
			//{
			//	return View(ex);
			//}
		}

		
	}
}