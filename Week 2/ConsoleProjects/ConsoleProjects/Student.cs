public class Student
{
    public string Name { get; set; }
    public List<double> Grades { get; set; } = new List<double>();
    static List<Student> students = new List<Student>();
    public Student()
    {
        
    }
    public Student(string name)
    {
        Name = name;
    }

    public double CalculateAverage()
    {
        if (Grades.Count == 0) return 0.0;
        double sum = 0;
        foreach (double grade in Grades)
        {
            sum += grade;
        }
        double avg = sum / Grades.Count;
        return avg;
    }
    // Function to add a new student
    public static void AddStudent()
    {
        Console.Write("Enter student name: ");
        string name = Console.ReadLine();

        if (!string.IsNullOrWhiteSpace(name))
        {
            students.Add(new Student(name));

            Console.WriteLine($"Student '{name}' added successfully.");
        }
        else
        {
            Console.WriteLine("Invalid name. Please enter a valid student name.");
        }
    }

    // Function to view all students
    public static void ViewAllStudents()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("No students in the system.");
            return;
        }

        Console.WriteLine("\nList of Students:");
        for (int i = 0; i < students.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {students[i].Name} (Grades: {string.Join(", ", students[i].Grades)})");
        }
    }

    // Function to add a grade to a student
    public static void AddGradeToStudent()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("No students available. Add a student first.");
            return;
        }

        ViewAllStudents(); // Show list to choose from
        Console.Write("Enter the student number to add grade: ");
        string input = Console.ReadLine();

        if (int.TryParse(input, out int index) && index >= 1 && index <= students.Count)
        {
            double grade = GetValidGrade("Enter grade (0-100): ");
            students[index - 1].Grades.Add(grade);
            Console.WriteLine($"Grade {grade} added to {students[index - 1].Name}.");
        }
        else
        {
            Console.WriteLine("Invalid student number.");
        }
    }

    // Function to calculate average grade for a student
    public static void CalculateAverageForStudent()
    {
        if (students.Count == 0)
        {
            Console.WriteLine("No students available. Add a student first.");
            return;
        }

        ViewAllStudents(); // Show list to choose from
        Console.Write("Enter the student number to calculate average: ");
        string input = Console.ReadLine();

        if (int.TryParse(input, out int index) && index >= 1 && index <= students.Count)
        {
            double average = students[index - 1].CalculateAverage();
            Console.WriteLine($"Average grade for {students[index - 1].Name}: {average:F2}");
        }
        else
        {
            Console.WriteLine("Invalid student number.");
        }
    }

    // Helper function to get a valid grade (0-100)
    public static double GetValidGrade(string prompt)
    {
        double grade;
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (double.TryParse(input, out grade) && grade >= 0 && grade <= 100)
            {
                return grade;
            }
            else
            {
                Console.WriteLine("Invalid grade. Please enter a number between 0 and 100.");
            }
        }
    }
}
