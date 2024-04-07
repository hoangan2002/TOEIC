namespace ChillToeic.Models.Entity
{
    public class HomeModel
    {
        public List<EducationCenter> Centers { get; set; }
        public List<Course> CoursesWithinRange { get; set; }
        public List<Course> Top3Courses { get; set; }
        public List<Test> Tests { get; set; }
    }
}
