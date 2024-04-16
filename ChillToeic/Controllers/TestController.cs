using ChillToeic.Models.Entity;
using ChillToeic.Models.DTO;
using ChillToeic.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ExcelDataReader;

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
        public TestController(EducationService educationService, QuestionDetailService questionDetailService, AnswerService answerService, UserService userService, QuestionService questionService, TestOfUserService testofuserService, TestService testService, QuestionOfTestService questionOfTestService)
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
            }
            else
            { 
                _testofuserService.AddTestOfUser(new TestOfUser { UserId = _userService.FindUserByUserName(userName).Id, TestId = idTest });

            }
            Test test = _testService.GetTestById(idTest);
            ViewBag.Duration = test.Duration;
            //Lấy danh sách câu hỏi
            IEnumerable<Question> questions = _questionOfTestService.GetQuestionsByTestId(idTest).OrderBy(q => q.Part);
            ViewBag.idTest = idTest;
            return View(questions);
        }

        public IActionResult SubmitTest(int testId, IEnumerable<ChillToeic.Models.DTO.AnswerDTO> answers)
        {
            var testofuser = _testofuserService.GetTestOfUserByUserIdAndTestId(GetUserId(), testId);
            int testofuserid = testofuser.Id;
            var testss = _testService.GetTestById(testId);
            int i = 0;
            foreach (var answer in answers)
            {
                int answervalue = 0;
                if (answer.AnswerValue != null)
                {
                    answervalue = int.Parse(answer.AnswerValue);
                }

                _answerService.AddAnswer(new Answer { AnswerQuestion = answervalue, QuestionNumber = answer.QuestionDetailId, TestOfUserId = testofuserid });
                if (answervalue == _questionDetailService.GetQuestionDetailById(answer.QuestionDetailId).CorrectAnswer)
                {
                    i = i + 10;
                }
            }
            testofuser.Score = i;
            _testofuserService.UpdateTestOfUser(testofuser);
            testss.NumberOfUserTest = testss.NumberOfUserTest + 1;
            ViewBag.score = i;
            ViewBag.testname = _testService.GetTestById(testId).Name;
            return View();
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
        public async Task<IActionResult> UploadFile(IFormFile excelFile, string TestName, string Description, int TestType, IEnumerable<IFormFile> fileInput)
        {
            try
            {
                List<string> filename = new List<string>();
                List<string> filename2 = new List<string>();
                int testid = new int();
                foreach (IFormFile file in fileInput)
                {
                    filename.Add(file.FileName);

                }

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
                            string record = null;
                            do
                            {
                                int i = 0;
                                while (reader.Read())
                                {
                                    if (i == 0)
                                    {
                                        record = (string)reader.GetValue(1);
                                        filename2.Add(record);

                                    }
                                    if (i > 1)
                                    {
                                        QuestionDetailDTO rowData = new QuestionDetailDTO();
                                        rowData.QuestionNumber = (int)(double)reader.GetValue(1);
                                        rowData.ImagePath = (string)reader.GetValue(2);
                                        if (rowData.ImagePath != null)
                                        {
                                            filename2.Add(rowData.ImagePath);

                                        }
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
                            bool sosanh = filename.All(element => filename2.Contains(element));
                            bool sosanh2 = filename2.All(element => filename.Contains(element));
                            if (!sosanh && !sosanh2) throw new Exception();

                            Test test = new Test();
                          
                                test = new Test { Name = TestName, Description = Description, TestTypeId = 1, NumberOfQuestion = 200, NumberOfPart = 7, NumberOfUserTest = 0, Duration = 180, Score = 990, Status = true, EducationCenterId = GetUserId() };
                                _testService.AddTest(test);
                            
                            int tested = test.Id;
                            testid = tested;

                            foreach (var file in fileInput)
                            {
                                var uploadDirectory2 = $"{Directory.GetCurrentDirectory()}\\wwwroot\\img\\Test{test.Id}";

                                if (!Directory.Exists(uploadDirectory2))
                                {
                                    Directory.CreateDirectory(uploadDirectory2);
                                }
                                var filePath2 = Path.Combine(uploadDirectory2, file.FileName);

                                using (var stream2 = new FileStream(filePath2, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream2);
                                }
                            }


                            ViewBag.ExcelData = excelData;
                            Question question1 = new Question { Part = 1, Title = "Part 1", Detail = record, QuestionTypeId = 1 };
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
                            _questionOfTestService.Add(new QuestionOfTest { QuestionId = question1.Id, TestId = tested });
                            _questionOfTestService.Add(new QuestionOfTest { QuestionId = question2.Id, TestId = tested });
                            _questionOfTestService.Add(new QuestionOfTest { QuestionId = question3.Id, TestId = tested });
                            _questionOfTestService.Add(new QuestionOfTest { QuestionId = question4.Id, TestId = tested });
                            _questionOfTestService.Add(new QuestionOfTest { QuestionId = question5.Id, TestId = tested });
                            _questionOfTestService.Add(new QuestionOfTest { QuestionId = question6.Id, TestId = tested });
                            _questionOfTestService.Add(new QuestionOfTest { QuestionId = question7.Id, TestId = tested });
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

                return RedirectToAction("UpdateTest", new {testid=testid});
            }
            catch (Exception ex)
            {
                ViewBag.error = "error when importing file";
    
                return View();
            }
        }

        
        public ActionResult UpdateTest(int testid)
        {
            IEnumerable<Question> questions = _questionOfTestService.GetQuestionsByTestId(testid).OrderBy(q => q.Part);
            ViewBag.idTest = testid;
           
            return View(questions);
            
        }
		public async Task<ActionResult> SubmitUpdate(int testId,IEnumerable<AnswerUpdateDTO2> answers)
		{
            foreach(AnswerUpdateDTO2 answer in answers)
            {
				int questionId = new int();
				int questionDetailId = new int();
                int answervalue = new int();
				string newimg = null;
				string title = null;
				string answerA = null;
				string answerB = null;
				string answerC = null;
				string answerD = null;
				string[] listid = answer.QuestionIdAndDetailId.Split(' ');
				questionId = int.Parse(listid[0]);
				questionDetailId = int.Parse(listid[1]);
                answervalue = answer.AnswerValue;

                if (answer.ImgForm != null)
                {
                    var questiondetail = _questionDetailService.GetQuestionDetailById(questionDetailId);
                    string? previousimage = questiondetail.Img;


                    string folder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\img\\Test{testId}";
                    if (previousimage != null)
                    {
                        string tenTep = previousimage;
                        string duongDanDayDu = Path.Combine(folder, tenTep);

                        if (System.IO.File.Exists(duongDanDayDu))
                        {
							System.IO.File.Delete(duongDanDayDu);
                            Console.WriteLine("Đã xóa tệp " + tenTep);
                        }
                        else
                        {
                            Console.WriteLine("Không tìm thấy tệp " + tenTep);
                        }
                        
                        
                    }
					string duongdanmoi = Path.Combine(folder, answer.ImgForm.FileName);
					using (var stream = new FileStream(duongdanmoi, FileMode.Create))
					{
						await answer.ImgForm.CopyToAsync(stream);
					}
					newimg = answer.ImgForm.FileName;
				}
					string[] listnewanser = answer.Answer.Split('/');
                    if (listnewanser.Count() == 5)
                    {
                        title = listnewanser[0];
						answerA = listnewanser[1];
						answerB = listnewanser[2];
						answerC = listnewanser[3];
						answerD = listnewanser[4];
					}
                    else{
						answerA = listnewanser[0];
						answerB = listnewanser[1];
						answerC = listnewanser[2];
						answerD = listnewanser[3];
                    }


				var questiondetail2 = _questionDetailService.GetQuestionDetailById(questionDetailId);
				questiondetail2.CorrectAnswer = answervalue;
				questiondetail2.AnswerA = answerA;
				questiondetail2.AnswerB = answerB;
				questiondetail2.AnswerC = answerC;
				questiondetail2.AnswerD = answerD;
                if(title != null)
                {
					questiondetail2.Description = title;
				}
                if (newimg !=null )
                {
                    questiondetail2.Img = newimg;
                }
                _questionDetailService.UpdateQuestionDetail(questiondetail2);	

			}

			return RedirectToAction("Index", "Home");


		}
       
    }
}