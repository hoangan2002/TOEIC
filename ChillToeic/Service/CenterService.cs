using ChillToeic.Models.Entity;
using ChillToeic.Repository;

namespace ChillToeic.Service
{
    public class CenterService
    {

        private readonly IRepository<EducationCenter> _CenterRepository;

        public CenterService(IRepository<EducationCenter> CenterRepository)
        {
            _CenterRepository = CenterRepository;
        }
        public IEnumerable<EducationCenter> GetCentersIsApprove()
        {
            return _CenterRepository.Find(m => m.IsApprove == true);
        }
        public IEnumerable<EducationCenter> GetAllCenter()
        {
            return _CenterRepository.GetAll();
        }

        public IEnumerable<EducationCenter> GetCentersByCenterId(int id)
        {
            return _CenterRepository.Find(m => m.Id == id);
        }
		public EducationCenter FindEducationCenterByUserName(string username)
		{


			return _CenterRepository.Find(m => m.UserName == username).FirstOrDefault();
		}

		public EducationCenter GetCourseById(int id)
        {
            return _CenterRepository.GetById(id);
        }

        public void AddCourse(EducationCenter Center)
        {
            _CenterRepository.Add(Center);
        }

        public void UpdateCourse(EducationCenter Center)
        {
            _CenterRepository.Update(Center);
        }

        public void DeleteCourse(int id)
        {
            _CenterRepository.Delete(id);
        }

    }
}
