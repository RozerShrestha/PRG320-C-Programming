using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.ExceptionHandling
{
    public class BankAcc
    {
        //property with private setter
        public decimal Balance { get; private set; }

        //public method to set Balance since its setter is private
        public BankAcc(decimal initialBalance)
        {
            Balance = initialBalance;
        }

        // Method to withdraw money
        public void Withdraw(decimal amount)
        {
            if (amount > Balance)
            {
                // Throw custom exception
                throw new InsufficientFundsException(
                    "Withdrawal amount exceeds current balance.",
                    Balance,
                    amount
                );
            }

            Balance -= amount;
            Console.WriteLine($"Withdrawal successful! Remaining balance: {Balance:C}");
        }
        public void Deposit(decimal amount)
        {
            if (amount < 0)
            {
                throw new InsufficientFundsException("Can't deposit less than 0");
            }
        }
    }
}
