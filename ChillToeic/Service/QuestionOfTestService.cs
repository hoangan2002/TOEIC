using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Linq.Expressions;

namespace ChillToeic.Service
{
    public class QuestionOfTestService
    {
        private readonly IRepository<QuestionOfTest> _questionoftestRepository;

        public QuestionOfTestService(IRepository<QuestionOfTest> questionoftestRepository)
        {
			_questionoftestRepository = questionoftestRepository;
        }

        public IEnumerable<QuestionOfTest> GetAll()
        {
            return _questionoftestRepository.GetAll();
        }

        public QuestionOfTest GetQuestionOfTestById(int id)
        {
            return _questionoftestRepository.Find(m => m.Id == id).FirstOrDefault();
        }
        //public QuestionOfTest FindUserByEmail(string email)
        //{
          

        //    return _userRepository.Find(m => m.Email == email).FirstOrDefault();
        //}
        //public QuestionOfTest FindUserByUserName(string username)
        //{


        //    return _userRepository.Find(m => m.UserName == username).FirstOrDefault();
        //}

        public void Add(QuestionOfTest questionOfTest)
        {
			_questionoftestRepository.Add(questionOfTest);
        }

        public void Update(QuestionOfTest questionOfTest)
        {
			_questionoftestRepository.Update(questionOfTest);
        }

        public void Delete(int id)
        {
			_questionoftestRepository.Delete(id);
        }
		public IEnumerable<Question> GetQuestionsByTestId(int testId)
		{
            List<Question> questions = new List<Question>(); 
             foreach (var i in _questionoftestRepository.Find(x => x.TestId == testId, x => x.Question.QuestionDetails))
            {
                Question question = i.Question;
                questions.Add(question);
            }
            return questions;
		}
	}
}
