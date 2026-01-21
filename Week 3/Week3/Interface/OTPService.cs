using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Interface
{
    internal class OTPService
    {
        private readonly ISender _sender;

        public OTPService(ISender sender)
        {
            _sender = sender;
        }

        public void SendOTP_SMS(string phone)
        {
            //logic to send otp through sms
        }

        public void SendOTP_Email(string email)
        {
            //logic to send otp through email
        }
    }
}
