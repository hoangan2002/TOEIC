using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using System.Linq.Expressions;

namespace ChillToeic.Service
{
    public class QuestionService
    {
        private readonly IRepository<Question> _questionRepository;

        public QuestionService(IRepository<Question> questionRepository)
        {
			_questionRepository = questionRepository;
        }

        public IEnumerable<Question> GetAllQuestions()
        {
            return _questionRepository.GetAll();
        }

        public Question GetQuestionById(int id)
        {
            return _questionRepository.Find(m => m.Id == id).FirstOrDefault();
        }
        //public Question FindUserByEmail(string email)
        //{
          

        //    return _questionRepository.Find(m => m.Email == email).FirstOrDefault();
        //}
        //public Question FindUserByUserName(string username)
        //{


        //    return _questionRepository.Find(m => m.UserName == username).FirstOrDefault();
        //}

        public void AddQuestion(Question question)
        {
			_questionRepository.Add(question);
        }

        public void UpdateQuestion(Question question)
        {
			_questionRepository.Update(question);
        }

        public void DeleteQuestion(int id)
        {
			_questionRepository.Delete(id);
        }
		public IEnumerable<QuestionDetail> GetQuestionDetailsByQuestionId(int questionId)
		{
		
			return _questionRepository.Find(x => x.Id == questionId, x => x.QuestionDetails).FirstOrDefault().QuestionDetails;
		}
	}
}
