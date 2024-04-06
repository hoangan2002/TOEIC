using ChillToeic.Models.DTO;

namespace ChillToeic.Infrastructure.EmailSender
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email);
    }
}
