using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Week3.Abstraction;

namespace Week3.Abstract
{
    public abstract class PaymentProcessor : IPaymentProcessor
    {
        public virtual void ProcessPayment(decimal amount)
        {
            ValidateBalance();
            TransferFund();
        }

        public  void ValidateBalance()
        {
            // common logic for validateBalance
        }

        public  void TransferFund()
        {// common logic for TransferFund

        }
    }
}
