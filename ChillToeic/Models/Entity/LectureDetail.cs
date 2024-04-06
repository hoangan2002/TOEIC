using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChillToeic.Models.Entity
{
    public class LectureDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
      
        [ForeignKey("Lecture")]
        public int LectureId { get; set; }
        public virtual Lecture Lecture { get; set; }
        [ForeignKey("LectureType")]
        public int LectureTypeId { get; set; }
        public virtual LectureType LectureType { get; set; }

    }
}

