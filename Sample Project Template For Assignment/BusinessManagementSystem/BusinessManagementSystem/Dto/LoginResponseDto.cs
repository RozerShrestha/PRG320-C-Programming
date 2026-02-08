using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BusinessManagementSystem.Dto
{
    public class LoginResponseDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public string RoleDescription { get; set; }
        public DateTime TokenExpiry { get; set; }
        public string Token { get; set; }
        public string Message { get; set; }
    }
}
