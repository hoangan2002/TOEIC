using System.ComponentModel.DataAnnotations;

namespace ChillToeic.Models.Entity
{
    public class QuestionType
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public virtual ICollection<Question> Questions { get; set; }
    }
}
