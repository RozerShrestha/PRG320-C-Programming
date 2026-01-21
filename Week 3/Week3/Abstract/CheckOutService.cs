using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Abstract
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
            //Option 1: Using 'is' pattern matching
            if (_processor is ICreditCardPaymentProcessor cardProcessor)
            {
                cardProcessor.ValidateCard();
                cardProcessor.ChargeCard();
            }
            if (_processor is IQRPaymentProcessor qrProcessor)
            {
                qrProcessor.QRDecrypt();
                qrProcessor.ProcessPayment(amount);
            }

            //Option 2: 
            _processor.ProcessPayment(amount);
        }
    }
}
