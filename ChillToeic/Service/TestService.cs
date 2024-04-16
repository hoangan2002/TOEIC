using ChillToeic.Models;
using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using System.Linq.Expressions;

namespace ChillToeic.Service
{
    public class TestService
    {
        private readonly IRepository<Test> _testRepository;
        private readonly IRepository<EducationCenter> _educationCenterRepository;
        private readonly IRepository<Course> _courseRepository;


        public TestService(IRepository<Test> testRepository, IRepository<EducationCenter> educationCenterRepository, IRepository<Course> courseRepository)
        {
            _testRepository = testRepository;
            _educationCenterRepository = educationCenterRepository;
            _courseRepository = courseRepository;
        }

        public IEnumerable<Test> GetAllTests()
        {
            return  _testRepository.GetAll();
        }

        public Test GetTestById(int id)
        {
            return _testRepository.Find(m => m.Id == id).FirstOrDefault();
        }
        public IEnumerable<Test> GetTestIsTrue()
        {
            return _testRepository.Find(m => m.Status == true);
        }
        public IEnumerable<TestDetailViewModel> GetTestById2(int id)
        {
            return _testRepository.Find(m => m.Id == id)
                                   .Select(test => new TestDetailViewModel
                                   {
                                       Id = test.Id,
                                       Name = test.Name,
                                       Description = test.Description,
                                       NumberOfQuestion = test.NumberOfQuestion,
                                       NumberOfPart = test.NumberOfPart,
                                       NumberOfUserTest = test.NumberOfUserTest,
                                       Duration = test.Duration,
                                       EducationCenterId = test.EducationCenterId,
                                       Score = test.Score,
                                       Status = test.Status,
                                       CreatedAt = test.CreatedAt,
                                       EducationCenterName = _educationCenterRepository.Find(ec => ec.Id == test.EducationCenterId).FirstOrDefault()?.Name != null ? test.EducationCenter.Name : "Unknown", // Kiểm tra xem EducationCenter có null không
                                       CourseName = _courseRepository.Find(ec => ec.Id == test.CourseId).FirstOrDefault()?.Name != null ? test.Course.Name : "Unknown" // Kiểm tra xem EducationCenter có null không
                                   });
        }
        //public Test FindUserByEmail(string email)
        //{


        //    return _testRepository.Find(m => m.Email == email).FirstOrDefault();
        //}
        //public User FindUserByUserName(string username)
        //{


        //    return _testRepository.Find(m => m.UserName == username).FirstOrDefault();
        //}

        public void AddTest(Test test)
        {
			_testRepository.Add(test);
        }
		public void AddTestNoSaveChanges(Test test)
		{
			_testRepository.AddNoSaveChanges(test);
		}
		public void SaveChanges()
		{
            _testRepository.SaveChanges();
		}

		public void UpdateTest(Test test)
        {
			_testRepository.Update(test);
        }

        public void DeleteTest(int id)
        {
			_testRepository.Delete(id);
        }
        public IEnumerable<Test> GetTestByCenterId(int id)
        {
            return _testRepository.Find(m => m.EducationCenterId == id);
        }
		public IEnumerable<Test> GetTestByCenterIdApprove(int id)
		{
			return _testRepository.Find(m => m.EducationCenterId == id&& m.Status );
		}
		public IEnumerable<Test> GetTestByCenterIdNotApprove(int id)
		{
			return _testRepository.Find(m => m.EducationCenterId == id && m.Status == false);
		}
	}
}
