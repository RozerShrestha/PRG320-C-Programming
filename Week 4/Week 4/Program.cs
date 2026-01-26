using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;
using System.Threading.Tasks;
using Week_4.FileSystem;

class Program
{
    static async Task Main(string[] args)
    {
        #region Write into or Create File

        File.WriteAllText("sample.txt", "This is a sample content"); //simply write text to a file
        File.WriteAllLines("Line.txt", new string[] { "Line 1", "Line 2", "Line 3" }); //write multiple lines to a file
        File.AppendAllText("sample.txt", "\nThis is appended text"); //append text to an existing file)

        #endregion

        #region Read From File

        string text =File.ReadAllText("sample.txt");
        Console.WriteLine(text);
        string[] lines=File.ReadAllLines("Line.txt");
        Console.WriteLine(lines);
        byte[] bytes=File.ReadAllBytes("sample.txt");
        Console.WriteLine(bytes);

        #endregion

        #region Copy/ Move / Delete File
        File.Copy("a.txt", "b.txt", overwrite: true);
        File.Move("a.txt", "archive/a.txt");
        File.Delete("temp.txt");
        #endregion

        bool exists=File.Exists("sample.txt"); //this checks if the file exists or not


        #region Get File Info

         FileInfo fileInfo=new FileInfo("example.txt");
         var extension = fileInfo.Extension; //check extension of the file


        #endregion


        #region Directory

        Directory.CreateDirectory("Logs"); //create a Folder called Logs
        //Directory.Delete("Logs", recursive: false); // recursive false means it will delete only if the folder is empty
        string[] files = Directory.GetFiles("Logs");
        string[] folders = Directory.GetDirectories("Logs");

        DirectoryInfo dir = new DirectoryInfo("Logs"); // its similar to FileInfo but for directories

        #endregion


        #region using StreamReader and StreamWriter (Mainly used for larger file, performance oriented)
        //FileStream works with Binary data not string
        using (FileStream fileStream = new FileStream("largefile.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite))
        {
            for (int i = 0; i < 10000; i++)
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes($"This is line number {i}\n"); // converts string to byte array using UTF-8 encoding
                await fileStream.WriteAsync(data, 0, data.Length);
            }
            await fileStream.FlushAsync();
           
        }
        //write
        using StreamWriter writer = new StreamWriter("data.txt", append: true);
        writer.WriteLine("Hello");

        writer.Close();
        //StreamReader
        using StreamReader reader = new StreamReader("largefile.txt");
    //string allText = reader.ReadToEnd();
    // Console.WriteLine(allText);

    //ID: 101, Name: Alice, Department: HR
    //ID: 102, Name: Bob, Department: IT
    //ID: 103, Name: Charlie, Department: Finance
    //ID: 104, Name: Diana, Department: Marketing
    //ID: 105, Name: Ethan, Department: IT




        string? line;
        while((line = reader.ReadLine()) != null)
        {
            line = line.Trim();

            //if (line.Contains("Finance"))
            {
                int startIndex= line.IndexOf("Name: ")+6;
                int lastIndex= line.IndexOf(", Department");
                Console.WriteLine(line.Substring(startIndex: startIndex, length: lastIndex-startIndex));
            }
            Console.WriteLine(line);
        }

        /*
        Write a project that reads text from the file. You will be given a sample text
        Extract Name from each line
        Extract Department
        Create a Dictionary<string, string> set department as a key and value as Name
        After you put all the extracted name and department, you will be given an Email Template
        Loop through the dictionary and create dynamic email template according to the key and value
        Print the generated email template using Console.WriteLine();

        Email Template:
        Hello name,

        You are assigned to Department:department

        Thank you,
        Kings College

        */








        #endregion

        #region reading json data
        string studentData = File.ReadAllText("example.txt");
        Student student = JsonSerializer.Deserialize<Student>(studentData);

        Console.WriteLine($"ID: {student.Id}- FullName: {student.FullName} - Age: {student.Age} - Email: {student.Email} - PhoneNumber: {student.PhoneNumber} - DOB: {student.DateOfBirth}");

        Console.WriteLine($"List of Courses: ");
        foreach(var course in student.Courses)
        {
            Console.WriteLine($"CourseID: {course.CourseId} Title: {course.Title} Credits: {course.Credits} Instructor: {course.Instructor}");
        }
        Console.ReadLine();
        #endregion

        

    }
}

