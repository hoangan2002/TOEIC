using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChillToeic.Models.Entity
{
    public class QuestionDetail
    {
        [Key]
        public int Id { get; set; }
        public int NumberQuestion { get; set; }
        public int AnswerA { get; set; }
        public int AnswerB { get; set; }
        public int AnswerC { get; set; }
        public int AnswerD { get; set; }
        public int CorrectAnswer { get; set; }
        public string Description { get; set; }
        [ForeignKey("Question")]
        public int QuestionId { get; set; }

        public virtual Question Question { get; set; }
    }
}
