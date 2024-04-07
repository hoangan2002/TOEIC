using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChillToeic.Models.Entity
{
	public class LearningProgress
	{
		[Key]
		public int Id { get; set; }

		[ForeignKey("User")]
		public int UserId { get; set; }
		public virtual User User { get; set; }

		[ForeignKey("Course")]
		public int CourseId { get; set; }
		public virtual Course Course { get; set; }

		[ForeignKey("Lecture")]
		public int LectureId { get; set; }
		public virtual Lecture Lecture { get; set; }

		[ForeignKey("LectureDetail")]
		public int LectureDetailId { get; set; }
		public virtual LectureDetail LectureDetail { get; set; }

		public Boolean IsCompleted { get; set; }
        internal static object Where(Func<object, object> value)
        {
            throw new NotImplementedException();
        }
    }
}
