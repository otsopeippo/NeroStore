using Microsoft.Extensions.Configuration;
using NeroStore.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace NeroStore
{
    public class Lasku
    {
        private IConfiguration _configuration;
        private NeroStoreDBContext _context;

        public Lasku(IConfiguration configuration, NeroStoreDBContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public bool LähetäLasku(List<Tuote> tuotteet, string sähköposti)
        {
            try
            {
                var emailOsoite = _configuration.GetConnectionString("Email");
                var emailSalasana = _configuration.GetConnectionString("EmailPassword");

                MailMessage mailMessage = new MailMessage(emailOsoite, "riku.soikkeli@hotmail.fi");
                mailMessage.Subject = "NeroStore tilauksesi";

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                smtpClient.Credentials = new System.Net.NetworkCredential()
                {
                    UserName = emailOsoite,
                    Password = emailSalasana
                };
                smtpClient.EnableSsl = true;

                var lasku = LuoLasku(tuotteet, sähköposti);
                //File.WriteAllBytes("testilasku.pdf", testilasku); // tää on ihan paska ja ruma

                mailMessage.BodyEncoding = System.Text.Encoding.UTF8;
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = lasku;
                smtpClient.Send(mailMessage);
            }
            catch
            {
                return false;
            }
            return true;
        }

        public string LuoLasku(List<Tuote> tuotteet, string sähköposti)
        {
            var a = new Apumetodit(_context);
            //tuotteet = a.HaeTuotteet();

            var lasku = new StringBuilder();
            var kokonaishinta = tuotteet.Sum(tuote => tuote.Hinta);

            lasku.AppendLine(@"<!DOCTYPE html>
                                <html lang=""en"">
                                <head>
                                    <meta charset=""UTF-8"">
                                    <meta http-equiv=""X-UA-Compatible"" content=""IE=edge"">
                                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                                    <title> Document </title>
                                    <link rel=""stylesheet"" href=""https://maxcdn.bootstrapcdn.com/bootstrap/4.0.0/css/bootstrap.min.css"" integrity=""sha384-Gn5384xqQ1aoWXA+058RXPxPg6fy4IWvTNh0E263XmFcJlSAwiGgFAW/dAiS6JXm"" crossorigin=""anonymous"">
                                </head>
                                <body style=""max-width: 50%"">");

            lasku.AppendLine(@$"<b>Pvm:</b> {DateTime.Now.Date.ToShortDateString()}</br>
                                <b>Ostaja:</b> {sähköposti}</br></br>");

            lasku.AppendLine(@"<table class=""table table-bordered"">
                          <thead>
                            <tr>
                              <th scope=""col"">#</th>
                              <th scope=""col"">Tuotenimi</th> 
                               <th scope=""col"">Lukumäärä</th>
                                <th scope=""col"">Hinta</th>
                               </tr>
                             </thead>
                             <tbody>");

            for (int i = 0; i < tuotteet.Count; i++)
            {
                lasku.AppendLine(@$"<tr>
                                  <th scope=""row"">{i+1}</th>
                                  <td>{tuotteet[i].Nimi}</td>
                                  <td>1</td>
                                  <td>{tuotteet[i].Hinta}</td>
                                </tr> ");
            }

            StringBuilder stringBuilder = lasku.AppendLine(@$"<tr>
                              <th scope=""row"">{tuotteet.Count+1}</th>
                              <td colspan=""2"">Yht:</td>
                              <td >{kokonaishinta}</td>
                            </tr>
                          </tbody>
                        </table>
                    </br>
                    <p style=""text-align: right"">Kiitos, kun asioit NeroStore :ssa!</p>
                    </body>
                    </html>");

            //File.WriteAllTextAsync("test.html", lasku.ToString());
            return lasku.ToString();
        }
    }
}
