using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Interface
{
    internal class NotificationService
    {
        private readonly ISender _sender;

        public NotificationService(ISender sender)
        {
            _sender = sender;
        }

        public void Notify(string message)
        {
            // here goes the message;
        }

        public void Notify(Notification notification)
        {
            switch (notification.Channel)
            {
                case Channel.Email:

                break;
                
                case Channel.Sms:

                break;

                default:
                    throw new NotSupportedException($"Channel '{notification.Channel}' is not supported.");

            }
        }
    }
}
