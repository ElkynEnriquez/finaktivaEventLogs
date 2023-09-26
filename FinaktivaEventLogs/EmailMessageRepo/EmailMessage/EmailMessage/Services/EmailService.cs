using EmailMessage.Configuration;
using EmailMessage.Models;
using EmailMessage.Services.Contract;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;
using System;
using System.IO;
using System.Text;

namespace EmailMessage.Services
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;
        public EmailService(EmailSettings emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings;
            _logger = logger;
        }
        public void SendNotification(EmailNotificationDto emailNotification)
        {
            try
            {
                //logo para usar en el encabezado de los correos
                var logo = "https://finaktiva.com/assets/images/miniatura-redes-sociales.jpg";
                MimeMessage emailMessage = new();
                // De:
                MailboxAddress emailFrom = new(_emailSettings.Name, _emailSettings.EmailId);
                emailMessage.From.Add(emailFrom);
                // Para:
                MailboxAddress emailTo = new(emailNotification.DisplayName, emailNotification.To);
                emailMessage.To.Add(emailTo);
                // Asunto
                emailMessage.Subject = emailNotification.Subject;

                BodyBuilder emailBodyBuilder = new();
                string EmailTemplateText = "";

                // eventos
                if (emailNotification.Events.Count > 0)
                {
                    foreach (EventICalendarDto eventCalendar in emailNotification.Events)
                    {
                        var calendar = new Calendar();
                        calendar.AddTimeZone(new VTimeZone("America/Bogota"));
                        calendar.Method = "REQUEST";
                        var calendarEvent = new CalendarEvent
                        {
                            Start = new CalDateTime(eventCalendar.StartDate),
                            End = new CalDateTime(eventCalendar.EndDate),
                            Description = eventCalendar.Description,
                            Summary = eventCalendar.Summary,
                            //Name = "Recordatori name",
                            Organizer = new Organizer()
                            {
                                CommonName = eventCalendar.NameOrganizer,
                                Value = new Uri("mailto:"+eventCalendar.EmailOrganizer)
                            }
                        };
                        calendar.Events.Add(calendarEvent);
                        var serializer = new CalendarSerializer();
                        var serializedCalendarContent = serializer.SerializeToString(calendar);

                        var attachment = new MimePart("text", "calendar")
                        {
                            Content = new MimeContent(new MemoryStream(Encoding.UTF8.GetBytes(serializedCalendarContent))),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = "event.ics"
                        };

                        emailBodyBuilder.Attachments.Add(attachment);
                    }
                    
                    EmailTemplateText = emailNotification.ContentHtml;

                    emailBodyBuilder.HtmlBody = EmailTemplateText;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();

                    SmtpClient emailClient = new();
                    emailClient.Connect(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);
                    emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                    emailClient.Send(emailMessage);
                    emailClient.Disconnect(true);
                    emailClient.Dispose();
                    _logger.LogInformation(emailClient.ToString());
                } else { 

                emailBodyBuilder.HtmlBody = EmailTemplateText;

                emailMessage.Body = emailBodyBuilder.ToMessageBody();

                SmtpClient emailClient = new();
                emailClient.Connect(_emailSettings.Host, _emailSettings.Port, _emailSettings.UseSSL);
                emailClient.Authenticate(_emailSettings.EmailId, _emailSettings.Password);
                emailClient.Send(emailMessage);
                emailClient.Disconnect(true);
                emailClient.Dispose();
                _logger.LogInformation(emailClient.ToString());
            }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.Message);
                throw new ArgumentException($"Error en EmailMicroservice::SendEmail:: {ex.Message}");
            }
        }

    }
}
