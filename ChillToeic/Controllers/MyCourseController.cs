using ChillToeic.Models;
using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using ChillToeic.Service;
using Microsoft.AspNetCore.Mvc;

namespace ChillToeic.Controllers
{
    public class MyCourseController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly CourseService _dbCourse;
        private readonly LearningProgressService _dbLearningProgress;
        private readonly LectureDetailService _dbLectureDetail;
        private readonly LectureService _dbLecture;

        public MyCourseController(CourseService dbCourse, LearningProgressService dbLearningProgress, LectureDetailService dbLectureDetail, LectureService dbLecture, ApplicationDbContext dbContext)
        {
            _context = dbContext;
            _dbCourse = dbCourse;
            _dbLearningProgress = dbLearningProgress;
            _dbLectureDetail = dbLectureDetail;
            _dbLecture = dbLecture;
        }

        public IActionResult MyCourse([FromRoute]int id)
        {
            var courseCompletionInfos = GetCourseCompletionInfoByUserId(id);
            return View(courseCompletionInfos);
        }

        private List<LearningProgress> GetListLearningProgressByUserId(int userID)
        {
            var learningProgresss = _dbLearningProgress.GetLearningProgressByUserId(userID);
            return learningProgresss.ToList();
        }

        private List<Lecture> GetListLectureByCourseId(int courseID)
        {
            var lecture = _dbLecture.GetLecturesByCourseId(courseID);
            return lecture.ToList();
        }

        private List<LectureDetail> GetListLectureDetailByLectureId(int userID)
        {
            var lectureDetail = _dbLectureDetail.GetLectureDetailsByLectureId(userID);
            return lectureDetail.ToList();
        }

        public List<LectureDetail> GetCompletedLectureDetails(int userId, int courseId, int lectureId)
        {
            var completedLectureDetails = _context.LearningProgresses
                .Where(lp => lp.UserId == userId && lp.CourseId == courseId && lp.LectureId == lectureId && lp.IsCompleted)
                .Select(lp => lp.LectureDetail)
                .ToList();

            return completedLectureDetails;
        }

        //--------------------------------------------------------------- I LOVE CHATGPT ----------------------------------------------------------------------------------------------


        public List<CourseCompletionInfo> GetCourseCompletionInfoByUserId(int userId)
        {
            var learningProgresses = GetListLearningProgressByUserId(userId);
            var courseCompletionInfos = new List<CourseCompletionInfo>();

            // Dictionary để lưu trữ thông tin về số lượng LectureDetail và số lượng LectureDetail đã hoàn thành cho từng CourseId
            var courseData = new Dictionary<int, (int totalLectureDetailCount, int completedLectureDetailCount)>();

            // Lặp qua từng LearningProgress để tính toán số liệu
            foreach (var learningProgress in learningProgresses)
            {
                var lectureDetails = GetListLectureDetailByLectureId(learningProgress.LectureId);

                // Nếu CourseId chưa có trong dictionary, thêm vào với giá trị ban đầu
                if (!courseData.ContainsKey(learningProgress.CourseId))
                {
                    courseData[learningProgress.CourseId] = (0, 0);
                }

                // Tăng số lượng LectureDetail và số lượng LectureDetail đã hoàn thành nếu IsCompleted là true
                courseData[learningProgress.CourseId] = (courseData[learningProgress.CourseId].totalLectureDetailCount + 1,
                                                         courseData[learningProgress.CourseId].completedLectureDetailCount + (learningProgress.IsCompleted ? 1 : 0));
            }

            // Tính CompletionPercentage cho từng CourseId và thêm vào danh sách kết quả
            foreach (var courseId in courseData.Keys)
            {
                var (totalLectureDetailCount, completedLectureDetailCount) = courseData[courseId];
                var completionPercentage = totalLectureDetailCount == 0 ? 0 : (double)completedLectureDetailCount / totalLectureDetailCount * 100;

                var course = _dbCourse.GetCourseById(courseId); // Lấy thông tin về Course từ Database

                courseCompletionInfos.Add(new CourseCompletionInfo
                {
                    CourseId = courseId,
                    CourseName = course.Name,
                    CompletionPercentage = completionPercentage
                });
            }

            return courseCompletionInfos;
        }




        //-------------------------------------------------------------------------------------------------------------------------------------------------------------


        public IActionResult CourseCompletionInfo(int userId)
        {
            var courseCompletionInfos = GetCourseCompletionInfoByUserId(userId);
            return View(courseCompletionInfos);
        }

    }
}
