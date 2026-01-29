using Microsoft.AspNetCore.Routing.Internal;
using System.Runtime.ExceptionServices;

namespace Week4EntityFramework.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public int GradeId { get; set; }
        public Grade Grade { get; set; }
    }
}
