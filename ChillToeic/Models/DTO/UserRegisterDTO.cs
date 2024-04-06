using System.ComponentModel.DataAnnotations;

namespace ChillToeic.Models.DTO
{
    public class UserRegisterDTO
    {
        [Required]   
        public string FullName { get; set; }
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string VerifyOTP { get; set; }
    }
}
