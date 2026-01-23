using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3
{

    //we are creating Custom Exception by inheriting the base Exception class
    public class InsufficientFundsException : Exception
    {
        public decimal CurrentBalance { get; }
        public decimal WithdrawAmount { get; }

        public InsufficientFundsException(string message, decimal balance, decimal withdrawAmount)
        : base(message)
        {
            CurrentBalance = balance;
            WithdrawAmount = withdrawAmount;
        }


    }

}
}
