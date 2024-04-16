using ChillToeic.Models.Entity;
using ChillToeic.Repository;

namespace ChillToeic.Service
{
	public class CourseService
	{

		private readonly IRepository<Course> _CourseRepository;

		public CourseService(IRepository<Course> CourseRepository)
		{
			_CourseRepository = CourseRepository;
		}

		public IEnumerable<Course> GetAllCourses()
		{
			return _CourseRepository.GetAll();
		}

		public Course GetCourseById(int id)
		{
			return _CourseRepository.GetById(id);
		}
		public IEnumerable<Course> GetCourseByCenterIdApprove(int id)
		{
			return _CourseRepository.Find(m=>m.EducationCenterId == id && m.IsApproved );
		}
		public IEnumerable<Course> GetCourseByCenterIdNotApprove(int id)
		{
			return _CourseRepository.Find(m => m.EducationCenterId == id && m.IsApproved ==false);
		}
		public void AddCourse(Course Course)
		{
			_CourseRepository.Add(Course);
		}
        public IEnumerable<Course> GetCoursesByCourseId(int id)
        {
            return _CourseRepository.Find(m => m.Id == id);
        }
        public void UpdateCourse(Course Course)
		{
			_CourseRepository.Update(Course);
		}

		public void DeleteCourse(int id)
		{
			_CourseRepository.Delete(id);
		}
        public IEnumerable<Course> GetCoursesByCenterId(int id)
        {
            return _CourseRepository.Find(m => m.EducationCenterId == id);
        }
    }
}
