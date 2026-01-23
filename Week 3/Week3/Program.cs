using System.Net.Http.Headers;
using Week3.Abstraction;
using Week3.AbstractSimple;
using Week3.Encapsulation;
using Week3.ExceptionHandling;
using Week3.Interface;
using Week3.Polymorphism;

class Program
{
    static void Main()
    {

        #region Encapsulation
        //BankAccount account = new BankAccount();

        //account.Deposit(3000);
        //account.Deposit(8000);
        //account.Withdraw(2000);
        //account.Withdraw(3000);
        //// account.Withdraw(4000); ❌ Exceeds daily limit
        //// account.Withdraw(5000); ❌ Insufficient balance

        //Console.WriteLine(account.Balance); // 3000

        #endregion

        #region Abstraction




        ////option 1
        //var qrpayment = new CheckOutService(new QRPayment());
        //qrpayment.CompletePayment(4000);

        //var creditCardPayment = new CheckOutService(new CreditCardPayment());
        //creditCardPayment.CompletePayment(5000);


        ////option 2
        //var qrPayment1 = new QRPayment();
        //qrPayment1.ProcessPayment(4000);

        //var creditPayment1 = new CreditCardPayment();
        //creditPayment1.ProcessPayment(5000);




        #endregion

        #region Interface
        //var emailSender = new EmailSender();
        //var smsSender = new SMSSender();


        ////using NotificationService to send email and sms is not necessary now.
        //var notify1 = new NotificationService(emailSender, smsSender);

        ////For email
        //var emailReq = new Notification
        //{
        //    Channel = Channel.Email,
        //    FromEmail = "from@gmail.com",
        //    To = "to@gmail.com",
        //    Subject = "sample email subject",
        //    Message = "Here is sample message",
        //    IsHtml = false
        //};
        //notify1.Notify(emailReq);

        //var emailReqList = new List<Notification>
        //{
        //    new Notification { Channel = Channel.Email, FromEmail = "from@gmail.com", To = "to1@gmail.com", Subject = "sample email subject", Message = "Here is sample message", IsHtml = false },
        //    new Notification { Channel = Channel.Email, FromEmail = "from@gmail.com", To = "to2@gmail.com", Subject = "sample email subject", Message = "Here is sample message", IsHtml = false },
        //    new Notification { Channel = Channel.Email, FromEmail = "from@gmail.com", To = "to3@gmail.com", Subject = "sample email subject", Message = "Here is sample message", IsHtml = false },
        //};
        //notify1.Notify(Channel.Email, emailReqList);

        ////For SMS
        //var smsReq = new Notification
        //{
        //    Channel = Channel.Sms,
        //    FromPhone = "1234567890",
        //    To = "0987654321",
        //    Message = "Here is sample SMS message",
        //    IsHtml = false
        //};
        //notify1.Notify(smsReq);
        #endregion

        #region Abrstract Class  

        //Vehicle myCar = new Car();
        //myCar.Drive();   // Output: Car is driving on the road.
        //myCar.FuelUp("car");  // Output: Car is fueled up.

        //Vehicle myBike = new Bike();
        //myBike.Drive();  // Output: Bike is riding on the street.
        //myBike.FuelUp("Bike"); // Output: Bike is fueled up.

        #endregion

        #region Polymorphism
        ////these are example of compile time polymorphism (method overloading)
        //Calculator calculator =new Calculator();
        //calculator.Add(1, 2);
        //calculator.Add(2, 3);
        //calculator.Add(1.1, 2.2);

        ////these are example of runtime polymorphism (method overriding)
        //CreditCardProcessor creditCardProcessor = new CreditCardProcessor();
        //creditCardProcessor.ProcessPayment(5000);
        //PayPalProcessor payPalProcessor=new PayPalProcessor();
        //payPalProcessor.ProcessPayment(7000);

        #endregion

        #region Exception handling
        //setting balance to 500 for testing
        BankAcc account = new BankAcc(500);

        try
        {
            Console.WriteLine("Attempting to withdraw 800...");
            account.Withdraw(800);
        }
        //since 800 > 500, it will throw InsufficientFundsException
        catch (InsufficientFundsException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Current Balance: {ex.CurrentBalance:C}");
            Console.WriteLine($"Requested Withdrawal: {ex.WithdrawAmount:C}");
        }
        finally
        {
            Console.WriteLine("Transaction attempt completed.");
        }



        try
        {
            account.Deposit(-1);
        }
        catch (InsufficientFundsException ex)
        {

            Console.WriteLine($"Error: {ex.Message}");
        }
        #endregion
    }
}
