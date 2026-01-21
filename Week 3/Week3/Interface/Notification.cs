using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Interface
{
    public class Notification
    {

        public Channel Channel { get; init; }

        // Common fields
        public string To { get; set; }
        public string Message { get; set; }

        // Email-specific
        public string? Subject { get; set; }
        public bool IsHtml { get; set; }
        public string? FromEmail { get; set; }

        // SMS-specific
        public string? FromPhone { get; set; }

    }
}
