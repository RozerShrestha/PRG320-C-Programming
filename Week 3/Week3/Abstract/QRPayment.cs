using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Abstract
{
    public class QRPayment : PaymentProcessor, IPaymentProcessor
    {

        public override void ProcessPayment(decimal amount)
        {
            ValidateBalance();
            base.ProcessPayment(amount);
        }

        public void QRDecrypt()
        {

        }

    }
}
