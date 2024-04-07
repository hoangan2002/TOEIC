using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChillToeic.Models.Entity
{
    public class Notification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Description { get; set; }
        public Boolean HaveRead { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
      
        public int? TestId { get; set; }
        public virtual Test Test { get; set; }
       
        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }
    
        public int? CertificateId { get; set; }
        public virtual Certificate Certificate { get; set; }
   
        public int? OrderId { get; set; }
        public virtual Order Order { get; set; }

    }
}
