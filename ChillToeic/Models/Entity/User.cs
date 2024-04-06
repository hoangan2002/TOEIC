using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChillToeic.Models.Entity
{
    
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; } = 0;
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Point { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; } = 1;
        public virtual Role Role { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<TestOfUser> TestOfUsers { get; set; }

    }
}
