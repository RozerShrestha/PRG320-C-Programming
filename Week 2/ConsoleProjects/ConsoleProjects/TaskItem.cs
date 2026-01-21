using System.Threading.Tasks;

class TaskItem
{
    public string Title { get; set; }
    public string Description { get; set; }
    public Priority Priority { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    static List<TaskItem> tasks = new List<TaskItem>();
    static string filePath = "tasks.txt";


    public TaskItem()
    {
        
    }
    public TaskItem(string title, string description, Priority priority, DateTime dueDate)
    {
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        IsCompleted = false;
    }

    public override string ToString()
    {
        //return $"Title: {Title} | Description: {Description} | Priority: {Priority} | Due Date: {DueDate:yyyy-MM-dd} | Status: {(IsCompleted ? "Completed" : "Pending")}";

        // Save current color
        var headerColor = ConsoleColor.Yellow;
        var defaultColor = ConsoleColor.White;
        

        // Title
        Console.ForegroundColor = headerColor;
        Console.Write("Title: ");
        Console.ForegroundColor = defaultColor;
        Console.Write($"{Title} | ");

        // Description
        Console.ForegroundColor = headerColor;
        Console.Write("Description: ");
        Console.ForegroundColor = defaultColor;
        Console.Write($"{Description} | ");

        // Priority
        Console.ForegroundColor = headerColor;
        Console.Write("Priority: ");
        Console.ForegroundColor = defaultColor;
        Console.Write($"{Priority} | ");

        // Due Date
        Console.ForegroundColor = headerColor;
        Console.Write("Due Date: ");
        Console.ForegroundColor = defaultColor;
        Console.Write($"{DueDate:yyyy-MM-dd} | ");

        // Status
        Console.ForegroundColor = headerColor;
        Console.Write("Status: ");
        Console.ForegroundColor = defaultColor;
        Console.WriteLine(IsCompleted ? "Completed" : "Pending");

        // Reset at the end
        Console.ResetColor();

        return string.Empty; // since we already wrote to console

    }

    // Function to add a new task
    public static void AddTask()
    {
        Console.Write("Enter task title: ");
        string title = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Title cannot be empty.");
            return;
        }

        Console.Write("Enter task description: ");
        string description = Console.ReadLine();

        Priority priority = GetValidPriority();

        DateTime dueDate = GetValidDate("Enter due date (yyyy-MM-dd): ");

        tasks.Add(new TaskItem(title, description, priority, dueDate));
        Console.WriteLine("Task added successfully.");
    }

    // Function to view all tasks
    public static void ViewAllTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks available.");
            return;
        }
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\nAll Tasks:");
        Console.ResetColor();
        for (int i = 0; i < tasks.Count; i++)
        {
            Console.Write($"{i + 1}. ");
            tasks[i].ToString(); // this will print the task details directly

        }
        Console.WriteLine("\nAll Tasks using Foreach:");
        int index = 1;
        foreach(var task in tasks)
        {
            Console.Write($"{index}. ");
            task.ToString(); // this will print the task details directly
            index++;

        }
        Console.WriteLine("\nAll Tasks using Foreach advance:");
        foreach (var task in tasks.Select((task,index)=> new { task, index }))
        {
            Console.Write($"{task.index + 1}. ");
            task.task.ToString(); // prints task details directly

        }
    }

    // Function to mark a task as completed
    public static void MarkTaskCompleted()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks available.");
            return;
        }

        ViewAllTasks();
        Console.Write("Enter the task number to mark as completed: ");
        string input = Console.ReadLine();

        if (int.TryParse(input, out int index) && index >= 1 && index <= tasks.Count)
        {
            tasks[index - 1].IsCompleted = true;
            Console.WriteLine("Task marked as completed.");
        }
        else
        {
            Console.WriteLine("Invalid task number.");
        }
    }

    // Function to delete a task
    public static void DeleteTask()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("No tasks available.");
            return;
        }

        ViewAllTasks();
        Console.Write("Enter the task number to delete: ");
        string input = Console.ReadLine();

        if (int.TryParse(input, out int index) && index >= 1 && index <= tasks.Count)
        {
            tasks.RemoveAt(index - 1);
            Console.WriteLine("Task deleted.");
        }
        else
        {
            Console.WriteLine("Invalid task number.");
        }
    }

    // Function to filter tasks by priority
    public static void FilterTasksByPriority()
    {
        Priority priority = GetValidPriority();

        var filteredTasks = tasks.Where(t => t.Priority == priority).ToList();

        if (filteredTasks.Count == 0)
        {
            Console.WriteLine($"No tasks with {priority} priority.");
            return;
        }

        Console.WriteLine($"\nTasks with {priority} priority:");
        for (int i = 0; i < filteredTasks.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {filteredTasks[i]}");
        }
    }

    // Function to sort tasks by due date
    public static void SortTasksByDueDate()
    {
        tasks = tasks.OrderBy(t => t.DueDate).ToList();
        Console.WriteLine("Tasks sorted by due date.");
        ViewAllTasks();
    }

    // Helper function to get a valid priority
    public static Priority GetValidPriority()
    {
        while (true)
        {
            Console.Write("Enter priority (Low, Medium, High): ");
            string input = Console.ReadLine();

            if (Enum.TryParse(input, true, out Priority priority))
            {
                return priority;
            }
            else
            {
                Console.WriteLine("Invalid priority. Please enter Low, Medium, or High.");
            }
        }
    }

    // Helper function to get a valid date
    public static DateTime GetValidDate(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string input = Console.ReadLine();

            if (DateTime.TryParse(input, out DateTime date))
            {
                return date;
            }
            else
            {
                Console.WriteLine("Invalid date format. Please use yyyy-MM-dd.");
            }
        }
    }

    // Function to load tasks from file
    public static void LoadTasksFromFile()
    {
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            foreach (string line in lines)
            {
                string[] parts = line.Split('|');
                if (parts.Length == 5)
                {
                    string title = parts[0].Trim();
                    string description = parts[1].Trim();
                    Priority priority = (Priority)Enum.Parse(typeof(Priority), parts[2].Trim());
                    DateTime dueDate = DateTime.Parse(parts[3].Trim());
                    bool isCompleted = parts[4].Trim() == "Completed";

                    TaskItem task = new TaskItem(title, description, priority, dueDate);
                    task.IsCompleted = isCompleted;
                    tasks.Add(task);
                }
            }
        }
    }

    // Function to save tasks to file
    public static void SaveTasksToFile()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            foreach (TaskItem task in tasks)
            {
                writer.WriteLine(task.ToString());
            }
        }
    }
}
