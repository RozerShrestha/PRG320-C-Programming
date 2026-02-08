using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BusinessManagementSystem.Helper
{
    public class CustomUserRequireClaim : IAuthorizationRequirement
    {
        public string ClaimType { get; }
        public CustomUserRequireClaim(string claimType)
        {
            ClaimType = claimType;
        }
    }
}
