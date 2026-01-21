using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

enum Priority
{
    Low,
    Medium,
    High
}

class Program
{
    static void Main(string[] args)
    {
        bool exitMainWindow = false;
        while (!exitMainWindow)
        {
            ShowMainMenu();

            string mainChoice = Console.ReadLine();

            switch (mainChoice)
            {
                case "1":
                    RunTaskManagementSystem();
                    break;
                case "2":
                    RunStudentGradeManagementSystem();
                    break;
                case "3":
                    exitMainWindow = true;
                    ExitSystem();
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select 1-3.");
                    break;
            }
        }
    }

    // ===== Main Menu =====
    static void ShowMainMenu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n==========================================================");
        Console.WriteLine("     List of Projects     ");
        Console.WriteLine("==========================================================");
        Console.WriteLine("1. Task Management System");
        Console.WriteLine("2. Student Grading System");
        Console.WriteLine("3. Exit");
        Console.Write("Choose an option: ");
        Console.ResetColor();
    }

    // ===== Task Management System =====
    static void RunTaskManagementSystem()
    {
        TaskItem.LoadTasksFromFile();
        bool exitTMS = false;

        while (!exitTMS)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n******************************");
            Console.WriteLine("     TASK MANAGEMENT SYSTEM     ");
            Console.WriteLine("******************************");
            Console.WriteLine("1. Add Task");
            Console.WriteLine("2. View All Tasks");
            Console.WriteLine("3. Mark Task as Completed");
            Console.WriteLine("4. Delete Task");
            Console.WriteLine("5. Filter Tasks by Priority");
            Console.WriteLine("6. Sort Tasks by Due Date");
            Console.WriteLine("7. Save Task to File");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an option: ");
            Console.ResetColor();

            string choiceTMS = Console.ReadLine();

            switch (choiceTMS)
            {
                case "1":
                    TaskItem.AddTask();
                    break;
                case "2":
                    TaskItem.ViewAllTasks();
                    break;
                case "3":
                    TaskItem.MarkTaskCompleted();
                    break;
                case "4":
                    TaskItem.DeleteTask();
                    break;
                case "5":
                    TaskItem.FilterTasksByPriority();
                    break;
                case "6":
                    TaskItem.SortTasksByDueDate();
                    break;
                case "7":
                    TaskItem.SaveTasksToFile();
                    break;
                case "8":
                    exitTMS = true;
                    Console.WriteLine("Exiting the system. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select 1-7.");
                    break;
            }
        }
    }

    // ===== Student Grade Management System =====
    static void RunStudentGradeManagementSystem()
    {
        bool exitGMS = false;

        while (!exitGMS)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("\n******************************");
            Console.WriteLine("     Student Grade Management System     ");
            Console.WriteLine("******************************");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View All Students");
            Console.WriteLine("3. Add Grade to Student");
            Console.WriteLine("4. Calculate Average Grade for Student");
            Console.WriteLine("5. Exit");
            Console.Write("Choose an option: ");
            Console.ResetColor();

            string choiceGMS = Console.ReadLine();

            switch (choiceGMS)
            {
                case "1":
                    Student.AddStudent();
                    break;
                case "2":
                    Student.ViewAllStudents();
                    break;
                case "3":
                    Student.AddGradeToStudent();
                    break;
                case "4":
                    Student.CalculateAverageForStudent();
                    break;
                case "5":
                    exitGMS = true;
                    Console.WriteLine("Exiting the system. Goodbye!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please select 1-5.");
                    break;
            }
        }
    }

    // ===== Exit =====
    static void ExitSystem()
    {
        Console.WriteLine("Exiting the entire system. Goodbye!");
    }
}