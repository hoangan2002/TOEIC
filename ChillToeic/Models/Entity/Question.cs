using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChillToeic.Models.Entity
{
    public class Question
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Title { get; set; }
        public string Detail { get; set; }
        [ForeignKey("QuestionType")]
        public int QuestionTypeId { get; set; }
        
        public virtual QuestionType QuestionType { get; set; }
        public virtual ICollection<QuestionDetail> QuestionDetails { get; set; }
    }
}
