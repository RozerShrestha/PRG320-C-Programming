using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Interface
{
    public interface ISMSSender: ISender
    {
        void SendSMS();
        void SendBulkSMS();
        void SendSMSWithTemplate();
        void SendBulkSMSWithTemplate();
    }
}
