using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChillToeic.Models.Entity
{
    public class LectureDetail
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
      
        [ForeignKey("Lecture")]
        public int LectureId { get; set; }
        public virtual Lecture Lecture { get; set; }
    }
}

