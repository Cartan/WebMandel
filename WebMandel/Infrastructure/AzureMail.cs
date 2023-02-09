using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Net;
using System.Net.Mail;
using SendGrid;
using System.Threading.Tasks;

namespace WebMandel.Infrastructure
{
    public class AzureMail : IMailService
    {
        string _userName;
        string _password;

        public AzureMail()
        {
            _userName = WebConfigurationManager.AppSettings["mailUser"];
            _password = WebConfigurationManager.AppSettings["mailPassword"];
        }

        public Task SendMailAsync(string fromName, string fromAddress, string toName, string toAddress,
                                  string subject, string message)
        {
            var msg = new SendGridMessage();
            msg.From = new MailAddress(fromAddress, fromName);
            var to = new MailAddress(toAddress, toName);
            msg.AddTo(to.ToString());
            msg.Subject = string.IsNullOrWhiteSpace(subject) ? "[WebMandel]"
                : "[WebMandel] " + subject.Trim();
            msg.Text = message;

            var creds = new NetworkCredential(_userName, _password);
            var trans = new Web(creds);
            return trans.DeliverAsync(msg);
        }
    }
}