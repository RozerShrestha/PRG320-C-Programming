using BusinessManagementSystem.Dto;

namespace BusinessManagementSystem.Services
{
    public interface IEmailSender
    {
       Task SendEmailAsync(string email, string subject, string htmlMessage);
        string PrepareEmail(string message, string fullName, string patientName, string type = "New", int id = 0, string status = null, int claimAmount = 0, int paidAmount = 0, string remark = null);
        string PrepareEmailFamily(string message, string fullName, string iNumber, int id = 0);
        string PrepareEmailInsurancePlan(string message, string fullName, string oldPlan, string newPlan, string iNumber, int id = 0);
        string PrepareEmailHrApproved(string message, string fullName);


    }
}
