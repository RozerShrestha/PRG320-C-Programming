using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Interface
{
    public class EmailSender : IEmailSender
    {
        public void SendBulkEmail(List<Notification> notifications)
        {
            throw new NotImplementedException();
        }

        public void SendBulkEmailWithTemplate(List<Notification> notificationsTemplate)
        {
            throw new NotImplementedException();
        }

        public void SendEmail(string from, string to, string message, bool isHtml)
        {
            Console.WriteLine($"Email send from: {from}");
            Console.WriteLine($"Email send to: {to}");
            Console.WriteLine($"Message: {message}");
            Console.WriteLine($"Is Html: {isHtml}");
        }

        public void SendEmailWithTemplate(string from, string to, string messageTemplate, bool isHtml)
        {
            throw new NotImplementedException();
        }

        public void SendOTP()
        {
            throw new NotImplementedException();
        }
    }
}
