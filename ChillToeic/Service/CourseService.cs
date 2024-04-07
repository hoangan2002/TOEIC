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

		public void AddCourse(Course Course)
		{
			_CourseRepository.Add(Course);
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
