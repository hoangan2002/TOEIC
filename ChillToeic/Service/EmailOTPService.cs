using ChillToeic.Models.Entity;
using ChillToeic.Repository;

namespace ChillToeic.Service
{
    
    public class EmailOTPService
    {
        private readonly IRepository<EmailOTP> _emailOTPRepository;

        public EmailOTPService(IRepository<EmailOTP> emailOTPRepository)
        {
            _emailOTPRepository = emailOTPRepository;
        }
        public string GetOTPByEmail(string email)
        {
            return _emailOTPRepository.Find(m => m.Email == email).FirstOrDefault().OTP;
        }
        
        public void AddEmailOTP(EmailOTP emailOTP)
        {
            _emailOTPRepository.Add(emailOTP);
        }
        public void DeleteEmailOTP(string email)
        {
            _emailOTPRepository.Delete(_emailOTPRepository.Find(m=>m.Email == email).FirstOrDefault().Id);
        }
    }
}
