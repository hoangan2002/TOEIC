using ChillToeic.Models.DTO;
using ChillToeic.Service;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;

namespace ChillToeic.Infrastructure.EmailSender
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;
        private readonly EmailOTPService _emailOTPService;
        public EmailService(IConfiguration config, EmailOTPService emailOTPService)
        {
            _config = config;
            _emailOTPService = emailOTPService;
        }
        public async Task SendEmailAsync(string email0)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["EmailUsername"]));
            email.To.Add(MailboxAddress.Parse(email0));
            email.Subject = "Email Verify";
            string OTP = GenerateOTP();
            // Nếu bạn muốn sử dụng HTML trong nội dung email, sử dụng TextFormat.Html
            email.Body = new TextPart(TextFormat.Plain) { Text = "Your OTP " + OTP };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["EmailHost"], 587, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["EmailUsername"], _config["EmailPassword"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
            _emailOTPService.AddEmailOTP(new Models.Entity.EmailOTP { Email = email0, OTP = OTP });
        }
        private string GenerateOTP()
        {
            Random random = new Random();
            int otpNumber = random.Next(1000, 10000);
            return otpNumber.ToString();
            
        }
    }
}
