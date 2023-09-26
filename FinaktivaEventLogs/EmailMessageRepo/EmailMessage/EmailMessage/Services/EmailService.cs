using EmailMessage.Configuration;
using EmailMessage.Models;
using EmailMessage.Models.Enum;
using EmailMessage.Services.Contract;
using Ical.Net;
using Ical.Net.CalendarComponents;
using Ical.Net.DataTypes;
using Ical.Net.Serialization;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MimeKit;
using MimeKit.Utils;
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
//using System.Net.Mail;
//using System.Net.Mime;

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
        public void SendNotificationWithTemplate(EmailNotificationDto emailNotification)
        {
            try
            {
                //logo para usar en el encabezado de los correos
                var logo = "";
                switch (emailNotification.LogoHeader)
                {
                    case ELogoHeader.Lilisoft:
                        logo = "http://lilisoft.net/assets/images/LogoLiliSoftQr300px.png";
                        break;
                    case ELogoHeader.CentroYClinica:
                        logo = "http://lilisoft.net/assets/images/logoCentro&ClinicaPabonx100.png";
                        break;
                    case ELogoHeader.Clinica:
                        logo = "http://lilisoft.net/assets/images/logoClinica.png";
                        break;
                    case ELogoHeader.Centro:
                        logo = "http://lilisoft.net/assets/images/logoCentro.png";
                        break;
                    case ELogoHeader.TarjetaPabon:
                        logo = "http://clinicardiopabon.com/wp-content/uploads/2022/07/logoPabonMasYCentrox100.png";
                        break;
                    case ELogoHeader.LilisoftCentroYClinica:
                        logo = "http://lilisoft.net/assets/images/Logos-ClinicaCentroLilisoft.png";
                        break;
                }
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

                // estructura del contenido del mensaje
                string htmlContentEmail = "";
                // plantilla del contenido del mensaje
                string contentTemplate1 = File.ReadAllText("/var/lib/Templates/notificationForEmailPart1.html");
                string contentTemplate2 = File.ReadAllText("/var/lib/Templates/notificationForEmailPart2.html");

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
                            FileName = "eventoLiliSoft.ics"
                        };

                        //var multipart = new Multipart("mixed");
                        //multipart.Add(attachment);
                        //multipart.Add(emailBodyBuilder.ToMessageBody()); // Agrega el cuerpo del correo electrónico
                        emailBodyBuilder.Attachments.Add(attachment);
                    }
                    // rutas temporales para cargar imágenes
                    List<string> patchs = new();
                    switch (emailNotification.Type)
                    {
                        // Email de notificación generico con template
                        case EEmailType.NotificationWithImage64:
                            // Complementar correo
                            htmlContentEmail = "";
                            // imágenes
                            string imagesHtml = "";
                            var index = 0;
                            // identificador
                            var contentId = Guid.NewGuid().ToString();
                            foreach (var image in emailNotification.Images64)
                            {
                                // convertir a imagen en bytes desde base 64
                                var imageData = Convert.FromBase64String(image);
                                // Obtiene ruta temporal
                                var fullPath = Path.GetTempPath();
                                // carga imagen en bytes a ruta 
                                using (var img1 = Image.Load(imageData))
                                {
                                    img1.Save(fullPath+ contentId + ".png");
                                    //Almacena ruta en array
                                    patchs.Add(fullPath+ contentId + ".png");
                                };
                                // Cadena con etiquetas de imagen a gráficar
                                string imgHtml = "<img style='max-width: calc(min(80%, 500px)); min-height: 100px; min-width: 100px;' src='cid:{" + index + "}'><br>";
                                //Reemplazar src con id de imagen
                                imagesHtml += String.Format(imgHtml, contentId);
                                index++;
                            }

                            var htmlActions = "";
                            foreach (var actions in emailNotification.HtmlActions)
                            {
                                htmlActions += actions;
                            }
                            emailNotification.ContentHtml += imagesHtml;
                            htmlContentEmail = String.Format(contentTemplate2, emailNotification.Title, logo, emailNotification.Greeting, emailNotification.DisplayName, emailNotification.Description, emailNotification.ContentHtml, htmlActions);

                            // se une parte 1 de la plantilla con parte 2 formateada con nuevo contenido
                            EmailTemplateText = contentTemplate1 + htmlContentEmail;

                            // Ciclo para añadir archivos adjuntos ocultos
                            foreach (var lr in patchs)
                            {
                                //añade archivo adjunto
                                var images = emailBodyBuilder.LinkedResources.Add(lr);
                                // se asigna identificador para ocultarlo en adjuntos y cargarlo en template
                                images.ContentId = contentId;
                            }
                            break;
                    }

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

                // rutas temporales para cargar imágenes
                List<string> patchs = new();
                switch (emailNotification.Type)
                {
                    // Email de notificación generico con template
                    case EEmailType.NotificationWithImage64:
                        // Complementar correo
                        htmlContentEmail = "";
                        // imágenes
                        string imagesHtml = "";
                        var index = 0;
                        // identificador
                        var contentId = Guid.NewGuid().ToString();
                        if(emailNotification.Images64 != null)
                        {
                            foreach (var image in emailNotification.Images64)
                            {
                                // convertir a imagen en bytes desde base 64
                                var imageData = Convert.FromBase64String(image);
                                // Obtiene ruta temporal
                                var fullPath = Path.GetTempPath();
                                var imageFileName = $"{contentId}.png";
                                var fullImagePath = Path.Combine(fullPath, imageFileName);
                                // carga imagen en bytes a ruta 
                                using (var img1 = Image.Load(imageData))
                                {
                                    img1.Save(fullImagePath);
                                };
                                //Almacena ruta en array
                                patchs.Add(fullImagePath);
                                // Cadena con etiquetas de imagen a gráficar
                                string imgHtml = "<img style='max-width: calc(min(80%, 500px)); min-height: 100px; min-width: 100px;' src='cid:{" + index + "}'><br>";
                                //Reemplazar src con id de imagen
                                imagesHtml += String.Format(imgHtml, contentId);
                                index++;
                            }
                        }

                        var htmlActions = "";
                        foreach (var actions in emailNotification.HtmlActions)
                        {
                            htmlActions += actions;
                        }
                        emailNotification.ContentHtml += imagesHtml;
                        htmlContentEmail = String.Format(contentTemplate2, emailNotification.Title, logo, emailNotification.Greeting, emailNotification.DisplayName, emailNotification.Description, emailNotification.ContentHtml, htmlActions);

                        // se une parte 1 de la plantilla con parte 2 formateada con nuevo contenido
                        EmailTemplateText = contentTemplate1 + htmlContentEmail;

                        // Ciclo para añadir archivos adjuntos ocultos
                        foreach (var lr in patchs)
                        {
                            //añade archivo adjunto
                            var images = emailBodyBuilder.LinkedResources.Add(lr);
                            // se asigna identificador para ocultarlo en adjuntos y cargarlo en template
                            images.ContentId = contentId;
                        }
                        break;
                }
                
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
                throw new ArgumentException($"Error en EmailMicroservice::SendUserWelcomeEmail:: {ex.Message}");
            }
        }

    }
}
