using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChillToeic.Models.Entity
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Point { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        [ForeignKey("Role")]
        public int RoleId { get; set; }
        public virtual Role Role { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Certificate> Certificates { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<TestOfUser> TestOfUsers { get; set; }

    }
}
