using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace BlissApp.Controllers
{
    [Route("api/[controller]")]
    public class ShareController : Controller
    {

        [HttpPost]
        public void Post(string dest, string content)
        {
            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Teste Email", "ad2w1et2h@hotmail.com"));
            message.To.Add(new MailboxAddress("Simon Santos", dest));
            message.Subject = "Teste";
            message.Body = new TextPart("plain")
            {
                Text = content
        };
            using (var client = new SmtpClient())
            {
                // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                client.Connect("smtp.live.com", 587, false);

                // Note: since we don't have an OAuth2 token, disable
                // the XOAUTH2 authentication mechanism.
                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication
                client.Authenticate("ad2w1et2h@hotmail.com", "Mailkit20");

                client.Send(message);
                client.Disconnect(true);
            }

        }
    }
}
