using ChillToeic.Models;
using ChillToeic.Models.Entity;
using ChillToeic.Service;
using ChillToeic.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ChillToeic.Controllers
{
    public class AdminController : Controller
    {
        private readonly CenterService _dbCenter;
        private readonly CourseService _dbCourse;
        private readonly TestService _dbTest;
        private readonly UserService _dbUser;
        private readonly TestOfUserService _dbTestOfUser;
        private readonly LearningProgressService _dbLearningProgress;
        private readonly OrdersService _dbOrders;
        private readonly CertificateService _dbCertificate;

        public AdminController(CenterService centerService, CourseService courseService,
                                TestService testService, UserService userService,
                                TestOfUserService testOfUserService, LearningProgressService learningProgressService,
                                OrdersService ordersService, CertificateService certificateService)
        {
            _dbCenter = centerService;
            _dbCourse = courseService;
            _dbTest = testService;
            _dbUser = userService;
            _dbTestOfUser = testOfUserService;
            _dbLearningProgress = learningProgressService;
            _dbOrders = ordersService;
            _dbCertificate = certificateService;

        }
        public IActionResult Dashboard()
        {
            return View();
        }

        public IActionResult UserManagement()
        {
            var user = _dbUser.GetAllUsers();
            return View(user);
        }

        public IActionResult CenterManagement()
        {
            var center = _dbCenter.GetAllCenter();
            return View(center);
        }

        public IActionResult CourseManagement()
        {
            var course = _dbCourse.GetAllCourses();
            return View(course);
        }

        public IActionResult TestManagement()
        {
            var test = _dbTest.GetAllTests();
            return View(test);
        }

        public IActionResult UserDetailManagement(int id)
        {
            var user = _dbUser.GetUserByUserId(id);
            var course = _dbOrders.GetOrderByUserId(id);
            var certificate = _dbCertificate.GetCertificateByUserId(id);

            var model = new UserDetailViewModel
            {
                Users = user.ToList(),
                Orders = course.ToList(),
                Certificates = certificate.ToList()
            };
            return View(model);
        }

        public IActionResult CenterDetailManagement(int id)
        {
            var courses = _dbCourse.GetCoursesByCenterId(id);
            var centers = _dbCenter.GetCentersByCenterId(id);
            var tests = _dbTest.GetTestByCenterId(id);

            var model = new CenterDetailViewModel
            {
                Centers = centers.ToList(),
                Courses = courses.ToList(),
                Tests = tests.ToList()
            };

            return View(model);
        }

        public IActionResult CourseDetailManagement(int id)
        {
            var course = _dbCourse.GetCoursesByCourseId(id).FirstOrDefault();
            return View(course);
        }

        public IActionResult TestDetailManagement(int id)
        {
            var testDetail = _dbTest.GetTestById(id);

            //if (testDetail == null)
            //{
            //    return NotFound();
            //}

            return View(testDetail);
        }
    }
}
