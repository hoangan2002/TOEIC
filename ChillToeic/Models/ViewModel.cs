using System.ComponentModel.DataAnnotations.Schema;

namespace ChillToeic.Models.Entity
{
    public class CreateNewCourseInfo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int level { get; set; }
        public int QuantityOfLecture { get; set; }
        public int QuantityOfRegister { get; set; }
        public Boolean IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("EducationCenter")]
        public int EducationCenterId { get; set; }

        public IEnumerable<LectureInfo> LectureInfos { get; set; }
    }

    // ViewModel la gi?
    public class LectureInfo
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }

        public int CourseId { get; set; }

        public int LectureTypeId { get; set; }

        public string Title { get; set; }
        public IFormFile? Content { get; set; }
    }
}
