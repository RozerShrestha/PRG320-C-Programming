using System;
using System.Collections.Generic;

namespace BankingSystem
{
    public class BankAccount
    {
        public string AccountNumber { get; }
        public string Owner { get; }
        public decimal Balance { get; private set; }
        public List<string> Transactions { get; }

        public BankAccount(string accountNumber, string owner, decimal initialBalance = 0)
        {
            if (string.IsNullOrWhiteSpace(accountNumber))
                throw new ArgumentException("Account number cannot be empty.");
            if (string.IsNullOrWhiteSpace(owner))
                throw new ArgumentException("Owner cannot be empty.");
            if (initialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative.");

            AccountNumber = accountNumber;
            Owner = owner;
            Balance = initialBalance;
            Transactions = new List<string>
            {
                $"Account created for {Owner} with balance {Balance}"
            };
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Deposit amount must be positive.");
            Balance += amount;
            Transactions.Add($"Deposited {amount}, new balance {Balance}");
        }

        public void Withdraw(decimal amount)
        {
            if (amount <= 0)
                throw new ArgumentException("Withdraw amount must be positive.");
            if (amount > Balance)
                throw new InvalidOperationException("Insufficient funds.");
            Balance -= amount;
            Transactions.Add($"Withdrew {amount}, new balance {Balance}");
        }

        public void TransferTo(BankAccount target, decimal amount)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));
            Withdraw(amount);
            target.Deposit(amount);
            Transactions.Add($"Transferred {amount} to {target.AccountNumber}");
        }
    }
}