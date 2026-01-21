public class Vehicle
{
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int Year { get; set; }
    public string? Color { get; set; }

    public void Start()
    {
        Console.WriteLine($"{Brand} {Model} is starting...");
    }

    public void Stop()
    {
        Console.WriteLine($"{Brand} {Model} is stopping...");
    }
}
