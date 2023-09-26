using EmailMessage.Models;

namespace EmailMessage.Services.Contract
{
    public interface IEmailService
    {
        void SendNotification(EmailNotificationDto emailNotification);
    }
}
