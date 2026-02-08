using BusinessManagementSystem.Enums;

namespace BusinessManagementSystem.Utility
{
    public static class SD
    {
        public const string Role_Superadmin = "superadmin";
        public const string Role_Admin = "admin";
        public const string Role_Employee = "employee";
        public const string Role_HR = "hr";

        public const string Gender_Male = "Male";
        public const string Gender_Female = "Female";
        public const string Status_Draft = "Draft";
        public const string Status_Submitted = "Submitted";
        public const string Status_Returned = "Returned";
        public const string Status_Resubmitted = "ReSubmitted";
        public const string Status_Approved = "Approved";
        public const string Status_Rejected = "Rejected";

        public static readonly Dictionary<string, string> Occupations = new Dictionary<string, string>
        {
            { Occupation.Occupation1.ToString(), "Occupation 1" },
            { Occupation.Occupation2.ToString(), "Occupation 2" },
            { Occupation.Occupation3.ToString(), "Occupation1 3" },
            { Occupation.Occupation4.ToString(), "Occupation 4" },
            { Occupation.Occupation5.ToString(), "Occupation1 5" },
            { Occupation.Occupation6.ToString(), "Occupation1 6" },
            { Occupation.Occupation7.ToString(), "Occupation1 7" }
        };


    }

    public static class DocumentType
    {
        public const string Reports = "Reports";
        public const string Bills = "Bills";

    }
}
