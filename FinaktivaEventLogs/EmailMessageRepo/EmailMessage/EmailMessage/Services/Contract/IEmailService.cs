using EmailMessage.Models;

namespace EmailMessage.Services.Contract
{
    public interface IEmailService
    {
        void SendNotificationWithTemplate(EmailNotificationDto emailNotification);
    }
}
