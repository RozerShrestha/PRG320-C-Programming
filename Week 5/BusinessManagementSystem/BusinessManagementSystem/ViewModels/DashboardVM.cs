using BusinessManagementSystem.Models;

namespace BusinessManagementSystem.ViewModels
{
    public class DashboardVM
    {
        #region Individual 
        public int Draft { get; set; }
        public string DraftLink { get; set; }
        public int Submitted { get; set; }
        public string SubmittedLink { get; set; }
        public int Acknowledged { get; set; }
        public string AcknowledgedLink { get; set; }
        public int ClaimProcessInInsurance { get; set; }
        public string ClaimProcessingInsuranceLink { get; set; }
        public int ClaimReimbursedAndApproved { get; set; }
        public string ClaimReimbursedAndApprovedLink { get; set; }
        public int ClaimReturned { get; set; }
        public string ClaimReturnedLink { get; set; }
        public int ClaimRejected { get; set; }
        public string ClaimRejectedLink { get; set; }
        #endregion

        #region All Employee
        public int Draft_All { get; set; }
        public string DraftLink_All { get; set; }
        public int Submitted_All { get; set; }
        public string SubmittedLink_All { get; set; }
        public int Acknowledged_All { get; set; }
        public string AcknowledgedLink_All { get; set; }
        public int ClaimProcessInInsurance_All { get; set; }
        public string ClaimProcessingInsuranceLink_All { get; set; }
        public int ClaimReimbursedAndApproved_All { get; set; }
        public string ClaimReimbursedAndApprovedLink_All { get; set; }
        public int ClaimReturned_All { get; set; }
        public string ClaimReturnedLink_All { get; set; }
        public int ClaimRejected_All { get; set; }
        public string ClaimRejectedLink_All { get; set; }
        #endregion
        public BasicConfiguration BasicConfiguration { get; set; }
    }
}
