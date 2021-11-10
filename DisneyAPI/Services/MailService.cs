using DisneyAPI.Interfaces;
using DisneyAPI.ViewModels.Services.MailService;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace DisneyAPI.Services
{
    public class MailService : IMailService
    {
        private readonly ISendGridClient _SendGridclient;
        private readonly ILogger<MailService> _logger;

        public MailService(ISendGridClient client, ILogger<MailService> logger)
        {
            _SendGridclient = client;
            _logger = logger;
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
            catch (AggregateException ae)
            {
                foreach (var e in ae.InnerExceptions)
                {
                    Console.WriteLine("{0}:\n   {1}", e.GetType().Name, e.Message);
                    _logger.LogError($"Unhandled exception: {e.Message} ");
                }
            }

        }
    }
}
