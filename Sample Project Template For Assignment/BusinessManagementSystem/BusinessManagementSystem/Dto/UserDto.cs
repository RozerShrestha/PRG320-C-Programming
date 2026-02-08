using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessManagementSystem.Dto
{
    public class UserDto
    {
        //[ValidateNever]
        public int UserId { get; set; }
        [Required]
        [DisplayName("User Name")]
        public string UserName { get; set; }
        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Display(Name = "Confirm Password")]
        [Required]
        public string ConfirmPassword { get; set; }


        [Required]
        [DisplayName("Full Name")]
        public string FullName { get; set; }
        [Required]
        public string Address { get; set; }
        [Display(Name = "Date Of Birth")]
        [Required]
        public DateOnly DateOfBirth { get; set; }
        [Display(Name = "Mobile Number")]
        [RegularExpression("^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Occupation { get; set; }
        [Required]
        public bool Status { get; set; }
        [Required]
        public int  RoleId { get; set; }
        [ValidateNever]
        [DisplayName("Role Name")]
        public string RoleName { get; set; }

        [DisplayName("Facebook Link")]
        public string? FacebookLink { get; set; }
        [DisplayName("Instagram Link")]
        public string? InstagramLink { get; set; }
        [DisplayName("Tiktok Link")]
        public string? TiktokLink { get; set; }
        [DisplayName("Profile Picture Link")]
        public string? ProfilePictureLink { get; set; }
        public string? Skills { get; set; }
        public string? Notes { get; set; }

    }
}
