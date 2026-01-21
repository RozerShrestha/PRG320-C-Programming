using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleBankingSystem
{
    public class BankingModule
    {
        public double _balance = 0.0;
        public string _correctPin = "1234";

        public void _Deposit()
        {
            Console.Write("Enter deposit amount: ");
            string input = Console.ReadLine();
            if (double.TryParse(input, out double amount) && amount > 0)
            {
                _balance += amount;
                Console.WriteLine($"Deposited: ${amount:F2}. New Balance: ${_balance:F2}");
            }
            else
            {
                Console.WriteLine("Invalid amount. Please enter a positive number.");
            }
        }

        public void _Withdraw()
        {
            Console.Write("Enter withdrawal amount: ");
            string input = Console.ReadLine();
            if (double.TryParse(input, out double amount) && amount > 0)
            {
                if (amount <= _balance)
                {
                    _balance -= amount;
                    Console.WriteLine($"Withdrew: ${amount:F2}. New Balance: ${_balance:F2}");
                }
                else
                {
                    Console.WriteLine("Insufficient funds.");
                }
            }
            else
            {
                Console.WriteLine("Invalid amount. Please enter a positive number.");
            }
        }

        public void _BalanceInquiry()
        {
            Console.WriteLine($"Current Balance: ${_balance:F2}");
        }
    }
}
