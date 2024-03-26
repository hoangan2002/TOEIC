using System.ComponentModel.DataAnnotations;

namespace ChillToeic.Models.Entity
{
    public class LectureType
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<Lecture> Lectures { get; set; }
    }
}
