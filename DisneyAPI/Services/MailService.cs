using DisneyAPI.Interfaces;
using DisneyAPI.ViewModels.Services.MailService;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisneyAPI.Services
{
    public class MailService : IMailService
    {
        private readonly ISendGridClient _SendGridclient;

        public MailService(ISendGridClient client)
        {
            _SendGridclient = client;
        }
        public async Task SendEmail(MailServiceRequestViewModel model)
        {
            try
            {
                var msg = new SendGridMessage()
                {
                    From = new EmailAddress(Startup.Configuration["MailSettings:Sender"], "DX Team"),
                    Subject = Startup.Configuration["MailSettings:Subject"]
                };
                msg.AddContent(MimeType.Text, $"Bienvenido {model.Username} a nuestra app!!!");
                msg.AddTo(new EmailAddress(model.Email, Startup.Configuration["MailSettings:Nickname"]));
                var response = await _SendGridclient.SendEmailAsync(msg);
                if (!response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"{response.StatusCode} /n Error en el envío del email de confirmación de registro");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return ;
            }
            
        }
    }
}
