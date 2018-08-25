using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Email.Services
{
     public class EmailService : IEmailService
     {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmail(string name, string phone, string email, string subject, string message)
        {
            using (var client = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = _configuration["Email:Email"],
                    Password = _configuration["Email:Password"]
                };

                client.Credentials = credential;
                client.Host = _configuration["Email:Host"];
                client.Port = int.Parse(_configuration["Email:Port"]);
                client.EnableSsl = false;

                using (var emailMessage = new MailMessage())
                {
                    emailMessage.To.Add(new MailAddress(_configuration["Email:Email"]));
                    emailMessage.From = new MailAddress(email);
                    emailMessage.Subject = subject;
                    emailMessage.Body = "Name: " + name + "; Phone: " + phone + "; Message: " + message;
                    client.Send(emailMessage);
                }
            }
            await Task.CompletedTask;
        }
    }
}