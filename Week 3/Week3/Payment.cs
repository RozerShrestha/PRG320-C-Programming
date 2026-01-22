using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week3
{

    public interface IPayment
    {
        void ProcessPayment(double amount, string paymentType);
        void PaymentLogging(string message);
    }


    public abstract class Payment
    {
        public int PaymentId { get; set; }
        public double Amount { get; set; }

        public abstract void ProcessPayment(double amount, string paymentType);

        public virtual void PaymentLogging(string message)
        {
            Console.WriteLine($"Log: {message}");
        }

    }

    public class KhaltiPayment : IPayment
    {
        public void PaymentLogging(string message)
        {
            throw new NotImplementedException();
        }
       
        public void ProcessPayment(double amount, string paymentType)
        {
            throw new NotImplementedException();
        }
    }

    public class EsewaPayment : Payment
    {
        public override void ProcessPayment(double amount, string paymentType)
        {
            Console.WriteLine($"Process payment according to Esewa");
        }
    }

    public class BankPayment : Payment
    {
        public override void ProcessPayment(double amount, string paymentType)
        {
            Console.WriteLine($"process payment according to Bank");
        }

        public override void PaymentLogging(string message)
        {
            if(message.Contains("Error"))
            {
                Console.WriteLine($"Error: {message}");
                return;
            }
            else if(message.Contains("Warning"))
                Console.WriteLine($"Warning: {message}");

        }
    }

}
