using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ChillToeic.Models.Entity
{
    public class TestOfUser
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int? Score { get; set; } = 0;

        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("Test")]
        public int TestId { get; set; }
        public virtual Test Test { get; set; }
        public int? CourseId { get; set; } = null;
        public virtual Course Course { get; set; }
    }
    
}
