using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Interface
{
    internal interface ISender
    {
        void SendOTP();
        void SendEmail();
        void SendSMS();

        void SendBulkEmail();
        void SendBulkSMS();

        void SendEmailWithTemplate();
        void SendSMSWithTemplate();

        void SendBulkEmailWithTemplate();
        void SendBulkSMSWithTemplate();

    }
}
