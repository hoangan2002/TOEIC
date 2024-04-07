using ChillToeic.Models.Entity;
using ChillToeic.Repository;

namespace ChillToeic.Service
{
	public class LectureDetailService
	{
		private readonly IRepository<LectureDetail> _lectureDetailRepository;

		public LectureDetailService(IRepository<LectureDetail> lectureDetailRepository)
		{
			_lectureDetailRepository = lectureDetailRepository;
		}

		public IEnumerable<LectureDetail> GetAllLectureDetails()
		{
			var _lectureDetail = _lectureDetailRepository.GetAll();
            return _lectureDetail;

		}

		//public LectureDetail GetLectureDetailById(int id)
		//{
		//	return _lectureDetailRepository.GetById(id);
		//}

		public void AddLectureDetail(LectureDetail lectureDetail)
		{
			_lectureDetailRepository.Add(lectureDetail);
		}

		public void UpdateLectureDetail(LectureDetail lectureDetail)
		{
			_lectureDetailRepository.Update(lectureDetail);
		}

		public void DeleteLectureDetail(int id)
		{
			_lectureDetailRepository.Delete(id);
		}

        public IEnumerable<LectureDetail> GetLectureDetailsByLectureId(int lectureId)
        {
            return _lectureDetailRepository.Find(detail => detail.LectureId == lectureId);
        }
        public IEnumerable<LectureDetail> GetLectureDetailById(int id)
        {
            return _lectureDetailRepository.Find(detail => detail.Id == id);
        }
    }
}
