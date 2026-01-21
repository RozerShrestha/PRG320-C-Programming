using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Abstraction
{
    public class CheckOutService
    {
        private readonly IPaymentProcessor _processor;
        public CheckOutService(IPaymentProcessor processor)
        {
            _processor = processor;
        }
        public void CompletePayment(decimal amount)
        {
            _processor.ProcessPayment(amount);
        }
    }
}
