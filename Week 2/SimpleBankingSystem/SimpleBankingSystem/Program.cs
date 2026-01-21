using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using SimpleBankingSystem;
using System;
using System;

class Program
{
    static double balance = 0.0;
    static string? correctPin = ""; // Hardcoded PIN for simplicity

    static void Main(string[] args)
    {
        // Login system with 3 attempts
        int attempts = 0;
        bool loggedIn = false;

        while (attempts < 3 && !loggedIn)
        {
            Console.Write("Enter PIN: ");
            string enteredPin = Console.ReadLine();

            using var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Bind AppSettings section to the AppSettings class
                services.Configure<AppSettings>(context.Configuration.GetSection("AppSettings"));
            })
            .Build();

            correctPin=  host.Services.GetRequiredService<IOptions<AppSettings>>().Value.Pin;







            if (enteredPin == correctPin)
            {
                loggedIn = true;
                Console.WriteLine("Login successful!");
            }
            else
            {
                attempts++;
                Console.WriteLine($"Incorrect PIN. Attempts remaining: {3 - attempts}");
            }
        }

        if (!loggedIn)
        {
            Console.WriteLine("Too many failed attempts. System locked.");
            return;
        }

        // Main banking loop
        bool exit = false;
        while (!exit)
        {
            Console.WriteLine("\nBanking Menu:");
            Console.WriteLine("1. Deposit");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Balance Inquiry");
            Console.WriteLine("4. Exit");
            Console.Write("Choose an option: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Deposit();
                    break;
                case "2":
                    Withdraw();
                    break;
                case "3":
                    BalanceInquiry();
                    break;
                case "4":
                    exit = true;
                    Console.WriteLine("Exiting the system. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select 1-4.");
                    break;
            }
        }
    }

    // Function to handle deposit
    static void Deposit()
    {
        Console.Write("Enter deposit amount: ");
        string input = Console.ReadLine();

        if (double.TryParse(input, out double amount))
        {
            if (amount > 0)
            {
                balance += amount;
                Console.WriteLine($"Successfully deposited ${amount:F2}.");
            }
            else
            {
                Console.WriteLine("Deposit amount must be positive.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    // Function to handle withdrawal
    static void Withdraw()
    {
        Console.Write("Enter withdrawal amount: ");
        string input = Console.ReadLine();

        if (double.TryParse(input, out double amount))
        {
            if (amount > 0)
            {
                if (amount <= balance)
                {
                    balance -= amount;
                    Console.WriteLine($"Successfully withdrew ${amount:F2}.");
                }
                else
                {
                    Console.WriteLine("Insufficient funds. Withdrawal denied.");
                }
            }
            else
            {
                Console.WriteLine("Withdrawal amount must be positive.");
            }
        }
        else
        {
            Console.WriteLine("Invalid input. Please enter a valid number.");
        }
    }

    // Function to handle balance inquiry
    static void BalanceInquiry()
    {
        Console.WriteLine($"Your current balance is: ${balance:F2}");
    }
}