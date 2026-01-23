using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3.Polymorphism
{
    //Runtime polymorphism - Method Overriding
    //achive through method overriding using virtual and override keywords
    public abstract class PaymentProcessor
    {
        public abstract void ProcessPayment(decimal amount);
    }

    //Compiletime polymorphism - Method overloading
    //achive through method overloading using same method name with different parameters
    public class Employee
    {
        public virtual void CalculateBonus()
        {
            Console.WriteLine("Standard bonus 10%");
        }
    }

    public class CreditCardProcessor : PaymentProcessor
    {
        public override void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing credit card payment of {amount:C}");
        }
    }

    public class PayPalProcessor : PaymentProcessor
    {
        public override void ProcessPayment(decimal amount)
        {
            Console.WriteLine($"Processing PayPal payment of {amount:C}");
        }
    }

    public class Manager: Employee
    {
        public override void CalculateBonus()
        {
            Console.WriteLine("Manager bonus 20%");
        }
        public void CalculateBonus(decimal extraBonus)
        {
            Console.WriteLine($"Manager bonus 20% + extra bonus {extraBonus:C}");
        }

    }



    //simple example of method overloading -> compile time polimorphism
    public class Calculator
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
        public double Add(double a, double b)
        {
            return a + b;
        }
        public decimal Add(decimal a, decimal b)
        {
            return a + b;
        }
    }





}
