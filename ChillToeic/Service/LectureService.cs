using ChillToeic.Models.Entity;
using ChillToeic.Repository;

namespace ChillToeic.Service
{
    public class LectureService
    {

        private readonly IRepository<Lecture> _LectureRepository;

        public LectureService(IRepository<Lecture> LectureRepository)
        {
            _LectureRepository = LectureRepository;
        }

        public IEnumerable<Lecture> GetLecturesByCourseId(int courseId)
        {
            return _LectureRepository.Find(lecture => lecture.CourseId == courseId);
        }
		//public IList<Lecture> GetLecturesByCourseId2(int courseId)
		//{
		//	return _LectureRepository.Find(lecture => lecture.CourseId == courseId, m=> m.LectureDetails);
		//}
		public IEnumerable<Lecture> GetAllLecture()
        {
            return _LectureRepository.GetAll();
        }

        public Lecture GetLectureById(int id)
        {
            return _LectureRepository.GetById(id);
        }

        public void AddLecture(Lecture Lecture)
        {
            _LectureRepository.Add(Lecture);
        }

        public void UpdateCourse(Lecture Lecture)
        {
            _LectureRepository.Update(Lecture);
        }

        public void DeleteLecture(int id)
        {
            _LectureRepository.Delete(id);
        }
    }
}
