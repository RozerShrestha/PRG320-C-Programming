using Week3.Abstraction;
using Week3.Encapsulation;
using Week3.Interface;

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

        //var qrPayment1 = new QRPayment();
        //qrPayment1.ProcessPayment(4000);


        //var creditPayment1 = new CreditCardPayment();
        //creditPayment1.ProcessPayment(5000);



        //var qrpayment = new CheckOutService(new QRPayment());
        //qrpayment.CompletePayment(4000);

        //var creditCardPayment = new CheckOutService(new CreditCardPayment());
        //creditCardPayment.CompletePayment(5000);
        #endregion

        #region Interface
        //var notify1 = new NotificationService(new SMSSender());
        //notify1.Notify("Here is the message from SMS");

        //var notify2=new NotificationService(new EmailSender());
        //notify2.Notify("Here is the message from Email");

        ////For email
        //var emailReq = new Notification
        //{
        //    Channel = Channel.Email,
        //    To = "rozer.shrestha@gmail.com",
        //    Subject = "sample email subject",
        //    Message = "Here is sample message",
        //    IsHtml = false
        //};

        //notify2.Notify(emailReq);
        #endregion

    }
}
