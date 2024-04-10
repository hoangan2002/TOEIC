using ChillToeic.Models.Entity;
using ChillToeic.Repository;
using System.Linq.Expressions;

namespace ChillToeic.Service
{
    public class QuestionDetailService
    {
        private readonly IRepository<QuestionDetail> _questiondetailRepository;

        public QuestionDetailService(IRepository<QuestionDetail> questiondetailRepository)
        {
			_questiondetailRepository = questiondetailRepository;
        }

        public IEnumerable<QuestionDetail> GetAllQuestionDetails()
        {
            return _questiondetailRepository.GetAll();
        }

        public QuestionDetail GetQuestionDetailById(int id)
        {
            return _questiondetailRepository.Find(m =>m.Id == id).FirstOrDefault();
        }
        //public QuestionDetail FindUserByEmail(string email)
        //{
          

        //    return _questiondetailRepository.Find(m => m.Email == email).FirstOrDefault();
        //}
        //public QuestionDetail FindQuestionDetailByUserName(string username)
        //{


        //    return _questiondetailRepository.Find(m => m.UserName == username).FirstOrDefault();
        //}

        public void AddQuestionDetail(QuestionDetail user)
        {
			_questiondetailRepository.Add(user);
        }
		public void AddQuestionDetailNoSaveChanges(QuestionDetail question)
		{
			_questiondetailRepository.AddNoSaveChanges(question);
		}
		public void UpdateQuestionDetail(QuestionDetail user)
        {
			_questiondetailRepository.Update(user);
        }

        public void DeleteQuestionDetail(int id)
        {
			_questiondetailRepository.Delete(id);
        }
    }
}
