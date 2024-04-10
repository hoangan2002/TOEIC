using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using System.Linq.Expressions;

namespace ChillToeic.Service
{
    public class TestService
    {
        private readonly IRepository<Test> _testRepository;

        public TestService(IRepository<Test> testRepository)
        {
			_testRepository = testRepository;
        }

        public IEnumerable<Test> GetAllTests()
        {
            return  _testRepository.GetAll();
        }

        public Test GetTestById(int id)
        {
            return _testRepository.Find(m => m.Id == id).FirstOrDefault();
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
    }
}
