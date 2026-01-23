using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Abstraction
{
    public class QRPayment : IQRPaymentProcessor
    {
        public void ProcessPayment(decimal amount)
        {
            //option 2
            QRDecrypt();
            ValidateBalance();
            TransferFund();
        }

        public void QRDecrypt()
        {
            throw new NotImplementedException();
        }

        public void TransferFund()
        {
            throw new NotImplementedException();
        }

        public void ValidateBalance()
        {
            throw new NotImplementedException();
        }
    }
}
