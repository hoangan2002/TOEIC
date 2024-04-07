using ChillToeic.Models.Entity;
using ChillToeic.Repository;

namespace ChillToeic.Service
{
    public class LearningProgressService
    {

        private readonly IRepository<LearningProgress> _LearningProgressRepository;

        public LearningProgressService(IRepository<LearningProgress> LearningProgressRepository)
        {
            _LearningProgressRepository = LearningProgressRepository;
        }

        public IEnumerable<LearningProgress> GetAllLearningProgresss()
        {
            return _LearningProgressRepository.GetAll();
        }

        public IEnumerable<LearningProgress> GetLearningProgressByUserId(int id)
        {
            return _LearningProgressRepository.Find(m => m.UserId == id);
        }

        public IEnumerable<LearningProgress> GetLearningProgressById(int id)
        {
            return _LearningProgressRepository.Find(m => m.Id == id);
        }

        public void AddLearningProgress(LearningProgress LearningProgress)
        {
            _LearningProgressRepository.Add(LearningProgress);
        }

        public void UpdateLearningProgress(LearningProgress LearningProgress)
        {
            _LearningProgressRepository.Update(LearningProgress);
        }

        public void DeleteLearningProgress(int id)
        {
            _LearningProgressRepository.Delete(id);
        }


    }
}
