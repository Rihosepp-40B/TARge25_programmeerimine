using Microsoft.Extensions.Configuration;
using MimeKit;
using TARge25_Shop.Core.Dto;

namespace TARge25_Shop.ApplicationServices.Services
{
    public class EmailServices
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
            email.From.Add(MailboxAddress.Parse(dto.To));
            email.Subject = dto.Subject;

            // 3.
            var builder = new BodyBuilder
            {
                HtmlBody = dto.Body
            };
        }
    }
}
