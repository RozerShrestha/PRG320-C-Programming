using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.Json.Serialization;

namespace BusinessManagementSystem.Dto
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Username is required")]
        [DataType(DataType.Text)]
        [JsonIgnore]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [JsonIgnore]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required")]
        [DataType(DataType.Password)]
        [JsonIgnore]
        public string ConfirmPassword { get; set; }
    }
}
