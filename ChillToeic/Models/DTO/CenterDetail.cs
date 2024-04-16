using ChillToeic.Models.Entity;

namespace ChillToeic.Models.DTO
{
	public class CenterDetail
	{
		public IEnumerable<Course> CoursesApprove { get; set; }
		public IEnumerable<Test> TestsApprove { get; set; }
		public IEnumerable<Course> CoursesNotApprove { get; set; }
        public IEnumerable<Test> TestsNotApprove { get; set; }
		public EducationCenter EducationCenter { get; set; }
	}
}
