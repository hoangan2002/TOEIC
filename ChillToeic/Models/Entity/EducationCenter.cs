using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChillToeic.Models.Entity
{
    public class EducationCenter
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Img { get; set; }
        public string? Description { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }
        public string? Address { get; set; }
        public string? Certification {  get; set; }
        public Boolean IsApprove { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        [ForeignKey("Role")]
        public int RoleId { get; set; } = 3;
        public virtual Role Role { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
