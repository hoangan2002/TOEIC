using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using System.Linq.Expressions;

namespace ChillToeic.Service
{
	public class TestOfUserService
	{
		private readonly IRepository<TestOfUser> _testofuserRepository;

		public TestOfUserService(IRepository<TestOfUser> testofuserRepository)
		{
			_testofuserRepository = testofuserRepository;
		}

		public IEnumerable<TestOfUser> GetAllTestOfUser()
		{
			return _testofuserRepository.GetAll();
		}

		public TestOfUser? GetTestOfUserById(int id)
		{
			return _testofuserRepository.Find(m => m.Id == id).FirstOrDefault();
		}
		public TestOfUser? GetTestOfUserByUserIdAndTestId(int idUser, int idTest)
		{
			Expression<Func<TestOfUser, bool>> predicate =  tu => tu.UserId == idUser && tu.TestId == idTest; 
			return _testofuserRepository.Find(predicate).FirstOrDefault();
		}
		//public TestOfUser FindEducationCenterByEmail(string email)
		//{


		//	return _testofuserRepository.Find(m => m.Email == email).FirstOrDefault();
		//}
		//public TestOfUser FindEducationCenterByUserName(string username)
		//{


		//	return _testofuserRepository.Find(m => m.UserName == username).FirstOrDefault();
		//}

		public void AddTestOfUser(TestOfUser testOfUser)
		{
			_testofuserRepository.Add(testOfUser);
		}

		public void UpdateTestOfUser(TestOfUser testOfUser)
		{
			_testofuserRepository.Update(testOfUser);
		}

		public void DeleteTestOfUser(int id)
		{
			_testofuserRepository.Delete(id);
		}
        public double GetAverageScoreByUserId(int userId)
        {
            var userTests = _testofuserRepository.GetAll().Where(t => t.UserId == userId && t.Score != null).ToList();
            if (userTests.Any())
            {
                return userTests.Average(t => t.Score.Value);
            }
            return 0; // return 0 if no tests or scores found
        }
    }

}
