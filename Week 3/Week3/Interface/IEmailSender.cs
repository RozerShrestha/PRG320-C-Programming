using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Interface
{
    public interface IEmailSender:ISender
    {
        void SendEmail(string from, string to, string message, bool isHtml);
        void SendBulkEmail(List<Notification> notifications);
        void SendEmailWithTemplate(string from, string to, string messageTemplate, bool isHtml);
        void SendBulkEmailWithTemplate(List<Notification> notificationsTemplate);
    }
}
