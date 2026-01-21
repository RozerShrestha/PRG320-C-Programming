using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Interface
{
    public class NotificationService
    {
        private readonly IEmailSender _emailSender;
        private readonly ISMSSender _smsSender;

        public NotificationService(IEmailSender emailSender, ISMSSender smsSender)
        {
            _emailSender = emailSender;
            _smsSender = smsSender;
        }

        public void Notify(string message)
        {
            Console.WriteLine(message);
        }

        public void Notify(Notification notification)
        {
            switch (notification.Channel)
            {
                case Channel.Email:
                    _emailSender.SendEmail(from: notification.FromEmail, 
                                            to: notification.To, 
                                            message: notification.Message, 
                                            isHtml: notification.IsHtml);
                break;
                
                case Channel.Sms:

                break;

                default:
                    throw new NotSupportedException($"Channel '{notification.Channel}' is not supported.");

            }
        }
    }
}
