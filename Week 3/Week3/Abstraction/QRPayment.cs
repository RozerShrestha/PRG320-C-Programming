using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Abstraction
{
    public class QRPayment : IPaymentProcessor
    {
        public void ProcessPayment(decimal amount)
        {
            QRDecrypt();
            ValidateBalance();
            TransferFund();
        }

        public void QRDecrypt()
        {
            
        }

        public void TransferFund()
        {
            
        }

        public void ValidateBalance()
        {
            
        }
    }
}
