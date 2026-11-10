using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using TARge25_Shop.Core.Dto;
using TARge25_Shop.Core.ServiceInterface;

namespace TARge25_Shop.ApplicationServices.Services
{
    public class EmailServices : IEmailServices
    {
        //4. konstruktor
        private readonly IConfiguration _config;
                
        public EmailServices
            (
                IConfiguration config 
            )
        {
            _config = config;
        }

        //1. meetod + packet MailKit alla laadida
        public void SendEmail(EmailDto dto)
        {
            var email = new MimeMessage();
            //2.  Set the sender and recipient addresses
            // 4. Parse( ..sisu..)
            email.From.Add(MailboxAddress.Parse(_config.GetSection("EmailUserName").Value));
            email.To.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;

            // 3.
            var builder = new BodyBuilder
            {
                HtmlBody = dto.Body
            };

            //failide lisamine
            foreach (var file in dto.Attachments)
            {
                if (file.Length > 0 && file.Length < 1048560 )
                {
                    using (var stream = new MemoryStream())
                    {
                        file.CopyTo( stream );
                        stream.Position = 0; // Reset stream position algusesse
                        builder.Attachments.Add(file.FileName, stream.ToArray());
                    }                    
                }
            }
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();

            smtp.Connect(_config.GetSection("EmailHost").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            smtp.Authenticate(_config.GetSection("EmailUserName").Value, _config.GetSection("EmailPassword").Value);
            smtp.Send(email);
            smtp.Disconnect(true);
        }
    }
}
