using LibraryManagementSystem;

class Program
{
    static void Main()
    {
        try
        {
            Book book = new Book
            {
                Title = "C# Fundamentals",
                Author = "John Doe",
                Publisher = "TechPress",
                PublicationYear = 2024
            };

            Magazine magazine = new Magazine
            {
                Title = "Tech Monthly",
                IssueNumber = -5, // ❌ Invalid input
                Publisher = "FutureMedia",
                PublicationYear = 2025
            };

            book.DisplayInfo();
            magazine.DisplayInfo();
        }
        catch (InvalidItemDataException ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }

    }
}
