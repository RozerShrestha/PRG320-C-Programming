using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Abstraction
{
    public class CreditCardPayment : IPaymentProcessor
    {
        

        public void ProcessPayment(decimal amount)
        {
            ValidateCard();
            ValidateBalance();
            ChargeCard();
            TransferFund();
        }

        public void TransferFund()
        {
           
        }

        public void ValidateBalance()
        {
           
        }

        public void ValidateCard()
        {
           
        }

        public void ChargeCard()
        {
            
        }
    }
}
