using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebMandel.Infrastructure
{
    public interface IMailService
    {
        Task SendMailAsync(string fromName, string fromAddress,
                           string toName, string toAddress,
                           string subject,
                           string message);
    }
}
