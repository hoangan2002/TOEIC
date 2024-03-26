using System.ComponentModel.DataAnnotations.Schema;

namespace ChillToeic.Models.Entity
{
    public class Lecture
    {
        public int Id { get; set; }
        public DateTime Time { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }
        public virtual Course Course { get; set; }
        [ForeignKey("LectureType")]
        public int LectureTypeId { get; set; }
        public virtual LectureType LectureType { get; set; }
        public virtual ICollection<LectureDetail> LectureDetails { get; set; }


    }
}
