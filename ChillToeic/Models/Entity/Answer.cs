using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChillToeic.Models.Entity
{
    public class Answer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int QuestionNumber { get; set; }
        public int AnswerQuestion { get; set; }
        [ForeignKey("TestOfUser")]
        public int TestOfUserId { get; set; }
        public virtual TestOfUser TestOfUser { get; set; }
    }
}
