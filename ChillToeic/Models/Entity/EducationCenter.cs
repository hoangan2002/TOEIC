using System.ComponentModel.DataAnnotations;

namespace ChillToeic.Models.Entity
{
    public class EducationCenter
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public string Certification {  get; set; }
        public Boolean IsApprove { get; set; }
        public DateTime CreatedAt { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        public virtual ICollection<Course> Courses { get; set; }
    }
}
