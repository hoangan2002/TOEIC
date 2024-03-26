using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChillToeic.Models.Entity
{
    public class Discount
    {
      
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; }
        public int DiscountValue { get; set; }
        [ForeignKey("Course")]
        public int CourseId { get; set; }

        public virtual Course Course { get; set; }
    }
}
