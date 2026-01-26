using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Week_4.FileSystem
{
    public class Student
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Department { get; set; }
        public double GPA { get; set; }
        public bool IsActive { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Year { get; set; }
        public string Nationality { get; set; }

        public EmergencyContact EmergencyContact { get; set; }
        public List<Course> Courses { get; set; }
        public List<Grade> Grades { get; set; }
        public List<Extracurricular> Extracurriculars { get; set; }
        public List<Scholarship> Scholarships { get; set; }
    }

    public class EmergencyContact
    {
        public string Name { get; set; }
        public string Relation { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class Course
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int Credits { get; set; }
        public string Instructor { get; set; }
    }

    public class Grade
    {
        public int CourseId { get; set; }
        public string LetterGrade { get; set; }
        public double Score { get; set; }
    }

    public class Extracurricular
    {
        public string Activity { get; set; }
        public string Role { get; set; }
        public List<string> Achievements { get; set; }
    }

    public class Scholarship
    {
        public string Name { get; set; }
        public double Amount { get; set; }
        public int YearAwarded { get; set; }
    }


}
