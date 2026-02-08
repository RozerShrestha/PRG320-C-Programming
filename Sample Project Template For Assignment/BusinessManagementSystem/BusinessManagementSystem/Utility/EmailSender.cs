using BusinessManagementSystem.Controllers;
using BusinessManagementSystem.Dto;
using BusinessManagementSystem.Services;
using BusinessManagementSystem.Utility;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;
using System.Text;
using System.Text.RegularExpressions;

namespace BusinessManagementSystem.Utility
{
    public class EmailSender : IEmailSender
    {
        protected readonly ILogger<EmailSender> _logger;
        private static string emailAlias = "";
        private static string emailAddress = "";
        private static string password = "";
        private static string hostName = "";
        private static int port=0;
        public EmailSender(ILogger<EmailSender> logger)
        {
            _logger= logger;
            //GetEmailDetail();

        }
        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            new Task(() =>
            {
                var emailToSend = new MimeMessage();
                emailToSend.From.Add(new MailboxAddress(emailAlias, emailAddress));
                emailToSend.To.Add(MailboxAddress.Parse(email));
                emailToSend.Subject = subject;
                emailToSend.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

                //send email
                using (var emailClient = new SmtpClient())
                {
                    emailClient.Connect(hostName, port, MailKit.Security.SecureSocketOptions.None);
                    emailClient.Send(emailToSend);
                    emailClient.Disconnect(true);
                }
            }).Start();
            _logger.LogInformation($"## {this.GetType().Name} Email Send to {email} Message: {htmlMessage}");
            return Task.CompletedTask;
        }
        public string PrepareEmail(string message, string fullName, string patientName, string type="New", int id=0, string status=null, int claimAmount=0, int paidAmount=0, string remark=null)
        {
            StringBuilder sb = new StringBuilder(message);
            sb.Replace("{{fullName}}", fullName);
            sb.Replace("{{patientName}}", patientName);
            if (type=="Update")
            {
                sb.Replace("{{id}}", id.ToString());
                sb.Replace("{{status}}", status);
                sb.Replace("{{claimAmount}}", claimAmount.ToString());
                sb.Replace("{{paidAmount}}", paidAmount==0?"N/A":paidAmount.ToString());
                sb.Replace("{{remark}}", remark);
            }
            return sb.ToString();
        }
        public string PrepareEmailFamily(string message, string fullName, string iNumber, int id = 0)
        {
            StringBuilder sb = new StringBuilder(message);
            sb.Replace("{{fullName}}", fullName);
            sb.Replace("{{inumber}}", iNumber);
            sb.Replace("{{id}}", id.ToString());
            
            return sb.ToString();
        }
        public string PrepareEmailHrApproved(string message, string fullName)
        {
            StringBuilder sb = new StringBuilder(message);
            sb.Replace("{{fullName}}", fullName);

            return sb.ToString();
        }
        public string PrepareEmailInsurancePlan(string message, string fullName, string oldPlan, string newPlan, string iNumber, int id = 0)
        {
            StringBuilder sb = new StringBuilder(message);
            sb.Replace("{{fullName}}", fullName);
            sb.Replace("{{inumber}}", iNumber);
            sb.Replace("{{oldplan}}", oldPlan);
            sb.Replace("{{newplan}}", newPlan);
            sb.Replace("{{id}}", id.ToString());

            return sb.ToString();
        }
        //private void GetEmailDetail()
        //{
        //    var basicInfo = _unitOfWork.BasicConfiguration.GetSingleOrDefault().Data;
        //    emailAlias = basicInfo.EmailAlias;
        //    emailAddress = basicInfo.Email;
        //    password= basicInfo.Password;
        //    hostName = basicInfo.HostName;
        //    port = basicInfo.Port;

        //}
        
    }
}
