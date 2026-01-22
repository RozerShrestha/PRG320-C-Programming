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

        //Example of Polymorphism - Method Overloading
        public void Notify(string message)
        {
            Console.WriteLine(message);
        }

        //Example of Polymorphism - Method Overloading
        public void Notify(Notification notification)
        {
            switch (notification.Channel)
            {
                case Channel.Email:
                    _emailSender.SendEmail(from: notification.FromEmail, 
                                            to: notification.To, 
                                            message: notification.Message, 
                                            isHtml: notification.IsHtml);
                    _emailSender.SendEmailWithTemplate(from: notification.FromEmail,
                                            to: notification.To,
                                            messageTemplate: notification.Message,
                                            isHtml: notification.IsHtml);
                                                
                    break;
                
                case Channel.Sms:
                    _smsSender.SendSMS();
                    _smsSender.SendSMSWithTemplate();
                    break;

                default:
                    throw new NotSupportedException($"Channel '{notification.Channel}' is not supported.");

            }
        }

        public void Notify(Channel channel, List<Notification> notifications)
        {
            switch (channel)
            {
                case Channel.Email:
                    _emailSender.SendBulkEmail(notifications);
                    _emailSender.SendBulkEmailWithTemplate(notifications);

                    break;

                case Channel.Sms:
                    _smsSender.SendSMS();
                    _smsSender.SendSMSWithTemplate();
                    break;

                default:
                    throw new NotSupportedException($"Channel '{channel}' is not supported.");

            }
        }
    }
}
