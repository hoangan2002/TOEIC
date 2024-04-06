using ChillToeic.Models.Entity;
using ChillToeic.Repository;

namespace ChillToeic.Service
{
    
    public class EducationService
    {
        private readonly IRepository<EducationCenter> _educationRepository;

        public EducationService(IRepository<EducationCenter> educationRepository)
        {
            _educationRepository = educationRepository;
        }

        public IEnumerable<EducationCenter> GetAllEducationCenter()
        {
            return _educationRepository.GetAll();
        }

        public EducationCenter GetEducationCenterById(int id)
        {
            return _educationRepository.Find(m => m.Id == id).FirstOrDefault();
        }
        public EducationCenter FindEducationCenterByEmail(string email)
        {


            return _educationRepository.Find(m => m.Email == email).FirstOrDefault();
        }
        public EducationCenter FindEducationCenterByUserName(string username)
        {


            return _educationRepository.Find(m => m.UserName == username).FirstOrDefault();
        }

        public void AddEducationCenter(EducationCenter educationCenter)
        {
            _educationRepository.Add(educationCenter);
        }

        public void UpdateEducationCenter(EducationCenter educationCenter)
        {
            _educationRepository.Update(educationCenter);
        }

        public void DeleteEducationCenter(int id)
        {
            _educationRepository.Delete(id);
        }
    }
}
