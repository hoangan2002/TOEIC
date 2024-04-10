using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChillToeic.Models.Entity
{
    public class Test
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        [ForeignKey("TestType")]
        public int TestTypeId { get; set; }
        public virtual TestType TestType { get; set; }
        public int NumberOfQuestion { get; set; }
        public int NumberOfPart { get; set; }
        public int NumberOfUserTest { get; set; }
        public int Duration { get; set; }
        public int Score { get; set; }
        public Boolean Status { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        
        public int? EducationCenterId { get; set; }
        public virtual EducationCenter EducationCenter { get; set; }
        
        public int? CourseId { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<TestOfUser> TestOfUsers { get; set; }
        public virtual ICollection<QuestionOfTest> QuestionOfTests { get; set; }
    }
}
