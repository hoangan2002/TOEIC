using System.ComponentModel.DataAnnotations;

namespace ChillToeic.Models.DTO
{
    public class UserResetPasswordDTO
    {
        [Required]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}")]
        public string Email { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        public string? VerifyOTP { get; set; }
    }
}
