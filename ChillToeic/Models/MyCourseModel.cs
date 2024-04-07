using ChillToeic.Models.Entity;

namespace ChillToeic.Models
{
    public class MyCourseModel
    {
        public List<LearningProgress> LearningProgresss { get; set; }
        public List<Course> Courses { get; set; }
        public List<Lecture> Lectures { get; set; }
        public List<LectureDetail> LectureDetails { get; set; }
    }
}
