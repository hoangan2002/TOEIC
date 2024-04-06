using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlTypes;

namespace ChillToeic.Models.Entity
{
    public class Course
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string level { get; set; }
        public int QuantityOfLecture { get; set; }
        public int QuantityOfRegister { get; set; }
        public Boolean IsApproved { get; set; }
        public DateTime CreatedAt { get; set; }
        [ForeignKey("EducationCenter")]
        public int EducationCenterId { get; set; }
        public virtual EducationCenter EducationCenter { get; set; }
        public virtual ICollection<Test> Tests { get; set; }
        public virtual ICollection<Lecture> Lectures { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Discount> Discounts { get; set; }
    }
}
