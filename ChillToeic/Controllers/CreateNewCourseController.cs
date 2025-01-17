﻿using ChillToeic.Models;
using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using ChillToeic.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ChillToeic.Controllers
{
    public class CreateNewCourseController : Controller
    {
        private readonly UserService _userService;
        private readonly EducationService _educationService;

        public CreateNewCourseController(UserService userService, EducationService educationService)
        {
            _userService = userService;
            _educationService = educationService;
        }
        public IActionResult CreateNewCourse()
        {
            return View();
        }
        //private int GetUserId()
        //{
        //    var claimsIdentity = User.Identity as ClaimsIdentity;
        //    var nameClaim = claimsIdentity?.FindFirst(ClaimTypes.Name);
        //    if (nameClaim != null)
        //    {
        //        var user = _userService.FindUserByUserName(nameClaim.Value);
        //        if (user != null)
        //        {
        //            return user.Id;
        //        }
        //    }

        //    throw new InvalidOperationException("User is not authenticated properly.");
        //}
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
        [HttpPost]
        public IActionResult CreateNewCourse(CreateNewCourseInfo newCourseInfo, IEnumerable<LectureInfo> LectureDetailInfos)
        {
            try
            {
                ApplicationDbContext dbContext = new ApplicationDbContext();
                Course course = new Course
                {
                    Name = newCourseInfo.Name,
                    Description = newCourseInfo.Description,
                    Price = newCourseInfo.Price,
                    level = newCourseInfo.level,
                    QuantityOfLecture = newCourseInfo.QuantityOfLecture,
                    CreatedAt = newCourseInfo.CreatedAt,
                    EducationCenterId = GetUserId(),
                };

                dbContext.Add(course);
                dbContext.SaveChanges();

                foreach (var lectureInfo in LectureDetailInfos)
                {
                    Lecture lecture = new Lecture
                    {
                        Time = lectureInfo.Time,
                        CourseId = course.Id,
                    };

                    dbContext.Add(lecture);
                    dbContext.SaveChanges();

                    lecture.LectureDetails = new List<LectureDetail>()
                    {
                        new LectureDetail
                        {
                            Content = lectureInfo.Content.FileName,
                            LectureId = lecture.Id,
                            Title = lectureInfo.Title,
                        }
                    };
                }

                dbContext.SaveChanges();
                return View();
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                string innerErrorMessage = ex.InnerException?.Message; // Truy xuất thông tin lỗi bên trong (nếu có)

                return Json(new { success = false, error = errorMessage, innerError = innerErrorMessage });
            }
        }
    }
}
