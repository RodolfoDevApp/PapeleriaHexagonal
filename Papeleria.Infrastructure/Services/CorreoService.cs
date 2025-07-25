using Microsoft.Extensions.Options;
using Papeleria.Application.DTOs.Correo;
using Papeleria.Application.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;

namespace Papeleria.Infrastructure.Services
{
    public class CorreoService : ICorreoService
    {
        private readonly SmtpSettings _settings;

        public CorreoService(IOptions<SmtpSettings> settings)
        {
            _settings = settings.Value;
        }

        public async Task EnviarCorreoAsync(string destinatario, string asunto, string contenidoHtml)
        {
            try
            {

                var mensaje = new MimeMessage();
                mensaje.From.Add(new MailboxAddress(_settings.FromName, _settings.From));
                mensaje.To.Add(MailboxAddress.Parse(destinatario));
                mensaje.Subject = asunto;

                mensaje.Body = new TextPart("html") { Text = contenidoHtml };

                using var client = new SmtpClient();
                await client.ConnectAsync(_settings.Host, _settings.Port, _settings.EnableSsl);
                await client.AuthenticateAsync(_settings.User, _settings.Password);
                await client.SendAsync(mensaje);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Error al enviar el correo a {destinatario}. Detalles: {ex.Message}", ex);
            }
        }
    }
}
