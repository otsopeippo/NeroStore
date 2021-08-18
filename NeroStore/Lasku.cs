using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;

namespace NeroStore
{
    public class Lasku
    {
        private IConfiguration _configuration;

        public Lasku(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void LähetäLasku()
        {
            var emailOsoite = _configuration.GetConnectionString("Email");
            var emailSalasana = _configuration.GetConnectionString("EmailPassword");

            MailMessage mailMessage = new MailMessage(emailOsoite, "riku.soikkeli@hotmail.fi");
            mailMessage.Subject = "testi subject";
            mailMessage.Body = "testi body";

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = emailOsoite,
                Password = emailSalasana
            };

            smtpClient.EnableSsl = true;
            smtpClient.Send(mailMessage);

        }

        public void LuoLasku()
        {

        }
    }
}
