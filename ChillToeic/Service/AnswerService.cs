using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using System.Linq.Expressions;

namespace ChillToeic.Service
{
    public class AnswerService
    {
        private readonly IRepository<Answer> _answerRepository;

        public AnswerService(IRepository<Answer> answerRepository)
        {
			_answerRepository = answerRepository;
        }

        public IEnumerable<Answer> GetAllAnswers()
        {
            return _answerRepository.GetAll();
        }

        public Answer GetAnswerById(int id)
        {
            return _answerRepository.Find(m =>m.Id == id).FirstOrDefault();
        }
        //public Answer FindUserByEmail(string email)
        //{
          

        //    return _answerRepository.Find(m => m.Email == email).FirstOrDefault();
        //}
        //public Answer FindAnswerByUserName(string username)
        //{


        //    return _answerRepository.Find(m => m.UserName == username).FirstOrDefault();
        //}

        public void AddAnswer(Answer user)
        {
			_answerRepository.Add(user);
        }

        public void UpdateAnswer(Answer user)
        {
			_answerRepository.Update(user);
        }

        public void DeleteAnswer(int id)
        {
			_answerRepository.Delete(id);
        }
    }
}
