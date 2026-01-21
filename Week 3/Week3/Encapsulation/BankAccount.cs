using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Encapsulation
{
    //Encapsulation

    public class BankAccount
    {
        // RULE 1: Private fields (data hiding)
        private decimal _balance;
        private decimal _dailyWithdrawnAmount;
        // Bank policy: daily withdrawal limit
        private const decimal DailyWithdrawalLimit = 5000;

        public decimal amount { get; private set; }

        private decimal _amount;

        public decimal Amount
        {
            get { return _amount; }
            //set
            //{
            //    if (value < 0)
            //        throw new ArgumentOutOfRangeException(nameof(value), "Amount cannot be negative");
            //    _amount = Math.Round(value, 2, MidpointRounding.AwayFromZero);
            //}

        }

       

        // RULE 2: Public read-only access to balance
        public decimal Balance
        {
            get { return _balance; }
        }

        // RULE 3 & 4: Controlled modification with validation
        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive.");

            _balance += amount;
        }

        // RULE 3, 4 & 5: Enforce bank rules and protect object state
        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdrawal amount must be positive.");

            // Rule: No negative balance
            if (amount > _balance)
                throw new InvalidOperationException("Insufficient balance.");

            // Rule: Daily withdrawal limit
            if (_dailyWithdrawnAmount + amount > DailyWithdrawalLimit)
                throw new InvalidOperationException("Daily withdrawal limit exceeded.");

            _balance -= amount;
            _dailyWithdrawnAmount += amount;
        }
    }

}
