using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Abstraction
{
    public class CreditCardPayment : ICreditCardPaymentProcessor
    {
        public void ChargeCard()
        {
            throw new NotImplementedException();
        }

        public void ProcessPayment(decimal amount)
        {
            //option 2
            ValidateCard();
            ValidateBalance();
            ChargeCard();
            TransferFund();
        }
        
        public void TransferFund()
        {
            throw new NotImplementedException();
        }

        public void ValidateBalance()
        {
            throw new NotImplementedException();
        }

        public void ValidateCard()
        {
            throw new NotImplementedException();
        }
    }
}
